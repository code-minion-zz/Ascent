using UnityEngine;
using System.Collections;

public class FadePlane : MonoBehaviour 
{
	private float fadeTime = 0.0f;
	private float startTime;

	public void StartFade(float fadeDuration, Vector3 position)
	{
		gameObject.SetActive(true);
		fadeTime = fadeDuration;
		startTime = fadeTime;

		Color col = gameObject.renderer.material.color;
		col.a = 0.0f;
		gameObject.renderer.material.color = col;

		transform.position = new Vector3(position.x, transform.position.y, position.z);
	}
	
	void Update () 
	{
		if (fadeTime > 0.0f)
		{
			fadeTime -= Time.deltaTime;
			if(fadeTime < 0.0f)
			{
                //gameObject.SetActive(false);
				fadeTime = 0.0f;
			}

			Color col = gameObject.renderer.material.color;
			col.a = Mathf.Lerp(1.0f, 0.0f, fadeTime / startTime);

			gameObject.renderer.material.color = col;
		}
	}
}
