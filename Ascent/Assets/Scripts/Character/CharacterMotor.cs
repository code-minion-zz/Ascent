using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterMotor : MonoBehaviour
{
	public GameObject actor;

	private Vector3 movementForce;
	private Vector3 targetVelocity;
	private Vector3 knockbackDirection;

	private float currentSpeed = 0.0f;
	private float originalSpeed = 6.0f;
	private float buffBonusSpeed;

	private float maxSpeed = 6.0f;
	private float minSpeed = 5.5f;
	private float acceleration = 1.0f;
	private float maxVelocityChange = 5.0f;
	
	private float knockbackMag;
	private float knockbackDecel = 0.65f;

	private bool isActionHaltingMovement = false;
	private bool isActionRotationMovement = false;
	private bool usingStandardMovement = true;
	private bool isRunAttacking = false;

	private bool isMovingAlongGrid;
	private const float gridUnitOffset = 1.0f;
	private const float timeToMoveAlongGrid = 0.5f;
	private float gridMovementTimeAccum;
	private Vector3 gridStartPos;
	private Vector3 gridTargetPos;

	/// <summary>
	/// Speed without any buffs applied
	/// </summary>
	public float OriginalSpeed
	{
		get { return originalSpeed; }
		set { originalSpeed = value; }
	}

	/// <summary>
	/// Speed with buffs applied
	/// </summary>
	public float BuffBonusSpeed
	{
		get { return buffBonusSpeed; }
		set { buffBonusSpeed = value; }
	}

	/// <summary>
	/// Maximum speed increase per frame
	/// </summary>
	public float MaxVelocityChange
	{
		get { return maxVelocityChange; }
		set { maxVelocityChange = value; }
	}

	/// <summary>
	/// Rate of speed gain
	/// </summary>
	public float Acceleration
	{
		get { return acceleration; }
		set { acceleration = value; }
	}

	/// <summary>
	/// Max speed threshold
	/// </summary>
	public float MaxSpeed
	{
		get { return maxSpeed; }
		set { maxSpeed = value; }
	}

	/// <summary>
	/// Minimum amount of speed threshold.
	/// </summary>
	public float MinSpeed
	{
		get { return minSpeed; }
		set { minSpeed = value; }
	}

	public bool IsHaltingMovementToPerformAction
	{
		get { return isActionHaltingMovement; }
		set { isActionHaltingMovement = value; }
	}

	public bool IsHaltingRotationToPerformAction
	{
		get { return isActionRotationMovement; }
		set { isActionRotationMovement = value; }
	}

	public bool IsUsingMovementForce
	{
		get { return usingStandardMovement; }
	}

    public Vector3 TargetVelocity
    {
        get { return targetVelocity; }
    }

	public bool IsMovingAlongGrid
	{
		get { return isMovingAlongGrid; }
		set { isMovingAlongGrid = value; }
	}

	
	public virtual void Initialise()
	{
		// Physics is mostly disabled and handled by code.
		rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}

	public virtual void FixedUpdate()
	{
		if (IsMovingAlongGrid)
		{
			ProcessGridMovement();
		}
		else
		{
			ProcessMovement();
		}
	}

	protected void ProcessMovement()
	{
		// Reset the targets
		targetVelocity = Vector3.zero;
		int forces = 0;

		// Calculate knockback velocity
		Vector3 knockbackVel = ProcessKnockback();
		//if (knockbackVel != Vector3.zero)
		//{
		//    targetVelocity += knockbackVel;
		//    ++forces;
		//}

		// movementForce is fed into the Motor based on user inputs or the SteeringAI
		Vector3 standardMovement = ProcessStandardMovement(); // Also works out current speed
		if (standardMovement != Vector3.zero)
		{
			targetVelocity += standardMovement;
			++forces;
		}

		// Combine the forces if any
		if (forces > 0)
		{
			targetVelocity /= (float)forces;
			targetVelocity = new Vector3(targetVelocity.x, 0.0f, targetVelocity.z);
		}

		// Apply speed to the velocity
		// Apply the knockback separately
		targetVelocity = (targetVelocity * currentSpeed) + knockbackVel;

		// Apply a force that attempts to reach target velocity
		Vector3 velocity = rigidbody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);

		float buffedMaxVelocityChange = maxVelocityChange + (buffBonusSpeed * 0.5f);

		velocityChange.x = Mathf.Clamp(velocityChange.x, -buffedMaxVelocityChange, buffedMaxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -buffedMaxVelocityChange, buffedMaxVelocityChange);
		velocityChange.y = 0;

		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	protected void ProcessRunAttackMovement()
	{
		//Vector3 standardMovement = ProcessStandardMovement();
	}

	protected Vector3 ProcessKnockback()
	{
		Vector3 knockbackVel = Vector3.zero;

		if (knockbackMag > 0.0f)
		{
			knockbackVel = (knockbackDirection * knockbackMag);

			// The knockback deaccellerates each frame
			knockbackMag -= knockbackMag * knockbackDecel;

			// If it is too small it is ignored.
			if (knockbackMag < 0.01f)
			{
				knockbackMag = 0.0f;
			}
		}

		return knockbackVel;
	}

	protected Vector3 ProcessStandardMovement()
	{
		Vector3 movement = Vector3.zero;

		if (movementForce != Vector3.zero && usingStandardMovement)
		{
			movement = new Vector3(movementForce.x, 0.0f, movementForce.z);

			// The highest value on either axis is used as the accelleration value
			float speed = Mathf.Abs(movementForce.x) > Mathf.Abs(movementForce.z) ? Mathf.Abs(movementForce.x) : Mathf.Abs(movementForce.z);
			float maxAccel = speed;

			// Buffs are added to the speed
			float buffedSpeed = maxSpeed + buffBonusSpeed;

			// CurrentSpeed is calculated using acceleration
			currentSpeed += (speed * buffedSpeed) * Acceleration * Time.deltaTime;
			currentSpeed = Mathf.Clamp(currentSpeed, MinSpeed, maxAccel * buffedSpeed);
		}
		else if (movementForce == Vector3.zero)
		{
		    currentSpeed = 0.0f;
		}

		return movement;
	}

	protected void ProcessGridMovement()
	{
		// TODO: Do a check in HeroController to stop prevent this movement happening.
		// Grid movement uses Lerp. This is dangerous because it can go through other objects.
		gridMovementTimeAccum += Time.deltaTime;
		if (gridMovementTimeAccum > timeToMoveAlongGrid)
		{
			gridMovementTimeAccum = 1.0f;
			IsMovingAlongGrid = false;
		}

		transform.position = Vector3.Lerp(gridStartPos, gridTargetPos, gridMovementTimeAccum / timeToMoveAlongGrid);
	}

	public virtual void Move(Vector3 motion)
	{
		movementForce = motion;
        movementForce.y = 0.0f;

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

	public virtual void LookAt(Vector3 curDirection)
	{
		transform.LookAt(curDirection, Vector3.up);
	}

	public virtual void SetKnockback(Vector3 direction, float mag)
	{
		knockbackDirection = new Vector3(direction.x, 0.0f, direction.z);
		knockbackMag = mag * 100.0f;
	}

	public void MoveAlongGrid(Vector3 direction)
	{
		if (!IsMovingAlongGrid)
		{
			IsMovingAlongGrid = true;
			gridMovementTimeAccum = 0.0f;
			gridStartPos = transform.position;
			gridTargetPos = gridStartPos + direction.normalized * gridUnitOffset;
		};
	}

	public void StopMovingAlongGrid()
	{
		gridMovementTimeAccum = 1.0f;
		IsMovingAlongGrid = false;
	}

    public void StopMotion()
    {
        movementForce = Vector3.zero;
        currentSpeed = 0.0f;
    }

	public void EnableStandardMovement(bool b)
	{
		usingStandardMovement = b;
	}
}
