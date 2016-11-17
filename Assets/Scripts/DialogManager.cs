using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {
	Dictionary<string,Dialogue> Dialogues;
	Dialogue CurrentDialogue;

	[SerializeField]
	public GameObject DialogText;
	[SerializeField]
	public GameObject DialogPanel;

	[SerializeField]
	public List<GameObject> Options;

	// Use this for initialization
	void Start () {
		Dialogues = GetComponent<XmlParser> ().Dialogues;
		ConstructDialog ("1");
	}

	void Update(){
		if (DialogPanel.activeSelf == true) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (CurrentDialogue.Options.Count > 0)
					ChooseDialogTree (CurrentDialogue.Options [0]);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2)){
				if (CurrentDialogue.Options.Count > 1)
					ChooseDialogTree (CurrentDialogue.Options [1]);
			}
			if(Input.GetKeyDown(KeyCode.Alpha3)){
				if (CurrentDialogue.Options.Count > 2)
					ChooseDialogTree (CurrentDialogue.Options [2]);
			}
		}
	}

	public void ChooseDialogTree(Option option)
	{
		//zerowanie tekstu opcji
		foreach (GameObject obj in Options)
			obj.GetComponent<Text> ().text = "";
		
		ConstructDialog (option.ID);
	}

	public void ConstructDialog(string ID)
	{
		CurrentDialogue = Dialogues [ID];
		ActivateDialogPanel();

		DialogText.GetComponent<Text> ().text = CurrentDialogue.Text;

		int iterator = 0;
		foreach (GameObject obj in Options) {
			if (CurrentDialogue.Options.Count > iterator) {
				Debug.Log ("opcja");
				obj.GetComponent<Text> ().text = CurrentDialogue.Options [iterator].Text;
				iterator++;
			}
		}
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
