using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{	
	public static MusicManager Instance;
	private static AudioClip towerMusic = Resources.Load("Sounds/music/tower") as AudioClip;
	private static AudioClip bossMusic = Resources.Load("Sounds/music/boss") as AudioClip;

	private MusicSelections nextMusic;

	public float FadeDuration = 1f;
	float elapsedTime;

	public enum State
	{
		Stop,
		In,
		Play,
		Out
	}

	public enum MusicSelections
	{
		None,
		Tower,
		Boss
	}
	
	State musicState = State.Stop;

	void Start()
	{
		if (Instance == null) Instance = this;
		audio.clip = towerMusic;
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;
		switch (musicState)
		{
		case State.In:
			Debug.Log("FADING IN" + elapsedTime);
			FadeInMusic();
			break;
		case State.Out:
			Debug.Log("FADING OUT" + elapsedTime);
			FadeOutMusic();
			break;
		}
	}

	public void PlayMusic(MusicSelections choice, bool immediate = false)
	{
		if (immediate)
		{
			SwapMusic(choice);
			audio.Stop();
			audio.volume = 1f;
			musicState = State.Play;
		}
		else
		{
			switch(musicState)
			{
			case State.Play:
				elapsedTime = 0f;
				musicState = State.Out;
				break;
			case State.In:
				elapsedTime = 0f;
				musicState = State.Out;
				break;
			case State.Stop:
				elapsedTime = 0f;
				audio.volume = 0f;
				SwapMusic(choice);
				musicState = State.In;
				break;
			}
			audio.Play();
			nextMusic = choice;
		}
	}

	public void SetVolume(float val)
	{
		audio.volume = val;
	}

	public void StopMusic()
	{
		musicState = State.Stop;
		audio.Stop();
		if (nextMusic != MusicSelections.None) PlayMusic(nextMusic);
	}

	void FadeOutMusic()
	{
		audio.volume = Mathf.Lerp(1f, 0f, elapsedTime/FadeDuration);
		if (audio.volume <= 0f)
		{
			musicState = State.Stop;
			StopMusic();
		}
	}
	
	void FadeInMusic()
	{

		audio.volume = Mathf.Lerp(0f, 1f, elapsedTime/FadeDuration);
		if (audio.volume >= 1f)
		{
			musicState = State.Play;
		}
	}

	void OnMusicEnd()
	{
		SwapMusic(nextMusic);
		audio.Play();
	}
	
	void SwapMusic(MusicSelections choice)
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
}
