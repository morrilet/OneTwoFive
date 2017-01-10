using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public AudioClip _buttonClip;
	public AudioClip _leverUpClip;
	public AudioClip _leverDownClip;

	public static AudioClip buttonClip;
	public static AudioClip leverUpClip;
	public static AudioClip leverDownClip;

	private static AudioSource source;

	public void Start()
	{
		source = GetComponent<AudioSource> ();

		buttonClip    = _buttonClip;
		leverUpClip   = _leverUpClip;
		leverDownClip = _leverDownClip;
	}

	public static void PlayClip(AudioClip clip)
	{
		source.PlayOneShot (clip);
	}
}
