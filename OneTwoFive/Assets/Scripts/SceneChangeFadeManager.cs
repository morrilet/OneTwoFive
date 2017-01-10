using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This class handles the combined fading of the text and background
/// GUI elements used at scene changes.
/// </summary>
public class SceneChangeFadeManager : MonoBehaviour 
{
	[SerializeField]
	protected TextFader textFader;
	[SerializeField]
	protected ImageFader backgroundFader;

	private float totalDuration; //How long it takes to perform the entire fading operation.
	public float GetTotalDuration () { return totalDuration; }

	public void Start()
	{
		//Call this here to avoid errors due to no initialization.
		backgroundFader.Start ();
		textFader.Start ();

		totalDuration = textFader.fadeTime + backgroundFader.fadeTime;
	}

	//Fades the scene out by fading the GUI elements in.
	public void FadeSceneIn()
	{
		//Set the GUI elements to fully transparent.
		Color tempColor = textFader.GetComponent<Text>().color;
		tempColor.a = 1.0f;
		textFader.GetComponent<Text> ().color = tempColor;
		tempColor = backgroundFader.GetComponent<Image> ().color;
		tempColor.a = 1.0f;
		backgroundFader.GetComponent<Image> ().color = tempColor;

		//Set the fader values for the GUI elements.
		textFader.fadeValue = 0.0f;
		backgroundFader.fadeValue = 0.0f;

		StartCoroutine (FadeSceneInCoroutine ());
	}

	private IEnumerator FadeSceneInCoroutine()
	{
		//Fade the GUI elements in a staggered fashion.
		backgroundFader.Fade();
		yield return new WaitForSeconds (backgroundFader.fadeTime);
		textFader.Fade();
		yield return new WaitForSeconds (textFader.fadeTime);
	}

	//Fades the scene out by fading the GUI elements in.
	public void FadeSceneOut()
	{
		//Set the GUI elements to fully transparent.
		Color tempColor = textFader.GetComponent<Text>().color;
		tempColor.a = 0.0f;
		textFader.GetComponent<Text> ().color = tempColor;
		tempColor = backgroundFader.GetComponent<Image> ().color;
		tempColor.a = 0.0f;
		backgroundFader.GetComponent<Image> ().color = tempColor;

		//Set the fader values for the GUI elements.
		textFader.fadeValue = 1.0f;
		backgroundFader.fadeValue = 1.0f;

		StartCoroutine (FadeSceneOutCoroutine ());
	}

	private IEnumerator FadeSceneOutCoroutine()
	{
		//Fade the GUI elements in a staggered fashion.
		textFader.Fade();
		yield return new WaitForSeconds (textFader.fadeTime);
		backgroundFader.Fade();
		yield return new WaitForSeconds (backgroundFader.fadeTime);
	}
}
