using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
	public delegate void KeyAction();
	public static event KeyAction OnLeftDown;
	public static event KeyAction OnRightDown;

	public delegate void MouseAction();
	public static event MouseAction OnLMBDown;

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

		if (Input.GetMouseButtonDown (0)) 
		{
			if (OnLMBDown != null) 
			{
				OnLMBDown ();
			}
		}
	}
}
