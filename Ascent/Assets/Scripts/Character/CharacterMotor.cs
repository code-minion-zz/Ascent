using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class CharacterMotor : MonoBehaviour
{
	public GameObject actor;

	private Vector3 movementForce;
	public float speed = 6.0f;
	public float maxVelocityChange = 5.0f;
    public bool canMove = true;

    private Vector3 specialMovementForce;

	private Vector3 knockbackDirection;
	private float knockbackMag;
	private float knockbackDecel = 0.65f;

	private bool usingMovementForce = true;
	public bool UsingMovementForce
	{
		get { return usingMovementForce; }
	}

    private Vector3 targetVelocity;
    public Vector3 TargetVelocity
    {
        get { return targetVelocity; }
    }

	// Grid Movement
	public bool moving;
	float offset = 1.0f;
	float moveTime = 0.5f;
	float timeAccum;

	Vector3 startPos;
	Vector3 targetPos;
	
	public virtual void Initialise()
	{
		rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}

	public virtual void FixedUpdate()
	{
	
		if (moving)
		{
			timeAccum += Time.deltaTime;
			if(timeAccum > moveTime)
			{
				timeAccum = 1.0f;
				moving = false;
			}

			 transform.position = Vector3.Lerp(startPos, targetPos, timeAccum / moveTime);
			 return;
		}

		// Calculate how fast we should be moving
		targetVelocity = Vector3.zero;
		Vector3 knockbackVel = Vector3.zero;
        int forces = 0;

        if (specialMovementForce != Vector3.zero)
        {
            targetVelocity += new Vector3(specialMovementForce.x, 0.0f, specialMovementForce.z);
        }
        else
        {
            if (knockbackMag > 0.0f)
            {
                //float knockbackWeight = 1000000.0f;
                //targetVelocity += (knockbackDirection * knockbackMag) * knockbackWeight;
                //++forces;

                knockbackMag -= knockbackMag * knockbackDecel;

                knockbackVel = (knockbackDirection * knockbackMag);

                if (knockbackMag < 0.01f)
                {
                    knockbackMag = 0.0f;
                }
            }

			if (movementForce != Vector3.zero && usingMovementForce)
            {
                targetVelocity += new Vector3(movementForce.x, 0.0f, movementForce.z);
                ++forces;
            }


            if (forces > 0)
            {
                targetVelocity /= (float)forces;

                targetVelocity = new Vector3(targetVelocity.x, 0.0f, targetVelocity.z);
            }

            targetVelocity = (targetVelocity.normalized * speed) + knockbackVel;
        }

		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = rigidbody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;
      
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

		//rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));
	}

	public virtual void Move(Vector3 motion)
	{
		movementForce = motion;
        movementForce.y = 0.0f;

		//FixRotationOrthogonally(motion);
	}

    public virtual void SpecialMove(Vector3 motion)
    {
        specialMovementForce = motion;
        specialMovementForce.y = 0.0f;

		//FixRotationOrthogonally(motion);
    }

	public virtual void FixRotationOrthogonally(Vector3 curDirection)
	{
        if (Mathf.Abs(movementForce.x) > Mathf.Abs(movementForce.z))
        {
            float sign = movementForce.x > 0.0f ? 1.0f : -1.0f;
            transform.LookAt(transform.position + new Vector3(1.0f * sign, 0.0f, 0.0f));
        }
        else if (Mathf.Abs(movementForce.x) < Mathf.Abs(movementForce.z))
        {
            float sign = movementForce.z > 0.0f ? 1.0f : -1.0f;
            transform.LookAt(transform.position + new Vector3(0.0f, 0.0f, 1.0f * sign));
        }
	}

	public virtual void SetKnockback(Vector3 direction, float mag)
	{
		knockbackDirection = new Vector3(direction.x, 0.0f, direction.z);
		knockbackMag = mag * 100.0f;
	}

	public void MoveAlongGrid(Vector3 direction)
	{
		if (!moving)
		{
			moving = true;
			timeAccum = 0.0f;
			startPos = transform.position;
			targetPos = startPos + direction.normalized * offset;
		};
	}

	public void StopMovingAlongGrid()
	{
		timeAccum = 1.0f;
		moving = false;
	}

    public void StopMotion()
    {
        movementForce = Vector3.zero;
    }

	public void EnableMovementForce(bool b)
	{
		usingMovementForce = b;
	}

    public void StopSpecialMovement()
    {
        specialMovementForce = Vector3.zero;
    }
}
