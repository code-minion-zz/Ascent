using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour 
{
	public float size = 1.0f;
	public float offsetZ = -0.5f;

	private Transform shadowTransform;

	GameObject shadow;

	public void Initialise()
	{
        if (shadow == null)
        {
            shadow = GameObject.Instantiate(Resources.Load("Prefabs/Effects/Shadow")) as GameObject;
			shadow.transform.parent = this.transform;

			shadowTransform = shadow.transform;
			shadowTransform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
			shadowTransform.localScale = new Vector3(size, size, size);
        }
	}

#if UNITY_EDITOR
	public void LateUpdate()
	{
		if (shadow != null)
		{
			shadowTransform.localScale = new Vector3(size, size, size);
			shadowTransform.position = new Vector3(shadowTransform.parent.position.x, 0.1f, shadowTransform.parent.position.z + offsetZ);
			shadowTransform.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);
		}
	}
#endif
}
