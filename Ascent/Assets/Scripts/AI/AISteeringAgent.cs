using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AISteeringAgent  
{
	private bool enabled = true;
	public bool Enabled
	{
		get { return enabled; }
		set { enabled = value; }
	}

	private bool canMove = true;
	public bool CanMove
	{
		get { return canMove; }
		set { canMove = value; }
	}

    private Vector3 startPos;
    public Vector3 StartPosition
    {
        get { return startPos; }
        set { startPos = value; }
    }

    private Vector3 targetPos;

#pragma warning disable 0414
    private Vector3 posLastFrame;

	private Character targetCharacter;
	public Character TargetCharacter
	{
		get { return targetCharacter; }
		set
		{
			targetCharacter = value;
			startPos = motor.transform.position;
			hasTarget = true;
		}
	}

    private Room containedRoom;

    protected CharacterMotor motor;
    public CharacterMotor Motor
    {
        get { return motor; }
        set { motor = value; }
    }

    protected float closeEnoughRange = 0.25f;
    public float CloseEnoughRange
    {
        get { return closeEnoughRange; }
        set { closeEnoughRange = value; }
    }

    protected float rotationSpeed = 1.0f;
    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    protected bool isRunningAway;
    public bool IsRunningAway
    {
        get { return isRunningAway; }
        set { isRunningAway = value; }
    }

    protected bool canRotate = true;
    public bool CanRotate
    {
        get { return canRotate; }
        set { canRotate = value; }
    }

    protected bool runIfTooClose = false;
    public bool RunIfTooClose
    {
        get { return runIfTooClose; }
        set { runIfTooClose = value; }
    }

	protected float distanceToKeepFromTarget = 1.75f;
	public float DistanceToKeepFromTarget
	{
		get { return distanceToKeepFromTarget; }
		set { distanceToKeepFromTarget = value; }
	}

    protected bool hasTarget = false;

    public delegate void TargetReached();
    public event TargetReached OnTargetReached;

	protected bool active;

	public void SetActive(bool active)
	{
		this.active = active;
	}

    public void Initialise(CharacterMotor motor)
    {
        startPos = motor.transform.position;

		active = true;
        this.motor = motor;
    }

    public void Process()
    {
		if (enabled)
		{
			bool moveThisFrame = true;
            bool moveBack = false;
			if (hasTarget)
			{
				if (targetCharacter != null)
				{
					if (motor.IsUsingMovementForce)
					{
						if (canRotate)
						{
                            if (IsRunningAway)
							{
                                // Rotate away
								motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(motor.transform.position - targetCharacter.transform.position, Vector3.up), rotationSpeed);
							}
							else
							{
								// If you are too close to the target. Do not get any closer!
                                if (Vector3.Distance(motor.transform.position, targetCharacter.transform.position) <= distanceToKeepFromTarget)
                                {
                                    if (runIfTooClose)
                                    {
                                        moveBack = true;
                                        //motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(motor.transform.position - targetCharacter.transform.position, Vector3.up), rotationSpeed);
                                    }
                                    else
                                    {
                                        moveThisFrame = false;
                                        motor.StopMotion();
                                    }
                                }
                                //else
                                {
                                    // Rotate toward
                                    motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(targetCharacter.transform.position - motor.transform.position, Vector3.up), rotationSpeed);
                                }
							}
						}
					}

					if (MathUtility.IsWithinCircle(motor.transform.position, targetCharacter.transform.position, closeEnoughRange))
					{
						if (OnTargetReached != null)
						{
							OnTargetReached.Invoke();
						}
					}
				}
				else
				{
					if (motor.IsUsingMovementForce)
					{
						motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(targetPos - motor.transform.position, Vector3.up), rotationSpeed);
					}

					if (MathUtility.IsWithinCircle(motor.transform.position, targetPos, closeEnoughRange))
					{
						if (OnTargetReached != null)
						{
							OnTargetReached.Invoke();
						}
					}
				}

				if (moveThisFrame && canMove)
				{
                    if (moveBack)
                    {
                        motor.Move(-motor.transform.forward);
                    }
                    else
                    {
                        motor.Move(motor.transform.forward);
                    }
				}
			}
		}
    }

	public void RemoveTarget()
	{
		targetCharacter = null;
		targetPos = Vector3.zero;
		hasTarget = false;
	}

    public void SetTargetPosition(Vector3 targetPosition)
    {
        startPos = motor.transform.position;
        targetPos = targetPosition;
        hasTarget = true;

        posLastFrame = startPos;
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
		if (!active)
		{
			return;
		}
        if(motor != null)
        {
            Color red = new Color(1.0f, 0.0f, 0.0f, 0.35f);
            Color green = new Color(0.0f, 1.0f, 0.0f, 0.35f);

            Color blue = new Color(0.0f, 0.0f, 1.0f, 0.75f);
            Color yellow = new Color(1.0f, 1.0f, 0.0f, 0.35f);

            if(hasTarget)
            {
                Vector3 pos = targetPos;
                if (targetCharacter != null)
                {
                    pos = targetCharacter.transform.position;

                    // From actor to direction of target
                    Debug.DrawLine(new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), 
                        new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z) + (new Vector3(pos.x, 0.2f, pos.z) - new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z)).normalized * 1.5f, 
                        red, 0.01f, false);
                    Debug.DrawLine(new Vector3(posLastFrame.x, 0.2f, posLastFrame.z), new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), green, 0.5f, false);
                }
                else
                {
                    Debug.DrawLine(new Vector3(startPos.x, 0.2f, startPos.z), new Vector3(pos.x, 0.2f, pos.z), red);
                    Debug.DrawLine(new Vector3(startPos.x, 0.2f, startPos.z), new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), green, 0.25f, false);
                }

                posLastFrame = motor.transform.position;
            }

            Debug.DrawLine(motor.transform.position, motor.transform.position + motor.TargetVelocity * 1.5f, yellow);
            Debug.DrawLine(motor.transform.position, motor.transform.position + motor.transform.forward * 1.5f, blue);

        }
    }
