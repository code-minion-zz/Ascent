using UnityEngine;
using System.Collections;

public class StunnedEffect : MonoBehaviour 
{
	public GameObject[] sparkles = new GameObject[3];
	public GameObject[] rats = new GameObject[3];

	private float fadeOutTime = 0.15f;
	private bool fadeOut;

	private float timeElapsed;

	private Transform targetPosition;
	private Character targetCharacter;

	public void Initialise(Transform target, Character targetCharacter)
	{
		this.targetCharacter = targetCharacter;
		this.targetPosition = target;
	}

	void Update () 
	{
		if (targetPosition != null)
		{
			Vector3 targetPos = targetPosition.transform.position;
			targetPos.y = transform.position.y;
			transform.position = targetPos;
		}

		if ((targetPosition == null || targetCharacter == null) && !fadeOut)
		{
			fadeOut = true;
		}
		else if (targetCharacter != null && !targetCharacter.IsInState(EStatus.Stun))
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
				return;
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
