using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour 
{
	NavMeshAgent agent;

	//Speed recording stuff...
	float currentSpeed;
	Vector3 previousPosition;

	void Start()
	{
		agent = GetComponent<NavMeshAgent> ();
		InputManager.OnLMBDown += SetDestination;

		//Initialize this stuff...
		previousPosition = transform.position;
		currentSpeed = 0f;
	}

	void Update()
	{
		currentSpeed = Vector3.Distance (transform.position, previousPosition);

		previousPosition = transform.position;
	}

	void SetDestination()
	{
		agent.destination = GetMousePosition ();
	}

	//Gets the mouse position in world coordinates.
	Vector3 GetMousePosition()
	{
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

	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}
}
