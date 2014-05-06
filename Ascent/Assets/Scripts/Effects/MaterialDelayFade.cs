using UnityEngine;
using System.Collections;

public class MaterialDelayFade : MonoBehaviour 
{
	public float delay = 0.0f;
	public float fadeInTime = 0.1f;
	public float stayTime = 1.0f;
	public float fadeOutTime = 0.7f;
	public Color myColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
	
	private float maxAlpha = 0.0f;
	private float delayTime = 0.0f;
	private float timeElapsed = 0.0f;
	private float timeElapsedLast = 0.0f;
	private float percent = 0.0f;

	void Start()
	{
		maxAlpha = myColor.a;
		renderer.material.SetColor("_TintColor", new Color(myColor.r, myColor.g, myColor.b, 0.0f));
		if (fadeInTime < 0.01f)
		{
			fadeInTime = 0.01f; // avoid div by 0
		}

		percent = (timeElapsed / fadeInTime) * maxAlpha;
	}

	void Update()
	{
		renderer.material.SetColor("_TintColor", new Color(myColor.r, myColor.g, myColor.b, 0.0f));

		delayTime += Time.deltaTime;
		if(delayTime > delay)
		{
			timeElapsed += Time.deltaTime;

			if (timeElapsed <= fadeInTime) //fade in
			{
				percent = (timeElapsed / fadeInTime) * maxAlpha;
				renderer.material.SetColor("_TintColor", new Color(myColor.r, myColor.g, myColor.b, percent));
			}

			if ((timeElapsed > fadeInTime) && (timeElapsed < fadeInTime + stayTime)) //set the normal color
			{
				renderer.material.SetColor("_TintColor", new Color(myColor.r, myColor.g, myColor.b, maxAlpha));
			}

			if (timeElapsed >= fadeInTime + stayTime && timeElapsed <= fadeInTime + stayTime + fadeOutTime) //fade out
			{
				timeElapsedLast += Time.deltaTime;
				percent = maxAlpha - ((timeElapsedLast / fadeOutTime) * maxAlpha);

				if (percent < 0)
				{
					percent = 0;
				}

				renderer.material.SetColor("_TintColor", new Color(myColor.r, myColor.g, myColor.b, percent));
			}

		}
	}
	
}
