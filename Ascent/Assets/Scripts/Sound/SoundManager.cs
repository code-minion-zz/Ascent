using UnityEngine;
using System.Collections.Generic;

public enum AudioClipType
{
    explosion,
    swordSlash,
    statueAwaken
}

public static class SoundManager
{
    private static AudioClip explosionClip = Resources.Load("Sounds/explosion") as AudioClip;
    private static AudioClip swordSlash = Resources.Load("Sounds/warriorStrike_snd01") as AudioClip;
    private static AudioClip statueAwaken = Resources.Load("Sounds/statueAwaken") as AudioClip;


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
        }

        return clip;
    }
}
