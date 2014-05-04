using UnityEngine;
using System.Collections;

public class RotateSelfRandomly : MonoBehaviour 
{
	public Vector2 randRotationSpeedX;
	public Vector2 randRotationSpeedY;
	public Vector2 randRotationSpeedZ;

	private float rotationSpeedX;
	private float rotationSpeedY;
	private float rotationSpeedZ;

	public bool local = true;

	void Start()
	{
		rotationSpeedX = Random.Range(randRotationSpeedX.x, randRotationSpeedX.y);
		rotationSpeedY = Random.Range(randRotationSpeedY.x, randRotationSpeedY.y);
		rotationSpeedZ = Random.Range(randRotationSpeedZ.x, randRotationSpeedZ.y);
	}

	void Update()
	{
		if (local)
		{
			transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
			return;
		}

		transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime, Space.World);
	}
}
