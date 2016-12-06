using UnityEngine;
using System.Collections;

public class LeverAnimator : MonoBehaviour 
{
	//Keeps track of whether the lever is up or down.
	public bool leverUp;
	private bool leverUpPrev;

	private Animator anim;

	void Start()
	{
		leverUp = true;
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		if (leverUp && !leverUpPrev) 
		{
			anim.SetTrigger ("LeverUp_Trigger");
		}
		if (!leverUp && leverUpPrev) 
		{
			anim.SetTrigger ("LeverDown_Trigger");
		}

		leverUpPrev = leverUp;
	}
}