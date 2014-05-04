using UnityEngine;
using System.Collections;

public class FallingDebrisShadow : MonoBehaviour
{
	float largestSize = 3.0f;

	public bool stop;

	void Start()
	{
	}

	void Update()
	{
		if (stop)
			return;

		Vector3 hit = transform.position;
		hit.y = 0.1f;
		transform.position = hit;

		transform.localScale = Vector3.Lerp(Vector3.one * largestSize, Vector3.one, transform.parent.transform.position.y / 15.0f);

		Color color = renderer.material.color;

		color.a = Mathf.Lerp(1.0f, 0.50f, transform.parent.transform.position.y / 15.0f);

		renderer.material.color = color;
	}
}
