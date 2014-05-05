using UnityEngine;
using System.Collections;

public class GroundSkullEye : MonoBehaviour 
{
	public Light pointLight;
	public ParticleSystem fire;
	public ParticleSystem halo;

	private float timeElapsed;

	private bool fadeOut;

	private float lightRangeStart;
	private float lightIntensityStart;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (fadeOut && timeElapsed < 1.0f)
		{
			timeElapsed += Time.deltaTime;
			if (timeElapsed > 1.0f)
			{
				timeElapsed = 1.0f;
			}

			// Light
			pointLight.range = Mathf.Lerp(lightRangeStart, 0.0f, timeElapsed);
			pointLight.intensity = Mathf.Lerp(lightIntensityStart, 0.0f, timeElapsed);

			Color color = fire.renderer.material.GetColor("_TintColor");
			color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
			fire.renderer.material.SetColor("_TintColor", color);

			color = halo.renderer.material.GetColor("_TintColor");
			color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
			halo.renderer.material.SetColor("_TintColor", color);

			if (timeElapsed == 1.0f)
			{
				GameObject.Destroy(this.gameObject);
			}
		}
	}

	[ContextMenu("FadeOutAndDie")]
	public void FadeOutAndDie()
	{
		if(!fadeOut)
		{
			fadeOut = true;
			
			lightRangeStart = pointLight.range;
			lightIntensityStart = pointLight.intensity;
		}
	}
}
