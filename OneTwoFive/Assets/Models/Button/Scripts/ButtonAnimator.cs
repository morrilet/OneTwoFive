using UnityEngine;
using System.Collections;

public class ButtonAnimator : MonoBehaviour 
{
	//Keeps track of whether the lever is up or down.
	public bool buttonPressed;
	private bool buttonPressedPrev;

	private Animator anim;

	void Start()
	{
		buttonPressed = false;
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		if (buttonPressed && !buttonPressedPrev) 
		{
			anim.SetTrigger ("ButtonPressed_Trigger");
		}
		if (!buttonPressed && buttonPressedPrev) 
		{
			anim.SetTrigger ("ButtonReleased_Trigger");
		}

		buttonPressedPrev = buttonPressed;
	}
}