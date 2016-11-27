using UnityEngine;
using System.Collections;

////////////////////////////////////NOTES////////////////////////////////////
/// Okay, so it's working well with set angles to stop at, next up is making
/// it be able to move while rotating. This works already, but once the 
/// current angle wraps aroud, the camera moves the other direction. The fix
/// for this is to make it so that you can only queue up one extra move past
/// the current movement being exectuted. This is a bit of polish, as the
/// camera works well for now.
/////////////////////////////////////////////////////////////////////////////

public class CameraController : MonoBehaviour 
{
	//This is for the wall fader. It is called right after the camera has finished rotating and right as it begins rotating.
	public delegate void RotationAction();
	public static event RotationAction OnRotationEnd;
	public static event RotationAction OnRotationBegin;
	
	public AnimationCurve movementCurve;
	public bool isRotatingCamera;

	private const float ROTATION_DURATION = 0.7f;
	private const float ROTATION_ANGLE = 90f;

	//This is an array of angles that the camera will travel to instead of moving there incrementally.
	Vector3[] angles = new Vector3[4];
	//This is the angle number we're on in the array.
	int currentAngle = 0;

	public int CurrentAngle
	{
		get {return currentAngle;}
	}

	void Start () 
	{
		//movementCurve.
		isRotatingCamera = false;

		//Set angles.
		angles [0] = new Vector3 (30,  45, 0);
		angles [1] = new Vector3 (30, 315, 0);
		angles [2] = new Vector3 (30, 225, 0);
		angles [3] = new Vector3 (30, 135, 0);

		//Attach methods to input manager events.
		InputManager.OnLeftDown  += RotateLeft;
		InputManager.OnRightDown += RotateRight;
	}

	//Clamp the current angle with wrapping.
	private void ClampCurrentAngle()
	{
		if (currentAngle > angles.Length - 1) 
		{
			currentAngle = 0;
		} 
		else if (currentAngle < 0) 
		{
			currentAngle = angles.Length - 1;
		}
	}

	void RotateLeft()
	{
		if (!isRotatingCamera) 
		{
			//Debug.Log ("\nRotateLeft..."); //Testing...
			//Debug.Log ("Previous angle = " + currentAngle); //Testing..

			//Get the next angle to move to.
			currentAngle--;
			ClampCurrentAngle ();

			//Debug.Log ("Current angle = " + currentAngle + " :: AngleWorld = " + angles[currentAngle].y); //Testing...
			StartCoroutine (RotateSmoothlyToAngle (angles [currentAngle].y, ROTATION_DURATION));
		}
	}

	void RotateRight()
	{
		if (!isRotatingCamera) 
		{	
			//Debug.Log ("\nRotateRight..."); //Testing...
			//Debug.Log ("Previous angle = " + currentAngle); //Testing...

			//Get the next angle to move to.
			currentAngle++;
			ClampCurrentAngle ();

			//Debug.Log ("Current angle = " + currentAngle + " :: AngleWorld = " + angles[currentAngle].y); //Testing...
			StartCoroutine (RotateSmoothlyToAngle (angles [currentAngle].y, ROTATION_DURATION));
		}
	}
		
	//Rotates the camera smoothly (and snappily) on the y axis. 
	//Used for turning the camera to another angle for better viewing.
	//NOTE: This turns the cammera by angle, not to angle.
	private IEnumerator RotateSmoothlyByAngle(float _angle, float _duration)
	{
		isRotatingCamera = true;

		float totalTime = 0.0f;
		float percentageComplete = 0.0f;
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 startRot = rot;

		if(OnRotationBegin != null)
			OnRotationBegin ();

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
		if(OnRotationEnd != null)
			OnRotationEnd ();
	}

	//Rotates the camera smoothly (and snappily) on the y axis. 
	//Used for turning the camera to another angle for better viewing.
	//NOTE: This turns the cammera to angle, not by angle.
	private IEnumerator RotateSmoothlyToAngle(float _angle, float _duration)
	{
		isRotatingCamera = true;

		float totalTime = 0.0f;
		float percentageComplete = 0.0f;
		Vector3 rot = transform.rotation.eulerAngles;
		Vector3 startRot = rot;

		if(OnRotationBegin != null)
			OnRotationBegin ();

		while (percentageComplete <= 1.0f) 
		{
			totalTime += Time.deltaTime;
			percentageComplete = totalTime / _duration;

			rot.x = 30f;
			rot.y = Mathf.LerpAngle (startRot.y, _angle, movementCurve.Evaluate (percentageComplete));
			rot.z = 0f;

			transform.rotation = Quaternion.Euler (rot);
			yield return null;
		}

		isRotatingCamera = false;
		if(OnRotationEnd != null)
			OnRotationEnd ();
	}
}
