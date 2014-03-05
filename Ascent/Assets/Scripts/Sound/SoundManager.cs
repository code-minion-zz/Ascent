using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }
}
