using UnityEngine;
using System.Collections;
using System.Collections.Generic;

////////////////////////////////////NOTES////////////////////////////////////
/// It should be noted that the material of the object to be faded MUST have
/// it's rendering mode set to Fade.
/////////////////////////////////////////////////////////////////////////////

public class WallFader : MonoBehaviour 
{
	/// <summary>
	/// The cameraAngle that the wall will fade for.
	/// -1 = none
	/// 0  =  45
	/// 1  = 135
	/// 2  = 225
	/// 3  = 315
	/// </summary>
	public List<int> fadeAngles = new List<int>();
	[HideInInspector]
	public bool isFaded;

	void Start()
	{
		//Check that fadeAngles are valid.
		foreach (int fadeAngle in fadeAngles) 
		{
			if (fadeAngle < -1 || fadeAngle > 3) 
			{
				Debug.Log ("Invalid fade angle on WallFader :: " + gameObject.name + ", " + fadeAngle);
			}
		}
	}
}
