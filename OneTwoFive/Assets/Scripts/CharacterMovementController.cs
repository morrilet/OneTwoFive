using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour 
{
	NavMeshAgent agent;

	void Start()
	{
		agent = GetComponent<NavMeshAgent> ();
		InputManager.OnLMBDown += SetDestination;
	}

	void SetDestination()
	{
		agent.destination = GetMousePosition ();
	}

	//Gets the mouse position in world coordinates.
	Vector3 GetMousePosition()
	{
		Vector3 result = Vector3.zero;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit ();

		Physics.Raycast (ray, out hit, Mathf.Infinity);
		if (hit.transform != null) 
		{
			if(hit.transform.tag == "Floor")
			return hit.point;
		}
		return agent.destination;
	}
}
