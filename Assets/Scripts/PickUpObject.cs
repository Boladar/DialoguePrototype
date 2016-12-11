using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour
{
	GameObject grabbedObject;
	float grabbedObjectSize;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log ("lol");
			if (grabbedObject == null){
				Debug.Log ("obiekt pusty");
				TryGrabObject (GetMouseHoverObject (20));
			}
			else
				DropObject();
		}

		if (grabbedObject != null) {
			Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize;
			grabbedObject.transform.position = newPosition;
		}
	}

	bool CanGrab(GameObject target)
	{
		if (target.GetComponent<ItemGameObject> () != null) {
			if (target.GetComponent<ItemGameObject> ().ItemData.Movable)
				return true;
		}
		return false;
	}

	void TryGrabObject(GameObject grabObject)
	{
		if (grabObject == null || !CanGrab(grabObject))
			return;

		grabbedObject = grabObject;
		if(grabObject.GetComponent<Renderer>() != null)
			grabbedObjectSize = grabObject.GetComponent<Renderer> ().bounds.size.magnitude;

	}

	void DropObject()
	{
		if (grabbedObject == null)
			return;

		if (grabbedObject.GetComponent<Rigidbody> () != null)
			grabbedObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		
		grabbedObject = null;
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

