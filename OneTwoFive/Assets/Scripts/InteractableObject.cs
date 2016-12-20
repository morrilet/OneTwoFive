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

	private int identifier; //This is used for identifying the source of sequence data entries.
	public int GetIdentifier  ()          { return identifier; }
	public void SetIdentifier (int value) { identifier = value; }

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
		else 
		{
			StopCoroutine ("TryInteractionCoroutine");
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
		if (!GetComponent<ButtonAnimator> ().buttonPressed) //Ensures that the button can't be pressed again while it's already down.
		{
			StopCoroutine ("PressButtonForSeconds");
			StartCoroutine ("PressButtonForSeconds", BUTTONPRESSDURATION);
			AudioManager.PlayClip (AudioManager.buttonClip);
		}
	}

	void ChangeLeverState()
	{
		LeverAnimator leverTemp = GetComponent<LeverAnimator> ();
		if (leverTemp.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime >= .9999f) //Ensures that the lever can't be flipped without it finishing its animation.
		{
			leverTemp.leverUp = !leverTemp.leverUp;
			if (leverTemp.leverUp)
			{ 
				AudioManager.PlayClip (AudioManager.leverUpClip); 
				SequenceManager.GetSequenceReader ().AppendToSequence (new SequenceReader.SequenceData (identifier, (int)SequenceReader.EventIDs.leverFlippedUp));
			}
			if (!leverTemp.leverUp)
			{ 
				AudioManager.PlayClip (AudioManager.leverDownClip);
				SequenceManager.GetSequenceReader ().AppendToSequence (new SequenceReader.SequenceData (identifier, (int)SequenceReader.EventIDs.leverFlippedDown));
			}
		}
	}

	IEnumerator PressButtonForSeconds(float seconds)
	{
		GetComponent<ButtonAnimator> ().buttonPressed = true;
		SequenceManager.GetSequenceReader ().AppendToSequence (new SequenceReader.SequenceData (identifier, (int)SequenceReader.EventIDs.buttonPressed));
		yield return new WaitForSeconds (seconds);
		SequenceManager.GetSequenceReader ().AppendToSequence (new SequenceReader.SequenceData (identifier, (int)SequenceReader.EventIDs.buttonReleased));
		GetComponent<ButtonAnimator> ().buttonPressed = false;
	}
	#endregion

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube (interactionPosition, new Vector3 (.15f, .15f, .15f));
	}
}
