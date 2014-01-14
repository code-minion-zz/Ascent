using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour 
{
	public float size = 1.0f;
	public float offsetZ = -0.5f;

	private float initialScale;

	private Transform shadowTransform;
	private Transform parentTransform;

	GameObject shadow;

	public void Start()
	{
		if (shadow == null)
		{
			Initialise();
		}
	}

	public void Initialise()
	{
		initialScale = size;
		initialScale++;

		shadow = GameObject.Instantiate(Resources.Load("Prefabs/Effects/Shadow")) as GameObject;

		shadow.transform.parent = this.transform;

		shadowTransform = shadow.transform;
		parentTransform = transform;

		shadowTransform.localScale = new Vector3(size, size, size);
	}

	public void Update()
	{
		Vector3 newPos = parentTransform.position;
		shadowTransform.localScale = new Vector3(size, size, size);
		shadowTransform.position = new Vector3(newPos.x, 0.1f, newPos.z + offsetZ);
		shadowTransform.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);
	}

	public void Process()
	{
		//Vector3 newPos = parentTransform.position;
		//shadowTransform.position = new Vector3(newPos.x, 0.01f, newPos.z + offsetZ);
		//shadowTransform.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);

		// TODO: Scale shadow so that is large when it is higher or vice versa... what makes more sense
		//Mathf.InverseLerp(parentTransform.position.y, 0.1f, 0.5f);
		//shadowTransform.localScale = new Vector3(size, size, size);
		
	}
}
