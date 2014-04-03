using UnityEngine;
using System.Collections.Generic;

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
    private static AudioClip explosionClip = Resources.Load("Sounds/effects/explode") as AudioClip;
	private static AudioClip swordSlash = Resources.Load("Sounds/effects/warriorStrike_snd01") as AudioClip;
	private static AudioClip statueAwaken = Resources.Load("Sounds/effects/statueAwaken") as AudioClip;
	private static AudioClip woodHit = Resources.Load("Sounds/effects/woodenhit") as AudioClip;
	private static AudioClip freezeBlast = Resources.Load("Sounds/effects/freezeblast") as AudioClip;
	private static AudioClip shootFire = Resources.Load("Sounds/effects/shootFire") as AudioClip;
	private static AudioClip pop = Resources.Load("Sounds/effects/pop") as AudioClip;
	private static AudioClip wethit1 = Resources.Load("Sounds/effects/wethit1") as AudioClip;
	private static AudioClip wethit2 = Resources.Load("Sounds/effects/wethit2") as AudioClip;
	private static AudioClip wethit3 = Resources.Load("Sounds/effects/wethit3") as AudioClip;
	private static AudioClip wethit4 = Resources.Load("Sounds/effects/wethit4") as AudioClip;
	private static AudioClip dooropen = Resources.Load("Sounds/effects/dooropen") as AudioClip;
	private static AudioClip stonedrag = Resources.Load("Sounds/effects/stonedrag") as AudioClip;
	private static AudioClip stonedrag2 = Resources.Load("Sounds/effects/stonedrag2") as AudioClip;
	private static AudioClip switchclick = Resources.Load("Sounds/effects/switchclick") as AudioClip;
	private static AudioClip lightning = Resources.Load("Sounds/effects/lightning") as AudioClip;
	private static AudioClip earthshock = Resources.Load("Sounds/effects/earthshock") as AudioClip;
	private static AudioClip arrowwoosh = Resources.Load("Sounds/effects/arrowwoosh") as AudioClip;
	private static AudioClip heavyhit = Resources.Load("Sounds/effects/heavyhit") as AudioClip;

    public static void PlaySound(AudioClipType clipType, Vector3 position, float volume)
    {
        AudioClip clip = GetClipFromType(clipType);

        if (clip != null)
        {
            position += new Vector3(0.0f, 10.0f);
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        else
        {
            Debug.LogWarning("Audio clip was not found or created");
        }
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
				int result = Random.Range(1,4);
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
