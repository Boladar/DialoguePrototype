using UnityEngine;
using System.Collections.Generic;

public class InvokeDialog : MonoBehaviour {

	public string DialogID;
	public List<string> Conditions;
	public List<string> Triggers;
	
	void OnTriggerEnter(Collider coll){
		DialogManager.DM.ConstructDialog (DialogID, Conditions);
	}

	void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			Conditions = DialogManager.DM.Dialogues [DialogID].Conditions;
			Triggers = DialogManager.DM.Dialogues [DialogID].Triggers;
		}

	}
}
