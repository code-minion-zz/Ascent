using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{	
	public static MusicManager Instance;
	private static AudioClip towerMusic = Resources.Load("Sounds/music/tower") as AudioClip;
	private static AudioClip bossMusic = Resources.Load("Sounds/music/boss") as AudioClip;

	public delegate void MusicEvent();

	public event MusicEvent musicEnd;

	public enum MusicSelections
	{
		Tower,
		Boss
	}

	void Start()
	{
		if (Instance == null) Instance = this;
		audio.clip = towerMusic;
	}

	public void SwapMusic(MusicSelections choice)
	{
		audio.clip = ParseEnum(choice);
	}

	AudioClip ParseEnum(MusicSelections choice)
	{		
		AudioClip retval = null;
		switch (choice)
		{
		case MusicSelections.Tower:
			retval = towerMusic;
			break;
		case MusicSelections.Boss:
			retval = bossMusic;
			break;
		}
		return retval;
	}

	public void PlayMusic()
	{
		StopCoroutine("FadeOutMusic");
		audio.volume = 1f;
		audio.Play();
	}

	public void StopMusic(float seconds)
	{
		StartCoroutine(FadeOutMusic(seconds));
	}

	IEnumerator FadeOutMusic(float seconds)
	{
		while (audio.volume > 0)
		{
			float decrement = Time.deltaTime / seconds;
			Debug.Log(decrement + " " + audio.volume);
			audio.volume -= decrement;
			yield return null;
		}
		audio.Stop();
	}

	void OnMusicEnd()
	{

	}
}
