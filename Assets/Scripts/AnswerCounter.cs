using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerCounter : MonoBehaviour {

	private float WaitTime;
	private Image img;
	public bool AnswerTime;

	void Start()
	{
		img = this.GetComponent < Image> ();
		WaitTime = DialogManager.DM.TimeToAnswer;
	}

	public IEnumerator ActivateCounter(float time)
	{
		Debug.Log ("poczatek");
		yield return new WaitForSecondsRealtime (time);
		DialogManager.DM.ChooseDialogTree (new Option ("0", ""));
		Debug.Log ("koniec");
	}

	// Update is called once per frame
	void Update () {	
		if (AnswerTime) {
			img.fillAmount -= 1.0f / WaitTime * Time.fixedDeltaTime;
		}
}
}
