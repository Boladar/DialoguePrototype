using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	GameObject GetMouseHoverObject(float range)
	{
		Vector3 position = gameObject.transform.position;
		RaycastHit raycastHit;
		Vector3 targert = position + Camera.main.transform.forward;
		if(Physics.Linecast(position,targert,out raycastHit))
			return raycastHit.collider.gameObject;
		else
			return null;
	}
}

