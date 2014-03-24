using UnityEngine;
using System.Collections;

public class AscentSoundManager : MonoBehaviour {

	void Start()
	{		
		audio.clip = Resources.Load("Sound");
	}

	public void SetSound()
	{
	}
}
