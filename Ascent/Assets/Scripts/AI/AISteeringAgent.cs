using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AISteeringAgent : MonoBehaviour
{
    public float maxSpeed;
	public float maxForce;

    private Vector3 velocity;
    private Vector3 acceleration;

    public float arriveRadius = 3.0f;

    public float wanderCircleDistance = 5.0f;
    public float wanderCircleRadius = 3.0f;
    private float wanderAngle = 0.0f;
	private int wanderFrames;

    private int forceCount = 0;

	public bool flock;
	public bool wander;


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


    public Character avoidanceCharacter;
	public Character targetCharacter;
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

    protected float closeEnoughRange = 1.0f;
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

    public void Initialise(CharacterMotor motor)
    {
        startPos = motor.transform.position;
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
		if (!gameObject.activeInHierarchy)
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

	protected Transform transformCache;
	public void Start()
	{
		transformCache = transform;
		velocity = transform.forward;
	}

	public Vector3 Steer()
	{
		ResetForces();

		if (wander)
		{
			ApplyForce(Wander());
			
		}
		else 
		{
			Flock(Game.Singleton.Tower.CurrentFloor.CurrentRoom.GetCharacterList(Character.EScope.Enemy));
		}
		//ApplyForce(Arrive(targetCharacter.gameObject));
		//ObstacleAvoidance(avoidanceCharacter);
		//ApplyForce(PatrolCircle());

		velocity += acceleration / (float)forceCount; // (Mass is not used)
		Vector3.ClampMagnitude(velocity, maxSpeed);

		// Render Target
		// Render Heading
		// Render Desired

		return velocity * Time.deltaTime;
	}

	private void ResetForces()
	{
		forceCount = 0;
		acceleration *= 0.0f;
		velocity = Vector3.zero;
	}

	private void ApplyForce(Vector3 force)
	{
		acceleration += force;
        forceCount++;
	}

	private Vector3 Seek(Vector3 target)
	{
		Vector3 desired = Vector3.zero;
		desired = target - transformCache.position;
		desired.Normalize();
		desired *= maxSpeed;
		desired -= velocity;

		return desired;
	}

	private Vector3 Seek(GameObject target)
    {
        if (target != null) 
			return Seek(target.transform.position);

		return Vector3.zero;
    }

	private Vector3 Flee(Vector3 target)
	{
		Vector3 desired = Vector3.zero;
		desired = transformCache.position - target;
		desired.Normalize();
		desired *= maxSpeed;
		desired -= velocity;

		return desired;
	}

	private Vector3 Flee(GameObject target)
	{
		if (target != null)
			return Seek(target.transform.position);

		return Vector3.zero;
	}

	private Vector3 Arrive(Vector3 target)
	{
		Vector3 desired = Vector3.zero;
		desired = target - transformCache.position;

		float distance = desired.magnitude;
		desired.Normalize();

		if (distance < arriveRadius)
		{
			float speed = (distance / arriveRadius) * maxSpeed; // Speed set according to dist
			
			desired *= speed;

			// Check if close enough and notify
			if (distance < closeEnoughRange)
			{
				if (OnTargetReached != null)
				{
					OnTargetReached.Invoke();
				}
			}
		}
		else
		{
			desired *= maxSpeed;
		}


		desired -= velocity;

		return desired;
	}

	private Vector3 Arrive(GameObject target)
    {
        if (target != null) 
			return Arrive(target.transform.position);

		return Vector3.zero;
    }

	private Vector3 Wander()
	{
        // Calculate centre of circle
        Vector3 circlePos = transformCache.position + velocity;
		circlePos.Normalize();
		circlePos *= wanderCircleDistance;

		// Calculate displacement
		Vector3 displacement = Vector3.zero;
		displacement = new Vector3(0.0f, 0.0f, -1.0f);
		displacement *= wanderCircleRadius;

		displacement.x = Mathf.Cos(wanderAngle) * displacement.magnitude;
		displacement.z = Mathf.Sin(wanderAngle) * displacement.magnitude;

		wanderFrames++;
		if (wanderFrames >= Random.Range(50, 100))
		{
			wanderFrames = 0;
			float jitter = Random.Range(0.0f, Mathf.PI * 2.0f);
			wanderAngle += jitter + ((Random.value) * (Mathf.PI / 36.0f));
		}

		Vector3 target = circlePos + displacement;
        Vector3 desired = target - transformCache.position;
		desired *= maxSpeed;
        desired -= velocity;

		return desired;
	}

	private Vector3 PatrolCircle()
	{
		// Calculate centre of circle
		Vector3 circlePos = transformCache.position + velocity;
		circlePos.Normalize();
		circlePos *= wanderCircleDistance;

		// Calculate displacement
		Vector3 displacement = Vector3.zero;
		displacement = new Vector3(0.0f, 0.0f, -1.0f);
		displacement *= wanderCircleRadius;

		displacement.x = Mathf.Cos(wanderAngle) * displacement.magnitude;
		displacement.z = Mathf.Sin(wanderAngle) * displacement.magnitude;

		wanderAngle += ((Random.value) * (Mathf.PI / 36.0f));

		Vector3 target = circlePos + displacement;
		Vector3 desired = target - transformCache.position;
		desired *= maxSpeed;
		desired -= velocity;

		return desired;
	}

	private void ObstacleAvoidance(Character target)
	{
        if(target == null)
            return;

        float seeAheadDistance = 3.0f;
        Vector3 ahead = transform.position + velocity.normalized * seeAheadDistance;
        Vector3 ahead2 = transform.position + velocity.normalized * seeAheadDistance * 0.5f;

        if(!LineIntersectCircle(ahead, ahead2, target.transform.position, 2.0f))
            return;

        Vector3 avoidanceForce = ahead - target.transform.position;
        avoidanceForce = avoidanceForce.normalized * maxSpeed;

        Vector3 steer = avoidanceForce - velocity;
        //steer = Vector3.Min(steer, maxForce);

        ApplyForce(steer);
	}

    private bool LineIntersectCircle(Vector3 ahead, Vector3 ahead2, Vector3 obstaclePosition, float obstacleRadius)
    {
        return Vector3.Distance(obstaclePosition, ahead) <= obstacleRadius ||
                Vector3.Distance(obstaclePosition, ahead2) <= obstacleRadius;
    }

	private void WallFollow()
	{
        // Ray forward
        // If collides with wall
        // Move along wall
	}

	private void PathFollow()
	{
		// Also need pathfinding
	}

	private Vector3 Flock(List<Character> flockers)
	{
		Vector3 separation = Separate(flockers);
		Vector3 alignment = Allignment(flockers);
		Vector3 cohesion = Cohesion(flockers);

		// Alter weights
		separation *= maxSpeed * 0.1f;
		alignment *= maxSpeed * 0.5f;
		cohesion *= maxSpeed * 0.4f;

		ApplyForce(separation);
		ApplyForce(alignment);
		ApplyForce(cohesion);

		//Vector3 flockForce = separation + alignment + cohesion;

		//Vector3 desired = flockForce - transformCache.position;
		//desired.Normalize();
		//desired *= maxSpeed;
		//desired -= velocity;

		return Vector3.zero;
	}

    // Method checks for nearby boids and steers away
    private Vector3 Separate(List<Character> characters)
    {
        float desiredSeparation = 5.0f;
        Vector3 sum = Vector3.zero;
        int count = 0;

        Vector3 position = transform.position;

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
                sum += diff;
                count++;
            }
        }

        // Average -- divide by how many 
		if (count > 0)
		{
			sum /= (float)count;
			sum.Normalize();
			sum *= maxSpeed;
			sum -= velocity;
			sum = Vector3.ClampMagnitude(sum, maxForce);

			return sum;
		}

        return Vector3.zero;
    }

    // For every nearby boid in the system, calculate the average velocity
    private Vector3 Allignment(List<Character> characters)
    {
        float neighbourDist = 5.0f;
        Vector3 sum = Vector3.zero;
        int count = 0;

        Vector3 position = transform.position;

        // Check if all others are too close
        foreach (Character c in characters)
        {
            Vector3 otherPosition = c.transform.position;

            float d = Vector3.Distance(position, otherPosition);

            // If distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < neighbourDist))
            {
				sum += transform.forward; // velocity
                count++;
            }
        }

        if (count > 0)
        {
            sum /= (float)count;

            sum.Normalize();
            sum *= maxSpeed; // max speed
            sum -= velocity; // velocity
			sum = Vector3.ClampMagnitude(sum, maxForce);

            return sum;
        }

        return Vector3.zero;
    }

    private Vector3 Cohesion(List<Character> characters)
    {
        float neighbourDist = 5.0f;
        Vector3 sum = Vector3.zero;
        int count = 0;

        Vector3 position = transform.position;

        foreach (Character c in characters)
        {
            Vector3 otherPosition = c.transform.position;

            float d = Vector3.Distance(position, otherPosition);

            // If distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((d > 0) && (d < neighbourDist))
            {
				sum += otherPosition; // velocity
                count++;
            }
        }

        if (count > 0)
        {
            sum /= (float)count;
            return Seek(sum);
        }
        
        return Vector3.zero;
    }
}
