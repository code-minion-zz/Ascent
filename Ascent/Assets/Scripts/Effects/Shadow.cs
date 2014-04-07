using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour 
{
	public float size = 1.0f;
	public float offsetZ = -0.5f;

	private Transform shadowTransform;

	public GameObject shadow;

	private Vector3 startScale;
	private float startAlpha;

	private bool fading;

	public void Initialise()
	{
        if (shadow == null)
        {
            shadow = GameObject.Instantiate(Resources.Load("Prefabs/Effects/CharacterShadow")) as GameObject;
			shadow.transform.parent = this.transform;

			shadowTransform = shadow.transform;
			shadowTransform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
			shadowTransform.localScale = new Vector3(size, size, size);

			startScale = shadowTransform.localScale;
			startAlpha = shadow.renderer.material.color.a;
        }
	}

	public void FadeOut(float t)
	{
		fading = true;

		Color color = shadow.renderer.material.color;
		color.a = Mathf.Lerp(startAlpha, 0.0f, t);
		shadow.renderer.material.color = color;
	}

//#if UNITY_EDITOR
	public void LateUpdate()
	{
		if (shadow != null && !fading)
		{
			shadowTransform.localScale = new Vector3(size, size, size);
			shadowTransform.position = new Vector3(shadowTransform.parent.position.x, 0.1f, shadowTransform.parent.position.z + offsetZ);
			shadowTransform.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);
		}
	}
//#endif
}
