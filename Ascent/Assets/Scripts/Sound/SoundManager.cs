using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AudioClipType
{
    explosion,
    swordSlash,
    statueAwaken,
	woodHit,
	freezeBlast,
	shootFire,
	pop,
	wethit,
	dooropen,
	stonedrag,
	switchclick,
	lightning,
	earthshock,
	arrowwoosh,
	heavyhit,

}

public static class SoundManager
{
    private static AudioClip explosionClip	;
	private static AudioClip swordSlash		;
	private static AudioClip statueAwaken	;
	private static AudioClip woodHit		;
	private static AudioClip freezeBlast	;
	private static AudioClip shootFire		;
	private static AudioClip pop			;
	private static AudioClip wethit1		;
	private static AudioClip wethit2		;
	private static AudioClip wethit3		;
	private static AudioClip wethit4		;
	private static AudioClip dooropen		;
	private static AudioClip stonedrag		;
	private static AudioClip stonedrag2		;
	private static AudioClip switchclick	;
	private static AudioClip lightning		;
	private static AudioClip earthshock		;
	private static AudioClip arrowwoosh		;
	private static AudioClip heavyhit		;

	public static float	VolumeScale = 0.08f;

	static AudioSource source;

	static List<AudioSource> AudioSourcePool;

	static int NumSources = 10;

	public static void Initialise()
	{

		explosionClip = Resources.Load("Sounds/effects/explode") as AudioClip;
		swordSlash = Resources.Load("Sounds/effects/warriorStrike_snd01") as AudioClip;
		statueAwaken = Resources.Load("Sounds/effects/statueAwaken") as AudioClip;
		woodHit = Resources.Load("Sounds/effects/woodenhit") as AudioClip;
		freezeBlast = Resources.Load("Sounds/effects/freezeblast") as AudioClip;
		shootFire = Resources.Load("Sounds/effects/shootFire") as AudioClip;
		pop = Resources.Load("Sounds/effects/pop") as AudioClip;
		wethit1 = Resources.Load("Sounds/effects/wethit1") as AudioClip;
		wethit2 = Resources.Load("Sounds/effects/wethit2") as AudioClip;
		wethit3 = Resources.Load("Sounds/effects/wethit3") as AudioClip;
		wethit4 = Resources.Load("Sounds/effects/wethit4") as AudioClip;
		dooropen = Resources.Load("Sounds/effects/dooropen") as AudioClip;
		stonedrag = Resources.Load("Sounds/effects/stonedrag") as AudioClip;
		stonedrag2 = Resources.Load("Sounds/effects/stonedrag2") as AudioClip;
		switchclick = Resources.Load("Sounds/effects/switchclick") as AudioClip;
		lightning = Resources.Load("Sounds/effects/lightning") as AudioClip;
		earthshock = Resources.Load("Sounds/effects/earthshock") as AudioClip;
		arrowwoosh = Resources.Load("Sounds/effects/arrowwoosh") as AudioClip;
		heavyhit = Resources.Load("Sounds/effects/heavyhit") as AudioClip;
		AudioSourcePool = new List<AudioSource>();
		source = GameObject.Find("SoundManager").GetComponent<AudioSource>();
		string path = "Prefabs/Audio Source";
		
		int i = 0;
		for (; i < NumSources; ++i)
		{
			Object obj = Resources.Load(path);
			GameObject go = GameObject.Instantiate(obj) as GameObject;
			AudioSourcePool.Add(go.GetComponent<AudioSource>());
			AudioSourcePool[i].transform.parent = source.transform;
		}
	}

    public static void PlaySound(AudioClipType clipType, Vector3 position, float volume)
    {
		AudioSource mySource = null;
		if (mySource == null)
		{
			mySource = GetSource();
		}

        AudioClip clip = GetClipFromType(clipType);

        if (clip != null)
        {
            //position += new Vector3(0.0f, 10.0f);
			mySource.clip = clip;
			mySource.volume = volume * VolumeScale;
			mySource.Play();

#if UNITY_EDITOR
			//Selection.activeGameObject = mySource.gameObject;
#endif
        }
        else
        {
            Debug.LogWarning("Audio clip was not found or created");
        }
    }

	static int NumIdleSources()
	{
		int count = 0; 

		AudioSourcePool.ForEach(delegate(AudioSource source) {
			if (source.isPlaying == false) 
				++count;
		});

		return count;
	}

	static AudioSource GetSource()
	{
		AudioSource retval = SoundManager.GetIdleSource();

		if (retval == null)
		{
			retval = GetOldestSource();
		}
		
		return retval;
	}

	static AudioSource GetIdleSource()
	{
		return AudioSourcePool.Find(source => source.isPlaying == false);
	}

	static AudioSource GetOldestSource()
	{
		AudioSource oldest = null;

		foreach (AudioSource source in AudioSourcePool)
		{
			if (oldest == null)
			{
				oldest = source;
				continue;
			}
			if (oldest.time < source.time)
			{
				oldest = source;
			}
		}
		return oldest;
	}

    public static AudioClip GetClipFromType(AudioClipType type)
    {
        AudioClip clip = null;

        switch (type)
        {
            case AudioClipType.explosion:
                clip = explosionClip;
                break;

            case AudioClipType.swordSlash:
                clip = swordSlash;
			break;
			
			case AudioClipType.statueAwaken:
				clip = statueAwaken;
				break;
				
			case AudioClipType.woodHit:
				clip = woodHit;
			break;
			
			case AudioClipType.freezeBlast:
				clip = freezeBlast;
			break;
			
			case AudioClipType.shootFire:
				clip = shootFire;
				break;
				
			case AudioClipType.pop:
				clip = pop;
			break;

			case AudioClipType.wethit:
			{
				int result = Random.Range(1,4);
				switch (result)
				{
				case 1:
					clip = wethit1;
					break;
				case 2:
					clip = wethit2;
					break;
				case 3:
					clip = wethit3;
					break;
				case 4:
					clip = wethit4;
					break;
				}
			break;
			}

			case AudioClipType.dooropen:
				clip = dooropen;
				break;

			case AudioClipType.stonedrag:
			{
				int result = Random.Range(1,2);
				switch (result)
				{
				case 1:
					clip = stonedrag;
				break;
				case 2:
					clip = stonedrag2;
				break;
				}
			break;
			}

			case AudioClipType.switchclick:
				clip = switchclick;
			break;

			case AudioClipType.lightning:
				clip = lightning;
			break;

			case AudioClipType.earthshock:
				clip = earthshock;
			break;
			case AudioClipType.arrowwoosh:
				clip = arrowwoosh;
			break;
			case AudioClipType.heavyhit:
				clip = heavyhit;
			break;

		}
		
        return clip;
    }
}
