//using UnityEngine;
//using System.Collections;

//public class CharacterMotor : MonoBehaviour 
//{
//    public GameObject actor;

//    private CharacterController controller;

//    public bool canMove = true;

//    private Vector3 velocity;
//    private Vector3 movementForce;
//    private Vector3 knockbackDirection;
//    private float knockbackMag;
//    private float knockbackDecel = 1.5f;
//    private float speed = 6.0f;

//    public virtual void Initialise()
//    {
//        controller = actor.GetComponentSafe<CharacterController>();
//    }

//    public virtual void LateUpdate()
//    {
//        if(transform.position.y > 0.0f)
//        {
//            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
//        }
//        int forces = 0;

//        if (knockbackMag > 0.0f)
//        {
//            float knockbackWeight = 1000000.0f;
//            velocity += (knockbackDirection * knockbackMag) * knockbackWeight;
//            ++forces;

//            knockbackMag -= knockbackMag / 1.1f;

//            if (knockbackMag < 0.01f)
//            {
//                knockbackMag = 0.0f;
//            }
//        }
//        else if (movementForce != Vector3.zero)
//        {
//            velocity += movementForce;
//            ++forces;
//        }


//        if(forces > 0)
//        {
//            velocity /= (float)forces;

//            velocity = new Vector3(velocity.x, 0.0f, velocity.z);
//        }

//        controller.Move((velocity.normalized * speed) * Time.deltaTime);

//        movementForce = Vector3.zero;
//        velocity = Vector3.zero;

//        CharacterTilt tilt = GetComponentInChildren<CharacterTilt>();
//        tilt.Process();

//        Shadow shadow = GetComponentInChildren<Shadow>();
//        shadow.Process();
//    }

//    public virtual void Move(Vector3 motion)
//    {
//        movementForce = motion;
//    }

//    public virtual void SetKnockback(Vector3 direction, float mag)
//    {
//        knockbackDirection  = direction;
//        knockbackMag = mag * 1000.0f;
//    }
//}


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class CharacterMotor : MonoBehaviour
{
	public GameObject actor;

	private Vector3 movementForce;
	public float speed = 6.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 5.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;

	private Vector3 knockbackDirection;
	private float knockbackMag;
	private float knockbackDecel = 0.65f;

	// Grid Movement
	public bool moving;
	float offset = 1.0f;
	float moveTime = 0.5f;
	float timeAccum;

	Vector3 startPos;
	Vector3 targetPos;
	
	public virtual void Initialise()
	{
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
		Vector3 targetVelocity = Vector3.zero;
		//Debug.Log(targetVelocity);
		//targetVelocity = transform.TransformDirection(targetVelocity);

		Vector3 knockbackVel = Vector3.zero;

		int forces = 0;

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
		
		if (movementForce != Vector3.zero)
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
	}

	public virtual void SetKnockback(Vector3 direction, float mag)
	{
		knockbackDirection = new Vector3(direction.x, 0.0f, direction.z);
		knockbackMag = mag * 1000.0f;
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
}
