using UnityEngine;
using System.Collections;

public class RayDown : MonoBehaviour 
{
    void Update () 
    {
        Vector3 hit = transform.position;
        hit.y = 0.1f;
        transform.position = hit;
	}
}
