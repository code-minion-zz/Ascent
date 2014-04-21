//This is free to use and no attribution is required
//No warranty is implied or given

using UnityEngine;
using System.Collections;



[RequireComponent(typeof(LineRenderer))]
public class LazerBeam : MonoBehaviour
{
	public float laserWidth = 1.0f;
	public float noise = 1.0f;
	public float maxLength = 50.0f;
	public Color color = Color.red;

	LineRenderer lineRenderer;

	int length;

	Vector3[] position;

	//Cache any transforms here
	Transform myTransform;
	Transform endEffectTransform;
	Transform startEffectTransform;

	public ParticleSystem startEffect;

	//The particle system, in this case sparks which will be created by the Laser
	public ParticleSystem endEffect;

	Vector3 offset;

	private GameObject lastHitObject;
	public GameObject LastHit
	{
		get { return lastHitObject; }
	}

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();

		lineRenderer.SetWidth(laserWidth, laserWidth);

		myTransform = transform;

		offset = new Vector3(0, 0, 0);

		//endEffect = GetComponentInChildren<ParticleSystem>();
		if (endEffect != null)
		{
			endEffect = Instantiate(endEffect) as ParticleSystem;
			endEffectTransform = endEffect.transform;
			endEffectTransform.parent = transform;
		}

		if (startEffect != null)
		{
			startEffect = Instantiate(startEffect) as ParticleSystem;
			startEffectTransform = startEffect.transform;
			startEffectTransform.parent = transform;
		}
	}


	void Update()
	{
		RenderLaser();
	}


	void RenderLaser()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetWidth(laserWidth, laserWidth);

		//Shoot our LazerBeam forwards!
		UpdateLength();

		lineRenderer.SetColors(color, color);

		//Move through the Array
		for (int i = 0; i < length; i++)
		{
			//Set the position here to the current location and project it in the forward direction of the object it is attached to
			offset.x = myTransform.position.x + i * myTransform.forward.x + Random.Range(-noise, noise);
			offset.y = i * myTransform.forward.y + Random.Range(-noise, noise) + myTransform.position.y;
			offset.z = i * myTransform.forward.z + Random.Range(-noise, noise) + myTransform.position.z;

			position[i] = offset;

			position[0] = myTransform.position;

			lineRenderer.SetPosition(i, position[i]);
		}

		if (startEffect != null)
		{
			startEffectTransform.position = myTransform.position;

			if (!startEffect.isPlaying)
			{
				endEffect.Play();
			}
		}
	}


	void UpdateLength()
	{
		//Raycast from the location of the cube forwards
		RaycastHit hitInfo;
		if(Physics.Raycast(myTransform.position, myTransform.forward, out hitInfo, maxLength))
		{
			//Check to make sure we aren't hitting triggers but colliders
			if (!hitInfo.collider.isTrigger)
			{
				length = (int)Mathf.Round(hitInfo.distance) + 1;

				position = new Vector3[length];

				//Move our End Effect particle system to the hit point and start playing it
				if (endEffect != null)
				{
					endEffectTransform.position = hitInfo.point;

					if (!endEffect.isPlaying)
					{
						endEffect.Play();
					}
				}

				lineRenderer.SetVertexCount(length);

				lastHitObject = hitInfo.collider.gameObject;

				return;
			}
		}

		//If we're not hitting anything, don't play the particle effects

		if (endEffect != null)
		{
			if (endEffect.isPlaying)
			{
				endEffect.Stop();
			}
		}

		length = (int)maxLength;

		position = new Vector3[length];

		lineRenderer.SetVertexCount(length);

		lastHitObject = null;
	}

}