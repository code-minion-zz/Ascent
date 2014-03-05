using UnityEngine;
using System.Collections;

public class RayDown : MonoBehaviour 
{
    float originalSize;
    float largestSize = 4.0f;

    void Start()
    {
        originalSize = 1.0f;
    }

    void Update () 
    {
        Vector3 hit = transform.position;
        hit.y = 0.1f;
        transform.position = hit;

        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * largestSize, transform.parent.transform.position.y / 15.0f);

        Color color = renderer.material.color;

        color.a = Mathf.Lerp(1.0f, 0.50f, transform.parent.transform.position.y / 15.0f);

        renderer.material.color = color;
	}
}
