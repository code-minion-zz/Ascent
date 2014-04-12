using UnityEngine;
using System.Collections;

public class EnemyMotor : CharacterMotor 
{
    public float rotationSpeed = 5.0f;

    protected override void ProcessMovement()
    {
        // Updates buff values
        Vector3 velocity = ProcessStandardMovement();

        // Animate based on current speed
        GetComponent<CharacterAnimator>().PlayAnimation("Movement", currentSpeed / MaxSpeed);

        // Rotate toward the target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed);


        // Move forward with the velocity magnitude
		if(!Mathf.Approximately(velocity.magnitude, 0.0f))
		{
			rigidbody.AddForce(transform.forward * velocity.magnitude, ForceMode.VelocityChange);
		}
    }
}
