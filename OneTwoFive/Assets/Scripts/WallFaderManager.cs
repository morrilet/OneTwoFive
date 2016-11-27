using UnityEngine;
using System.Collections;

public class WallFaderManager : MonoBehaviour 
{
	CameraController cameraObj; //Used for getting the camera angle from CameraController.
	WallFader[] wallFaderObjects; //All the objects with a WallFader script attached.

	//How long it takes to fully fade a wall.
	public const float FADE_DURATION = .5f;

	void Start()
	{
		cameraObj = Camera.main.transform.parent.GetComponent<CameraController> ();
		wallFaderObjects = GameObject.FindObjectsOfType<WallFader> ();

		//Set all the walls to use z-writing. This is because materials set to Fade will
		//not do this by default and it looks wonky.
		foreach (WallFader wall in wallFaderObjects)
		{
			wall.GetComponent<Renderer> ().material.SetInt ("_ZWrite", 1);
		}

		FadeWalls ();
		CameraController.OnRotationBegin += FadeWalls;
	}

	//This will check for any walls that must be faded out or in and fade them appropriately.
	void FadeWalls()
	{
		foreach (WallFader wall in wallFaderObjects) 
		{
			bool angleMatch = false; //True if any fade angle matches the camera angle.
			foreach (int fadeAngle in wall.fadeAngles) 
			{
				if (fadeAngle == cameraObj.CurrentAngle)
					angleMatch = true;
			}

			if (angleMatch) 
			{
				StartCoroutine (FadeWallOut (wall.gameObject));
			} 
			else
			{
				StartCoroutine (FadeWallIn (wall.gameObject));
			}
		}
	}

	//Fades a wall out to full transparency.
	IEnumerator FadeWallOut(GameObject wall)
	{
		Material wallMat = wall.GetComponent<Renderer> ().material;
		Color startColor = wallMat.color;

		wall.GetComponent<WallFader> ().isFaded = true;
		float t = 0;
		while (t <= FADE_DURATION) 
		{
			Color newColor = wallMat.color;
			newColor.a = 0f;
			float percentComplete = t / FADE_DURATION;

			wallMat.color = Color.Lerp (startColor, newColor, percentComplete);

			wall.GetComponent<Renderer> ().material.color = wallMat.color;
			t += Time.deltaTime;
			yield return null;
		}
	}

	//Fades a wall in to fully opaque.
	IEnumerator FadeWallIn(GameObject wall)
	{
		Material wallMat = wall.GetComponent<Renderer> ().material;
		Color startColor = wallMat.color;

		wall.GetComponent<WallFader> ().isFaded = false;
		float t = 0;
		while (t <= FADE_DURATION) 
		{
			Color newColor = wallMat.color;
			newColor.a = 1f;
			float percentComplete = t / FADE_DURATION;

			wallMat.color = Color.Lerp (startColor, newColor, percentComplete);

			wall.GetComponent<Renderer> ().material.color = wallMat.color;
			t += Time.deltaTime;
			yield return null;
		}
	}
}
