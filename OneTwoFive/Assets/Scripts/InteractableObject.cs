using UnityEngine;
using System.Collections;

//This was done real quick and dirty. If there's time, clean it up some.
//12/7/16 UPDATE: Still super dirty, but roughly working.
public class InteractableObject : MonoBehaviour 
{
	private delegate void InteractionAction();
	private event InteractionAction StartInteraction;

	public Vector3 interactionPosition; //Where the navmesh agent goes to when the object is clicked.
	private const float BUTTONPRESSDURATION = 2.5f;
	private NavMeshAgent playerNavMeshAgent;

	void Start()
	{
		playerNavMeshAgent = GameObject.FindWithTag ("Player").GetComponent<NavMeshAgent> ();

		InputManager.OnLMBDown += TryInteraction;

		if (this.gameObject.GetComponent<LeverAnimator> () != null) 
		{
			StartInteraction = null;
			StartInteraction = ChangeLeverState;
		}
		if (this.gameObject.GetComponent<ButtonAnimator> () != null) 
		{
			StartInteraction = null;
			StartInteraction = ActivateButton;
		}
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

	void TryInteraction()
	{
		if (CheckClicked ()) 
		{
			StopCoroutine ("TryInteractionCoroutine");
			StartCoroutine ("TryInteractionCoroutine");
		}
	}

	IEnumerator TryInteractionCoroutine()
	{
		MovePlayerToInteractionPosition ();
		yield return new WaitForSeconds (0.1f);
		while (playerNavMeshAgent.gameObject.GetComponent<CharacterMovementController> ().GetCurrentSpeed() > 0)
		{
			yield return null;
		}
		StartInteraction ();
	}

	//Moves the player to the interaction position.
	void MovePlayerToInteractionPosition()
	{
		playerNavMeshAgent.destination = interactionPosition;
	}

	#region Interaction Methods
	void ActivateButton()
	{
		StopCoroutine  ("PressButtonForSeconds");
		StartCoroutine ("PressButtonForSeconds", BUTTONPRESSDURATION);
		AudioManager.PlayClip (AudioManager.buttonClip);
	}

	void ChangeLeverState()
	{
		LeverAnimator leverTemp = GetComponent<LeverAnimator> ();

		leverTemp.leverUp = !leverTemp.leverUp;
		if (leverTemp.leverUp)  { AudioManager.PlayClip (AudioManager.leverUpClip); }
		if (!leverTemp.leverUp) { AudioManager.PlayClip (AudioManager.leverDownClip); }
	}

	IEnumerator PressButtonForSeconds(float seconds)
	{
		GetComponent<ButtonAnimator> ().buttonPressed = true;
		yield return new WaitForSeconds (seconds);
		GetComponent<ButtonAnimator> ().buttonPressed = false;
	}
	#endregion

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube (interactionPosition, new Vector3 (.15f, .15f, .15f));
	}
}
