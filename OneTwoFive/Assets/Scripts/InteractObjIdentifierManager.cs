using UnityEngine;
using System.Collections;

public class InteractObjIdentifierManager : MonoBehaviour 
{
	private InteractableObject[] interactableObjects;

	void Start()
	{
		interactableObjects = FindObjectsOfType<InteractableObject> ();
		for(int i = 0; i < interactableObjects.Length; i++)
		{
			interactableObjects [i].SetIdentifier (i);
		}
	}
}
