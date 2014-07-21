using UnityEngine;
using System.Collections;

public class EnemyMotor : CharacterMotor 
{
    public float rotationSpeed = 5.0f;

	private bool snapToTarget = false;
	public bool SnapToTarget
	{
		get { return snapToTarget; }
		set { snapToTarget = value; }
	}

    protected override void ProcessMovement()
    {
		Vector3 desiredVelocity = Vector3.zero;

        // Updates buff values
		if (!isActionHaltingMovement)
		{
			desiredVelocity = ProcessStandardMovement();
		}

        // Move forward with the velocity magnitude
		if (desiredVelocity.magnitude > 0.01f)
		{
			// Rotate toward the target
			if (snapToTarget)
			{
				transform.rotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
			}
			else
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(desiredVelocity, Vector3.up), rotationSpeed);	
			}
			// Animate based on current speed
			GetComponent<CharacterAnimator>().PlayAnimation("Movement", (GetComponent<AISteeringAgent>().Velocity.magnitude / GetComponent<AISteeringAgent>().maxSpeed));


			rigidbody.AddForce(transform.forward * desiredVelocity.magnitude, ForceMode.VelocityChange);

			targetVelocity = (targetVelocity * currentSpeed);

			// Apply a force that attempts to reach target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);

			float buffedMaxVelocityChange = maxVelocityChange + (buffBonusSpeed * 0.5f);

			velocityChange.x = Mathf.Clamp(velocityChange.x, -buffedMaxVelocityChange, buffedMaxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -buffedMaxVelocityChange, buffedMaxVelocityChange);
			velocityChange.y = 0;

			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
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
