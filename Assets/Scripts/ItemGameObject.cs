using UnityEngine;
using System.Collections;

public class ItemGameObject : MonoBehaviour{

	public string ID;
	private Item ItemData;

	void Start(){
		this.ItemData = XmlParser.XP.GetItemData (ID);
		Debug.Log("moje id to:"  + ItemData.ID);
		Debug.Log ("nazwa" + ItemData.Name);
		Debug.Log ("opis " + ItemData.Description);
		Debug.Log ("movable " + ItemData.Movable);
		Debug.Log ("invokeDialog" + ItemData.InvokeDialog);
	}
}
