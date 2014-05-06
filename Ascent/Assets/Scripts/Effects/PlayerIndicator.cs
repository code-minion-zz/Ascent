using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour
{
	private float size = 1.5f;
	private float offsetZ = -0.15f;

	private Transform indicatorTransform;

    private float offsetY = 0.225f;

	GameObject indicator;

	public void Initialise(Color color)
	{
		if (indicator == null)
		{
			indicator = GameObject.Instantiate(Resources.Load("Prefabs/PlayerIndicator")) as GameObject;
			indicator.transform.parent = this.transform;

			indicatorTransform = indicator.transform;
            indicatorTransform.localPosition = new Vector3(0.0f, offsetY, 0.0f);
			indicatorTransform.localScale = new Vector3(size, size, size);

			indicator.renderer.material.color = color;
		}
	}

#if UNITY_EDITOR
	public void LateUpdate()
	{
		if (indicator != null)
		{
			indicatorTransform.localScale = new Vector3(size, size, size);
            indicatorTransform.position = new Vector3(indicatorTransform.parent.position.x, offsetY, indicatorTransform.parent.position.z + offsetZ);
			indicatorTransform.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);
		}
	}
#endif
}