#endif

    float maxSpeed;
    Vector3 velocity;
    Vector3 maxForce;
	Vector3 position;
	Vector3 acceleration;

	public void Update()
	{
		velocity += acceleration;
		Vector3.ClampMagnitude(velocity, maxSpeed);
		position += velocity;
		acceleration *= 0.0f;
	}

	private void ApplyForce(Vector3 force)
	{
		acceleration += force;
	}

	private void Seek(Vector3 target)
	{
		Vector3 desired = target - motor.transform.position;
		desired.Normalize();
		desired *= maxSpeed;
		Vector3 steer = desired - velocity;
		steer = Vector3.Max(steer, maxForce);

		ApplyForce(steer);
	}

	private void Flee(Vector3 target)
	{
		Vector3 desired = motor.transform.position - target;
		desired.Normalize();
		desired *= maxSpeed;
		Vector3 steer = desired - velocity;
		steer = Vector3.Max(steer, maxForce);

		ApplyForce(steer);
	}

	//private void Pursuit(Vector3 target)
	//{
	//    float maxVelocity = 3.0f;
	//    Vector3 distance = (target - motor.transform.position);
	//    float updatesAhead = distance.magnitude / maxVelocity;
	//    Vector3 futurePosition = target + (target.velocity * updatesAhead); // This can be improved by taking previous frames
	//    Vector3 desired = Seek(futurePosition);

	//    ApplyForce(desired);
	//}

	//private void Evade(Vector3 target)
	//{
	//    float maxVelocity = 3.0f;
	//    Vector3 distance = (motor.transform.position - target);
	//    float updatesAhead = distance.magnitude / maxVelocity;
	//    Vector3 futurePosition = target + (target.velocity * updatesAhead); // This can be improved by taking previous frames
	//    Vector3 desired = Seek(futurePosition);

	//    ApplyForce(desired);
	//}

	private void Arrive(Vector3 target)
	{
		Vector3 desired = target - motor.transform.position;

		float dist = desired.magnitude;
		desired.Normalize();

		float closeEnoughRadius = 2.0f;

		if (dist < closeEnoughRadius)
		{
			float mag = dist / closeEnoughRadius; // Mag set according to dist
			desired *= dist;
		}
		else
		{
			desired *= maxSpeed;
		}

		Vector3 steer = desired - velocity;
		steer = Vector3.Max(steer, maxForce);
		ApplyForce(steer);
	}

	private void Wander()
	{

	}

	private void ObstacleAvoidance()
	{
	}

	private void Contain()
	{

	}

	private void WallFollow()
	{

	}

	private void PathFollow()
	{

	}

    Vector3 position;

    public void Update()
    {
        position = motor.transform.position;
    }


    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steer = desired - velocity;
        steer = Vector3.Max(steer, maxForce);

        return steer;
    }

    private Vector3 Arrive(Vector3 target)
    {
        Vector3 desired = target - position;

        return desired;
    }


    // Method checks for nearby boids and steers away
    private Vector3 Separate(List<Character> characters)
    {
        float desiredSeparation = 25.0f;
        Vector3 steer = Vector3.zero;
        int count = 0;

        Vector3 position = motor.transform.position;

        // Check if all others are too close
        foreach(Character c in characters)
        {
            Vector3 otherPosition = c.transform.position;

            float d = Vector3.Distance(position, otherPosition);

            // If distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < desiredSeparation))
            {
                // Calculate vector pointing away from neighbor
                Vector3 diff = position - otherPosition;
                diff.Normalize();
                diff /= d; // weight by distance
                steer += diff;
                count++;
            }
        }

        // Average -- divide by how many 
        if (count > 0)
        {
            steer /= (float)count;
        }

        // As long as the vector is greater than 0
        if(steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= maxSpeed; // max speed
            steer -= velocity; // velocity
            steer = Vector3.Max(steer, maxForce); // maxforce
        }

        return steer;
    }

    // For every nearby boid in the system, calculate the average velocity
    private Vector3 Allignment(List<Character> characters)
    {
        float neighbourDist = 50.0f;
        Vector3 sum = Vector3.zero;
        int count = 0;

        Vector3 position = motor.transform.position;

        // Check if all others are too close
        foreach (Character c in characters)
        {
            Vector3 otherPosition = c.transform.position;

            float d = Vector3.Distance(position, otherPosition);

            // If distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < neighbourDist))
            {
                sum += c.Motor.TargetVelocity; // velocity
                count++;
            }
        }

        if (count > 0)
        {
            sum /= (float)count;

            sum.Normalize();
            sum *= maxSpeed; // max speed
            sum -= velocity; // velocity
            sum = Vector3.Max(sum, maxForce); // maxforce

            return sum;
        }

        return Vector3.zero;
    }

    private Vector3 Cohesion(List<Character> characters)
    {
        float neighbourDist = 50.0f;
        Vector3 sum = Vector3.zero;
        int count = 0;

        Vector3 position = motor.transform.position;

        foreach (Character c in characters)
        {
            Vector3 otherPosition = c.transform.position;

            float d = Vector3.Distance(position, otherPosition);

            // If distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < neighbourDist))
            {
                sum += c.Motor.TargetVelocity; // velocity
                count++;
            }
        }

        if (count > 0)
        {
            sum /= (float)count;
            return sum; // return Seek(sum);
        }
        
        return Vector3.zero;
    }
}
