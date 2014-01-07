using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour 
{
	public GameObject actor;

	private CharacterController controller;

	public bool canMove = true;

	private Vector3 velocity;
	private Vector3 movementForce;
	private Vector3 knockbackDirection;
	private float knockbackMag;
	private float knockbackDecel = 1.5f;
	private float speed = 6.0f;

	public virtual void Initialise()
	{
		controller = actor.GetComponentSafe<CharacterController>();
	}

	public virtual void LateUpdate()
	{
		int forces = 0;

		if (knockbackMag > 0.0f)
		{
			float knockbackWeight = 1000000.0f;
			velocity += (knockbackDirection * knockbackMag) * knockbackWeight;
			++forces;

			knockbackMag -= knockbackMag / 1.1f;

			if (knockbackMag < 0.01f)
			{
				knockbackMag = 0.0f;
			}
		}
		else if (movementForce != Vector3.zero)
		{
			velocity += movementForce;
			++forces;
		}


		if(forces > 0)
		{
			velocity /= (float)forces;
		}

		controller.Move((velocity.normalized * speed) * Time.deltaTime);

		movementForce = Vector3.zero;
		velocity = Vector3.zero;

		CharacterTilt tilt = GetComponentInChildren<CharacterTilt>();
		tilt.Process();

		Shadow shadow = GetComponentInChildren<Shadow>();
		shadow.Process();
	}

	public virtual void Move(Vector3 motion)
	{
		movementForce = motion;
	}

	public virtual void SetKnockback(Vector3 direction, float mag)
	{
		knockbackDirection  = direction;
		knockbackMag = mag * 1000.0f;
	}
}
