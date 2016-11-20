using UnityEngine;
using System.Collections;

public class ControlOptionPanelSize : MonoBehaviour {

	public GameObject OptionPrefab;
	public GameObject OptionsPanel;

	RectTransform thisTransform;
	RectTransform optionsTransform;

	public float optionHeight;

	float height;

	void Start(){
		thisTransform = this.GetComponent<RectTransform> ();
		optionsTransform = OptionsPanel.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update (){
		if (DialogManager.DM.CurrentDialogue != null) {
			height = DialogManager.DM.CurrentDialogue.Options.Count * optionHeight;	
		}

		thisTransform.sizeDelta = new Vector2 (335, height);
	}
}
