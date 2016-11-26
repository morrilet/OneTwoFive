using UnityEngine;
using System.Collections;

////////////////////////////////////NOTES////////////////////////////////////
/// It may be best to use a series of set rotations to transition to instead
/// of rotating by a set amount. This way there's no chance of math fuckery
/// putting the camera on a rotation that's slightly off.
/////////////////////////////////////////////////////////////////////////////

public class CameraController : MonoBehaviour 
{
	public AnimationCurve movementCurve;
	public bool isRotatingCamera;

	private const float ROTATION_DURATION = 0.7f;
	private const float ROTATION_ANGLE = 90f;

	void Start () 
	{
		//movementCurve.
		isRotatingCamera = false;

		InputManager.OnLeftDown  += RotateLeft;
		InputManager.OnRightDown += RotateRight;
	}

	void RotateLeft()
	{
		if(!isRotatingCamera)
			StartCoroutine (RotateSmoothly (ROTATION_ANGLE, ROTATION_DURATION));
	}

	void RotateRight()
	{
		if(!isRotatingCamera)
			StartCoroutine (RotateSmoothly (-ROTATION_ANGLE, ROTATION_DURATION));
	}

	//Rotates the camera smoothly (and snappily) on the y axis. 
	//Used for turning the camera to another angle for better viewing.
	private IEnumerator RotateSmoothly(float _angle, float _duration)
	{
		isRotatingCamera = true;

		float totalTime = 0.0f;
		float percentageComplete = 0.0f;
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 startRot = rot;

		while (percentageComplete <= 1.0f) 
		{
			totalTime += Time.deltaTime;
			percentageComplete = totalTime / _duration;

			rot.x = 30f;
			rot.y = Mathf.LerpAngle (startRot.y, startRot.y + _angle, movementCurve.Evaluate (percentageComplete));
			rot.z = 0f;

			transform.rotation = Quaternion.Euler (rot);
			yield return null;
		}

		isRotatingCamera = false;
	}
}
