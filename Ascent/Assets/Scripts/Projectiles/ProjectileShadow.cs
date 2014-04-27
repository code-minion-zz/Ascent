using UnityEngine;
using System.Collections;

public class ProjectileShadow : MonoBehaviour 
{
    public float size = 0.5f;

    void Update () 
    {
        Vector3 hit = transform.position;
        hit.y = 0.1f;
        transform.position = hit;
	}
}
