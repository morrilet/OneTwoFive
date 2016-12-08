using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFader : MonoBehaviour 
{
	public AnimationCurve fadeCurve;
	public float fadeTime;
	public float fadeValue;
	Image image;

	void Start()
	{
		image = GetComponent<Image> ();
		StartCoroutine (Fade (fadeTime, fadeValue));
	}

	public void Fade()
	{
		StartCoroutine (FadeCoroutine (fadeTime, fadeValue));
	}

	/// <summary>
	/// Fades the image from its current alpha value to the specified alpha value
	/// in the specified duration.
	/// </summary>
	/// <param name="duration">Duration for the fade.</param>
	/// <param name="alpha">The alpha value to fade to.</param>
	private IEnumerator FadeCoroutine(float duration, float alpha)
	{
		float percentage = 0.0f;
		float timeCount = 0.0f;
		Color startColor = image.color;
		Color tempColor = startColor;

		while (timeCount < duration) 
		{
			Debug.Log (fadeCurve.Evaluate(percentage));
			percentage = timeCount / duration;
			tempColor.a = Mathf.Lerp(startColor.a, alpha, fadeCurve.Evaluate(percentage));
			image.color = tempColor;
			timeCount += Time.deltaTime;
			yield return null;
		}

		tempColor.a = alpha;
		image.color = tempColor;
	}
}
