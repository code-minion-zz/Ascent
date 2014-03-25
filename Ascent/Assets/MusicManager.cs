using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{	
	private static AudioClip towerMusic = Resources.Load("Sounds/music/tower") as AudioClip;
	private static AudioClip bossMusic = Resources.Load("Sounds/music/boss") as AudioClip;
	
	public enum MusicSelections
	{
		Tower,
		Boss
	}

	void Start()
	{		
		audio.clip = towerMusic;
	}

	public void SwapMusic(MusicSelections choice)
	{
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
			audio.volume -= Time.deltaTime * 0.5f;
			yield return null;
		}
	}
}
