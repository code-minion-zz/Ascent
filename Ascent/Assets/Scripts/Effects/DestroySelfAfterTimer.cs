using UnityEngine;
using System.Collections;

public class DestroySelfAfterTimer : MonoBehaviour 
{
    public float timer = 5.0f;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
