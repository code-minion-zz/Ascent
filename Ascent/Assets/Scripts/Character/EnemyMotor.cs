using UnityEngine;
using System.Collections;

public class EnemyMotor : CharacterMotor 
{
    public float rotationSpeed = 5.0f;

    protected override void ProcessMovement()
    {
        // Updates buff values
        Vector3 velocity = ProcessStandardMovement();

        // Move forward with the velocity magnitude
		if (velocity.magnitude > 0.01f)
		{
			// Rotate toward the target
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed);

			// Animate based on current speed
			GetComponent<CharacterAnimator>().PlayAnimation("Movement", currentSpeed / MaxSpeed);


			rigidbody.AddForce(transform.forward * velocity.magnitude, ForceMode.VelocityChange);
		}
		else
		{
			GetComponent<CharacterAnimator>().PlayAnimation("Movement", 0.0f);
		}
    }
}
