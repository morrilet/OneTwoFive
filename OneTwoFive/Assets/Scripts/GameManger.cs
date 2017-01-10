using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManger : MonoBehaviour 
{
	[SerializeField]
	protected SequenceManager sequenceManager;
	const float TIME_BETWEEN_SEQUENCE_NOTES = .75f;
	bool playingSequence = false;

	[SerializeField]
	protected SceneChangeFadeManager sceneFadeManager;
	[SerializeField]
	protected AudioManager audioManager; //This is here b/c PlaySequence was being called before the 
									     //audio manager had initialized, so we want to call it's start method early.

	void Start()
	{
		//Init managers that need it.
		audioManager.Start ();
		sceneFadeManager.Start ();

		StartScene ();
		InputManager.OnActionDown += PlaySequence;
	}

	public void PlaySequence()
	{
		if (!playingSequence) 
		{
			StartCoroutine (PlaySequenceCoroutine ());
		}
	}

	private IEnumerator PlaySequenceCoroutine()
	{
		playingSequence = true;
		for (int i = 0; i < sequenceManager.referenceSequence.Count; i++) 
		{
			if (sequenceManager.referenceSequence [i].y == 0)      //Button Pressed
			{
				AudioManager.PlayClip (AudioManager.buttonClip);
			} 
			else if (sequenceManager.referenceSequence [i].y == 1) //Button Released
			{
			} 
			else if (sequenceManager.referenceSequence [i].y == 2) //Lever Up
			{
				AudioManager.PlayClip (AudioManager.leverUpClip);
			} 
			else if (sequenceManager.referenceSequence [i].y == 3) //Lever Down
			{
				AudioManager.PlayClip (AudioManager.leverDownClip);
			}
			yield return new WaitForSeconds (TIME_BETWEEN_SEQUENCE_NOTES);
		}
		playingSequence = false;
	}

	public void StartScene()
	{
		StartCoroutine (StartSceneCoroutine ());
	}

	private IEnumerator StartSceneCoroutine()
	{
		sceneFadeManager.FadeSceneIn ();
		yield return new WaitForSeconds (sceneFadeManager.GetTotalDuration ());
		PlaySequence ();
	}

	public void CloseScene(string sceneToLoad)
	{
		StartCoroutine (CloseSceneCoroutine (sceneToLoad));
	}

	private IEnumerator CloseSceneCoroutine(string sceneToLoad)
	{
		sceneFadeManager.FadeSceneOut ();
		yield return new WaitForSeconds (sceneFadeManager.GetTotalDuration ());
		SceneManager.LoadScene (sceneToLoad);
	}
}

















