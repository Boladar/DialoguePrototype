using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
	public Dictionary<string,Dialogue> Dialogues;
	public Dictionary<string,bool> Flags;
	public Dialogue CurrentDialogue;

	Queue<string> DialogueIDQueue;

	//static reference to this instance all over the namespace!
	public static DialogManager DM;
	DialogSoundManager DSM;

	public GameObject DialogSpeaker;
	public GameObject DialogText;
	public GameObject DialogPanel;

	public GameObject TimeBar;

	public List<GameObject> OptionObjects;
	public List<GameObject> OptionTextBoxes;

	public float TimeToAnswer = 30f;
	public bool AnswerTime = false;

	int currentlySelectedOption;

	void Awake(){
		if (DM != null)
			GameObject.Destroy (DM);
		else
			DM = this;

		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		Dialogues = GetComponent<XmlParser> ().Dialogues;
		Flags = GetComponent<XmlParser>().Flags;
		DSM = GetComponent<DialogSoundManager>();

		DialogueIDQueue = new Queue<string>();


		DeActivateDialogueOptions ();
		DeActivateDialogPanel();
	}

	void Update(){
		//scrool input
		if (DialogPanel.activeSelf && OptionObjects[currentlySelectedOption].activeSelf == true) {
			OptionObjects[currentlySelectedOption].GetComponent<Animator> ().SetBool ("IsHighlighted", false);
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				currentlySelectedOption = (currentlySelectedOption + 1) % CurrentDialogue.Options.Count;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				currentlySelectedOption -= 1;
				if (currentlySelectedOption == -1)
					currentlySelectedOption = CurrentDialogue.Options.Count - 1;
			}

			OptionObjects [currentlySelectedOption].GetComponent<Animator> ().SetBool ("IsHighlighted", true);

			if (Input.GetMouseButtonDown (0)) {
				OptionObjects [currentlySelectedOption].GetComponent<Animator> ().SetBool ("IsHighlighted", false);

				foreach (GameObject obj in OptionObjects) {
					obj.GetComponent<Animator> ().SetBool ("IsIdle", false);
				}

				DeActivateDialogPanel ();
				ChooseDialogTree (CurrentDialogue.Options [currentlySelectedOption]);
				currentlySelectedOption = 0;
			}
		}
	}

	IEnumerator WaitForCurrentDialogToFinish(string DialogueIDToCall){
		while (CurrentDialogue != null && DialogueIDQueue.Peek() == DialogueIDToCall) {
			yield return null;
		}

		Debug.Log("dequeue: " + DialogueIDQueue.Dequeue());

		ConstructDialog (DialogueIDToCall);	
	}

	public void ChooseDialogTree(Option option)
	{
		//nullowanie obecnego dialogu
		CurrentDialogue = null;

		//zerowanie licznika
		TimeBar.GetComponent<AnswerCounter>().AnswerTime = false;
		TimeBar.GetComponent<Image>().fillAmount = 1.0f;
		//deaktywacja zbednych pcji
		DeActivateDialogueOptions();

		if (Dialogues.ContainsKey (option.ID))
			ConstructDialog (option.ID);
		else if (option.ID == "0") {
			CurrentDialogue = null;
			DeActivateDialogPanel ();
		}
		else
			Debug.Log ("Dialogues does not contain given key: " + option.ID);
	}

	IEnumerator ConstructMessagesForDialog(Dialogue d){
		foreach (Message ms in d.Messages) {
			DialogText.GetComponent<Text> ().text = ms.Text;
			yield return StartCoroutine (DSM.PlayDialogue (ms.AudioFilename));
		}

		ConstructDialogOptions(d);
	}

	public void ConstructDialogOptions(Dialogue d)
	{
		int iterator = 0;
		foreach (GameObject obj in OptionTextBoxes) {
			if (CurrentDialogue.Options.Count > iterator) {
				//aktywacja potrzebnego obiektu opcji
				OptionObjects [iterator].SetActive (true);
				obj.GetComponent<Text> ().text = CurrentDialogue.Options [iterator].Text;
				iterator++;
				obj.GetComponentInParent<Animator> ().SetBool ("IsIdle", true);
			}
		}
		TimeBar.SetActive (true);
		TimeBar.GetComponent<AnswerCounter>().AnswerTime = true;
		StartCoroutine (TimeBar.GetComponent<AnswerCounter> ().ActivateCounter(TimeToAnswer));
	}

	public void ConstructDialog(string ID, List<string> flagNames)
	{
		foreach (string s in flagNames) {
			if (Flags [s] == false) {
				Debug.Log ("cannot construct dialog: " + s + " flag is set to" + Flags [s]);
				return;
			}
		}

		ConstructDialog (ID);
	}

	public void ConstructDialog(string ID)
	{
		if (!Dialogues.ContainsKey (ID)) {
			Debug.LogError("Dialogues does not contain given ID: " + ID);
			DeActivateDialogPanel();
			return;
		}

		if (CurrentDialogue != null) {
			if (!DialogueIDQueue.Contains(ID)) {
				DialogueIDQueue.Enqueue ( ID);
				Debug.Log ("dialogQueue count" + DialogueIDQueue.Count);
				StartCoroutine (WaitForCurrentDialogToFinish (ID));
			}
			return;
		}

		CurrentDialogue = Dialogues [ID];
		ActivateDialogPanel();

		StartCoroutine (ConstructMessagesForDialog (CurrentDialogue));

		foreach (string flagname in CurrentDialogue.Triggers) {
			FlipFlagValue (flagname);
		}
	}

	public void FlipFlagValue(string flagName)
	{
		if (Flags [flagName] == true)
			Flags [flagName] = false;
		else
			Flags [flagName] = true;
	}

	public void SetFlagValue(string flagName, bool value)
	{
		Flags [flagName] = value;
	}


	public void DeActivateDialogueOptions(){
		foreach (GameObject obj in OptionObjects) {
			obj.SetActive (false);
		}
		TimeBar.SetActive (false);
	}

	public void ActivateDialogueOptions(){
		foreach (GameObject obj in OptionObjects) {
			obj.SetActive (true);
		}
		TimeBar.SetActive (true);
	}

	public void DeActivateDialogPanel()
	{
		DialogPanel.SetActive (false);
	}
	public void ActivateDialogPanel()
	{
		DialogPanel.SetActive (true);
	}
}
