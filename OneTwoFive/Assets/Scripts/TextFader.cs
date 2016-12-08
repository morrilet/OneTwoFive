using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFader : MonoBehaviour 
{
	public AnimationCurve fadeCurve;
	public float fadeTime;
	public float fadeValue;
	Text text;

	void Start()
	{
		text = GetComponent<Text> ();
	}

	public void Fade()
	{
		StartCoroutine (FadeCoroutine (fadeTime, fadeValue));
	}
		
	/// <summary>
	/// Fades the text from its current alpha value to the specified alpha value
	/// in the specified duration.
	/// </summary>
	/// <param name="duration">Duration for the fade.</param>
	/// <param name="alpha">The alpha value to fade to.</param>
	private IEnumerator FadeCoroutine(float duration, float alpha)
	{
		float percentage = 0.0f;
		float timeCount = 0.0f;
		Color startColor = text.color;
		Color tempColor = startColor;

		while (timeCount < duration) 
		{
			Debug.Log (fadeCurve.Evaluate(percentage));
			percentage = timeCount / duration;
			tempColor.a = Mathf.Lerp(startColor.a, alpha, fadeCurve.Evaluate(percentage));
			text.color = tempColor;
			timeCount += Time.deltaTime;
			yield return null;
		}

		tempColor.a = alpha;
		text.color = tempColor;
	}
}
