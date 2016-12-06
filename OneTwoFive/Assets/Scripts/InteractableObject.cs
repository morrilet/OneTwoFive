using UnityEngine;
using System.Collections;

//This was done real quick and dirty. If there's time, clean it up some.
public class InteractableObject : MonoBehaviour 
{
	public delegate void Interaction();
	public event Interaction InteractionEnabled;

	public Vector3 interactionPosition; //Where the navmesh agent goes to when the object is clicked.

	void Start()
	{
		InputManager.OnLMBDown += TryMoveToInteractionPosition;
	}

	/// <summary>
	/// Checks whether or not the objects collider was clicked.
	/// </summary>
	bool CheckClicked()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit ();
		Physics.Raycast (ray, out hit, Mathf.Infinity);
		if (hit.transform != null)
		{
			if(hit.transform == this.GetComponent<Transform>())
				return true;
		}
		return false;
	}

	//Moves the player to the interaction position if the object was clicked.
	void TryMoveToInteractionPosition()
	{
		if (CheckClicked ()) 
		{
			//Do this without find!
			Debug.Log (this.transform.name + " clicked!");
			GameObject.FindWithTag ("Player").GetComponent<NavMeshAgent> ().destination = interactionPosition;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube (interactionPosition, new Vector3 (.15f, .15f, .15f));
	}
}
