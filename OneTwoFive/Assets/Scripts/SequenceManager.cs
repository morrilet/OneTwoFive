using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceManager : MonoBehaviour 
{
	public delegate void SequenceMatchAction();
	public static event SequenceMatchAction OnSequenceMatch;

	public List<Vector2> referenceSequence;
	private static SequenceReader reader;
	private bool sequenceMatch;

	void Start()
	{
		reader = this.GetComponent<SequenceReader> ();
		sequenceMatch = false;
	}

	void Update()
	{
		if (!sequenceMatch) 
		{
			sequenceMatch = CheckSequenceMatch ();
		}

		if (sequenceMatch)
		{
			Debug.Log ("Match!");
			if (OnSequenceMatch != null) 
			{
				OnSequenceMatch ();
			}
			reader.ClearSequence ();
			sequenceMatch = false;
		}
	}

	public bool CheckSequenceMatch()
	{
		bool match = false;

		if (reader.GetCurrentSequence ().Length == referenceSequence.Count)
		{
			int matchCount = 0;
			for (int i = 0; i < referenceSequence.Count; i++)
			{
				if (referenceSequence [i] == reader.GetCurrentSequence () [i].getFormattedData ())
				{
					matchCount++;
				}
			}
			if (matchCount == referenceSequence.Count)
			{
				match = true;
			}
		}

		return match;
	}

	public static SequenceReader GetSequenceReader()
	{
		return reader;
	}
}
