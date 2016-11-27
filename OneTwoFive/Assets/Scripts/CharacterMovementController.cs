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

		foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor")) 
		{
			if (floor.GetComponent<Collider> ().Raycast (ray, out hit, Mathf.Infinity)) 
			{
				return hit.point;
			}
		}
		return this.transform.position;
	}
}
