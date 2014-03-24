using UnityEngine;
using System.Collections;

public class AscentSoundManager : MonoBehaviour {

	void Start()
	{		
		audio.clip = Resources.Load("Sounds/music/tower") as AudioClip;
	}

	public void SetSound(string audioClip)
	{
		audio.clip = Resources.Load(audioClip) as AudioClip;
	}

	public void PlaySound()
	{
		audio.Play();
	}
}
