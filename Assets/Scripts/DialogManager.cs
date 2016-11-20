using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
	public Dictionary<string,Dialogue> Dialogues;
	public Dictionary<string,bool> Flags;
	Dialogue CurrentDialogue;

	//static reference to this instance all over the namespace!
	public static DialogManager DM;
	DialogSoundManager DSM;

	public GameObject DialogSpeaker;
	public GameObject DialogText;
	public GameObject DialogPanel;

	public List<GameObject> OptionTextBoxes;

	int previouslySelectedOption;
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
	}

	void Update(){
		//scrool input
		if (DialogPanel.activeSelf) {
			OptionTextBoxes [currentlySelectedOption].GetComponentInParent<Animator> ().SetBool ("IsScrollOnThis", false);
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				currentlySelectedOption = (currentlySelectedOption + 1) % CurrentDialogue.Options.Count;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				currentlySelectedOption -= 1;
				if (currentlySelectedOption == -1)
					currentlySelectedOption = CurrentDialogue.Options.Count - 1;
			}

			OptionTextBoxes [currentlySelectedOption].GetComponentInParent<Animator> ().SetBool ("IsScrollOnThis", true);

			if (Input.GetMouseButtonDown (0)) {
				OptionTextBoxes [currentlySelectedOption].GetComponentInParent<Animator> ().SetBool ("IsScrollOnThis", false);

				foreach (GameObject obj in OptionTextBoxes) {
					obj.GetComponentInParent<Animator> ().SetBool ("IsIdle", false);
				}

				ChooseDialogTree (CurrentDialogue.Options [currentlySelectedOption]);
				currentlySelectedOption = 0;
			}
		}
		//choose dialog based on keys
		/*if (DialogPanel.activeSelf == true && Input.anyKeyDown) {
			if (Input.inputString.Length > 0) {
				char c = Input.inputString [0];
				if (char.IsDigit (c)) {
					int value = (int)char.GetNumericValue (c);
					if (CurrentDialogue.Options.Count >= value) {
						Debug.Log (c);
						ChooseDialogTree (CurrentDialogue.Options [value - 1]);
					}
				}
			}
		}*/
	}

	public void ChooseDialogTree(Option option)
	{
		//zerowanie tekstu opcji
		foreach (GameObject obj in OptionTextBoxes)
			obj.GetComponent<Text> ().text = "";
		
		ConstructDialog (option.ID);
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
				obj.GetComponent<Text> ().text = CurrentDialogue.Options [iterator].Text;
				iterator++;
				obj.GetComponentInParent<Animator> ().SetBool ("IsIdle", true);
			}
		}
		
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

	public void DeActivateDialogPanel()
	{
		DialogPanel.SetActive (false);
	}
	public void ActivateDialogPanel()
	{
		DialogPanel.SetActive (true);
	}
}
