using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Reads sequences from interactable objects and creates an array of sequence data.
/// </summary>
public class SequenceReader : MonoBehaviour 
{
	public struct SequenceData
	{
		private int identifier;
		private int eventID;

		public SequenceData(int _identifier, int _eventID)
		{
			identifier = _identifier;
			eventID = _eventID;
		}

		//Returns a vector (ID, EventID)
		public Vector2 getFormattedData()
		{
			Vector2 result = new Vector2 (identifier, eventID);
			return result;
		}

		public int getIdentifier () { return identifier; }
		public int getEventID () { return eventID; }
	}

	private static List<SequenceData> currentSequence;
	private const float CLEAR_ON_INACTVE_TIME = 3.0f; //How much inactive time until clearing the current sequence.
	private static float inactiveTime;

	public enum EventIDs
	{
		buttonPressed    = 0,
		buttonReleased   = 1,
		leverFlippedUp   = 2,
		leverFlippedDown = 3
	}

	void Start()
	{
		currentSequence = new List<SequenceData> ();
	}

	void Update()
	{
		//Iterate inactive time.
		if (currentSequence.Count > 0) 
		{
			inactiveTime += Time.deltaTime;
		} 
		else 
		{
			inactiveTime = 0;
		}

		if (inactiveTime >= CLEAR_ON_INACTVE_TIME) 
		{
			ClearSequence ();
		}

		//Debugging/Testing...
		/*
		for (int i = 0; i < currentSequence.Count; i++) 
		{
			Debug.Log ("Sequence Item " + i + " :: ID = " + currentSequence [i].getIdentifier () + "; EventID = " + (EventIDs)currentSequence [i].getEventID () + ";");
		}
		*/
	}

	public void ClearSequence()
	{
		currentSequence.RemoveRange (0, currentSequence.Count);
		currentSequence = new List<SequenceData> ();
	}

	public void AppendToSequence(SequenceData data)
	{
		currentSequence.Add (data);
		inactiveTime = 0.0f;
		Debug.Log ("Appending... (" + data.getIdentifier() + ", " + data.getEventID() + ")");
	}

	public SequenceData[] GetCurrentSequence()
	{
		return currentSequence.ToArray ();
	}
}