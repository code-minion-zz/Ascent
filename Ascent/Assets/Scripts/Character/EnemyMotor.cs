using UnityEngine;
using System.Collections;

public class EnemyMotor : CharacterMotor 
{
    public float rotationSpeed = 5.0f;

    protected override void ProcessMovement()
    {
		Vector3 velocity = Vector3.zero;

        // Updates buff values
		if (!isActionHaltingMovement)
		{
			velocity = ProcessStandardMovement();
		}

        // Move forward with the velocity magnitude
		if (velocity.magnitude > 0.01f)
		{
			// Rotate toward the target
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed);
			// Animate based on current speed
			GetComponent<CharacterAnimator>().PlayAnimation("Movement", (GetComponent<AISteeringAgent>().Velocity.magnitude / GetComponent<AISteeringAgent>().maxSpeed));


			rigidbody.AddForce(transform.forward * velocity.magnitude, ForceMode.VelocityChange);
		}
		else
		{
			// If in the close enough range, movement has stopped but we still want to rotate to the target
			if (GetComponent<AISteeringAgent>().CloseEnough)
			{
				Character target = GetComponent<Enemy>().TargetCharacter;

				if (target != null && !isActionHaltingMovement)
				{
					float relativeAngle = Vector3.Dot(GetComponent<AISteeringAgent>().Velocity, target.transform.position - transform.position);

					if (Mathf.Abs(relativeAngle) > 0.55f || Mathf.Abs(relativeAngle) < 0.45f)
					{
						GetComponent<CharacterAnimator>().PlayAnimation("Movement", 0.15f);

						transform.LookAt(transform.position + GetComponent<AISteeringAgent>().Velocity);
					}
					else
					{
						GetComponent<CharacterAnimator>().PlayAnimation("Movement", 0.0f);
					}
				}
				else
				{
					GetComponent<CharacterAnimator>().PlayAnimation("Movement", 0.0f);
				}
			}
			else
			{
				GetComponent<CharacterAnimator>().PlayAnimation("Movement", 0.0f);
			}
		}
    }
}
