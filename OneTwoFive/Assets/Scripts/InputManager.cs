using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	public delegate void KeyAction();
	public static event KeyAction OnLeftDown;
	public static event KeyAction OnRightDown;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			if (OnLeftDown != null) 
			{
				OnLeftDown ();
			}
		}
		if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			if (OnRightDown != null) 
			{
				OnRightDown ();
			}
		}
	}
}
