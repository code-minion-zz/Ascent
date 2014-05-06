using UnityEngine;
using System.Collections;

public class StunnedEffect : MonoBehaviour 
{
	public GameObject[] sparkles = new GameObject[3];
	public GameObject[] rats = new GameObject[3];

	private float fadeOutTime = 0.15f;
	private bool fadeOut;

	private float timeElapsed;

	private Transform target;

	public void Initialise(Transform target)
	{
		this.target = target;
	}

	void Update () 
	{
		if (target != null)
		{
			Vector3 targetPos = target.transform.position;
			targetPos.y = transform.position.y;
			transform.position = targetPos;
		}

		if (target == null && !fadeOut)
		{
			fadeOut = true;
		}

		if (fadeOut && timeElapsed < fadeOutTime)
		{
			timeElapsed += Time.deltaTime;
			if (timeElapsed > fadeOutTime)
			{
				timeElapsed = fadeOutTime;
			}

			foreach (GameObject sparkle in sparkles)
			{
				GameObject light = sparkle.transform.FindChild("light").gameObject;
				Color color = light.renderer.material.GetColor("_TintColor");
				color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
				light.renderer.material.SetColor("_TintColor", color);

				GameObject star = sparkle.transform.FindChild("stars").gameObject;
				color = star.renderer.material.GetColor("_TintColor");
				color.a = Mathf.Lerp(1.0f, 0.0f, timeElapsed);
				star.renderer.material.SetColor("_TintColor", color);
			}

			if (timeElapsed == fadeOutTime)
			{
				GameObject.Destroy(this.gameObject);
			}
		}
	}

	[ContextMenu("FadeOutAndDie")]
	public void FadeOutAndDie()
	{
		if (!fadeOut)
		{
			fadeOut = true;

			foreach (GameObject rat in rats)
			{
				rat.GetComponentInChildren<RimShaderIrisator>().enabled = false;
			}
		}
	}
}
