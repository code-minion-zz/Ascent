using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AISteeringAgent : MonoBehaviour
{
	public enum ESteerTypes
	{
		None = 0x00000,
		Seek = 0x00002,
		Flee = 0x00004,
		Arrive = 0x00008,

		Wander = 0x00010,
		FleeInRange = 0x00020,
		Evade = 0x00040,
		Pursuit = 0x00080,

		ObstacleAvoidance = 0x00100,
		WallAvoidance = 0x00200,
		FollowPath = 0x00400,

		Cohesion = 0x01000,
		Separation = 0x02000,
		Allignment = 0x04000,
		Flock = 0x08000,

		OffsetPursuit = 0x10000,
	};

	public ESteerTypes steerTypes = 0;

    public float maxSpeed;
	public float maxForce;
	public float mass;

    private Vector3 velocity;
	public Vector3 Velocity
	{
		get { return velocity; }
	}

    private Vector3 acceleration;

	private Vector3 position;
	private Vector3 heading;
	
    public float arriveRadius = 3.0f;

    public float wanderCircleDistance = 5.0f;
    public float wanderCircleRadius = 3.0f;
	public float wanderJitter = 0.15f;

	private float wanderAngle;
	private Vector3 wanderTarget;

	public float fleeDistance = 5.0f;

	public bool stopIfCloseEnough;
	public float closeEnoughDistance = 2.0f;
	public bool moveAwayIfTooClose;
	public float tooCloseDistance = 1.0f;
	public float distanceToKeepFromTarget = 1.75f;

	public Vector2 pursuitOffset;
	public Character pursuitOffsetLeader;

	public AIPath path;
	private int currentPathNode;

	private bool closeEnough = false;
	public bool CloseEnough
	{
		get { return closeEnough; }
	}

//#pragma warning disable 0414
    private Vector3 posLastFrame;

    public delegate void TargetReached();
    public event TargetReached OnTargetReached;

	public delegate void PathCompleted();
	public event PathCompleted OnPathCompleted;

	public Vector3 Steer(Vector3 target)
	{
		closeEnough = false;
		posLastFrame = position; 
		position = transform.position;

		// Calculate the combined force from each steering behaviour
		Vector3 steeringForce = Calculate(target);

		steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

		// Acceleration = Force/Mass
		Vector3 acceleration = steeringForce / mass;

		// Update velocity
		velocity += acceleration * Time.deltaTime;

		// Do not allow velocity to exceed max
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

		heading = velocity.normalized;

		return velocity * Time.deltaTime;
	}

	public Vector3 Steer(GameObject target)
	{
		closeEnough = false;
		posLastFrame = position;
		position = transform.position;

		// Calculate the combined force from each steering behaviour
		Vector3 steeringForce = Calculate(target);

		steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

		// Acceleration = Force/Mass
		Vector3 acceleration = steeringForce / mass;

		// Update velocity
		velocity += acceleration * Time.deltaTime;

		// Do not allow velocity to exceed max
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

		heading = velocity.normalized;

		return velocity * Time.deltaTime;
	}

	private Vector3 Calculate(Vector3 target)
	{
		Vector3 combinedForces = Vector3.zero;

		if (On(ESteerTypes.Seek))
		{
			combinedForces += Seek(target);
		}
		if (On(ESteerTypes.Arrive))
		{
			combinedForces += Arrive(target, arriveRadius);
		}
		if (On(ESteerTypes.Wander))
		{
			combinedForces += Wander();
		}
		if (On(ESteerTypes.FleeInRange))
		{
			combinedForces += FleeWithinRange(target);
		}
		if (On(ESteerTypes.ObstacleAvoidance))
		{
			combinedForces += ObstacleAvoidance();
		}

		return combinedForces;
	}

	private Vector3 Calculate(GameObject target)
	{
		Vector3 combinedForces = Vector3.zero;

		if (On(ESteerTypes.Seek))
		{
			combinedForces += Seek(target);
		}
		if (On(ESteerTypes.Arrive))
		{
			combinedForces += Arrive(target, arriveRadius);
		}
		if (On(ESteerTypes.Wander))
		{
			combinedForces += Wander();
		}
		if (On(ESteerTypes.FleeInRange))
		{
			combinedForces += FleeWithinRange(target);
		}
		if (On(ESteerTypes.ObstacleAvoidance))
		{
			combinedForces += ObstacleAvoidance();
		}

		return combinedForces;
	}

	private bool On(ESteerTypes steerType)
	{
		return (steerTypes & steerType) == steerType;
	}

	private Vector3 Seek(Vector3 target)
	{
		if (IsTooClose(target))
		{
			return Flee(target);
		}

		if (IsCloseEnough(target))
		{
			if (OnTargetReached != null)
			{
				OnTargetReached.Invoke();
			}

			closeEnough = true;

			return Stop(target);
		}

		// Seek as normal
		Vector3 desiredVelocity = Vector3.zero;
		desiredVelocity = target - position;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		desiredVelocity -= velocity;

		return desiredVelocity;
	}

	private Vector3 Seek(GameObject target)
    {
        if (target != null) 
			return Seek(target.transform.position);

		return Vector3.zero;
    }

	private Vector3 Flee(Vector3 target)
	{
		Vector3 desiredVelocity = Vector3.zero;
		desiredVelocity = position - target;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		desiredVelocity -= velocity;

		return desiredVelocity;
	}

	private Vector3 Flee(GameObject target)
	{
		if (target != null)
			return Flee(target.transform.position);

		return Vector3.zero;
	}

	private bool IsTooClose(Vector3 target)
	{
		if (!moveAwayIfTooClose)
			return false;

		Vector3 desired = target - position;

		float distance = Mathf.Abs(desired.magnitude);

		desired.Normalize();

		return distance <= tooCloseDistance;
	}

	private bool IsCloseEnough(Vector3 target)
	{
		if (!stopIfCloseEnough)
			return false;

		Vector3 desired = target - position;

		float distance = Mathf.Abs(desired.magnitude);

		desired.Normalize();

		return distance <= closeEnoughDistance;
	}

	private Vector3 Stop(Vector3 target)
	{
		Vector3 desired = target - position;

		velocity = desired * 0.1f;

		if (OnTargetReached != null)
		{
			OnTargetReached.Invoke();
		}

		closeEnough = true;
		return velocity;
	}

	private Vector3 FleeWithinRange(Vector3 target)
	{
		// Only flee when within a range that is too close
		if (Vector3.Distance(position, target) > fleeDistance)
		{
			return Vector3.zero;
		}

		Vector3 desiredVelocity = Vector3.zero;
		desiredVelocity = position - target;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		desiredVelocity -= velocity;

		return desiredVelocity;
	}

	private Vector3 FleeWithinRange(GameObject target)
	{
		if (target != null)
			return FleeWithinRange(target.transform.position);

		return Vector3.zero;
	}

	private Vector3 Arrive(Vector3 target, float radius)
	{
		if (IsTooClose(target))
		{
			return Flee(target);
		}

		if (IsCloseEnough(target))
		{
			if (OnTargetReached != null)
			{
				OnTargetReached.Invoke();
			}

			closeEnough = true;

			return Stop(target);
		}

		Vector3 desired = target - position;

		float distance = Mathf.Abs(desired.magnitude);

		// If outside of the arrival range go at normal speed
		if (distance < 0.0f && distance > radius)
		{
			desired *= velocity.magnitude;
		}
		else
		{
			// slow down because in range of arrival
			float newSpeed = (distance / radius) * maxSpeed; // Speed set according to dist
			desired *= newSpeed;
		}

		desired -= velocity;

		return desired;
	}

	private Vector3 Arrive(GameObject target, float radius)
    {
        if (target != null)
			return Arrive(target.transform.position, radius);

		return Vector3.zero;
    }

	private Vector3 Pursuit(Vector3 target, Vector3 targetsVelocity, float targetsSpeed)
	{
		// If the target is ahead and facing me then just seek it

		Vector3 toEvader = target - position;

		Vector3 targetsHeading = targetsVelocity.normalized;

		float relativeHeading = Vector3.Dot(heading, targetsHeading);

		if ((Vector3.Dot(toEvader, heading)) > 0 &&
			(relativeHeading < -0.95f)) // acos(0.95) == 18degs
		{
			return Seek(target);
		}

		// It isn't ahead to predict where it is going

		// Look ahead based on combined speeds and distance
		float lookAheadTime = toEvader.magnitude / (maxSpeed + targetsSpeed);
		lookAheadTime += CalculateTurnAroundTime(target);


		// Finally seek the predicted position
		return Seek(target + (targetsVelocity * lookAheadTime));
	}

	private Vector3 Pursuit(GameObject target)
	{
		AISteeringAgent steer = target.GetComponentInChildren<AISteeringAgent>();
		if (steer == null)
			return Vector3.zero;

		return Pursuit(steer.transform.position, steer.velocity, steer.velocity.magnitude);
	}

	private float CalculateTurnAroundTime(Vector3 target)
	{
		Vector3 toTarget = target - position;
		toTarget.Normalize();

		float dot = Vector3.Dot(heading, toTarget);

		// NOTE: From Matt Buckland's book
		//change this value to get the desired behavior. The higher the max turn
		//rate of the vehicle, the higher this value should be. If the vehicle is
		//heading in the opposite direction to its target position then a value
		//of 0.5 means that this function will return a time of 1 second for the
		//vehicle to turn around.
		const float coefficient = 0.05f;

		//the dot product gives a value of 1 if the target is directly ahead and -1
		//if it is directly behind. Subtracting 1 and multiplying by the negative of
		//the coefficient gives a positive value proportional to the rotational
		//displacement of the vehicle and target.
		return (dot - 1.0f) * -coefficient;		
	}

	private Vector3 OffSetPursuit()
	{
		AISteeringAgent steering = pursuitOffsetLeader.GetComponentInChildren<AISteeringAgent>();
		if(steering == null)
			return Vector3.zero;

		Vector3 offset = new Vector3(pursuitOffset.x, 0.0f, pursuitOffset.y);

		Vector3 leaderOffset = steering.transform.position + offset;

		Vector3 toOffset = leaderOffset - position;

		float lookAhead = toOffset.magnitude / (maxSpeed + steering.velocity.magnitude);

		return Arrive(leaderOffset + steering.velocity * lookAhead, arriveRadius);
	}

	private Vector3 Evade(Vector3 target, Vector3 targetsVelocity, float targetsSpeed)
	{
		Vector3 toPursuer = position - target;

		// Look ahead based on combined speeds and distance
		float lookAheadTime = toPursuer.magnitude / (maxSpeed + targetsSpeed);
		lookAheadTime += CalculateTurnAroundTime(target);
		
		// Finally seek the predicted position
		return Flee(target + (targetsVelocity * lookAheadTime));
	}

	private Vector3 Evade(GameObject target)
	{
		AISteeringAgent steer = target.GetComponentInChildren<AISteeringAgent>();
		if (steer == null)
			return Vector3.zero;

		return Evade(steer.transform.position, steer.velocity, steer.velocity.magnitude);
	}
	
	private Vector3 Wander()
	{
		// Put wander circle infront
		Vector3 wanderCirclePos = position + heading * wanderCircleDistance;
		
		// Random a value on circle
		//wanderJitter += (Random.value * Mathf.PI * 2.0f) + (Random.value * (Mathf.PI * 36.0f)) * Time.deltaTime;
		wanderJitter +=  Random.Range(-1.0f, 1.0f);
		wanderTarget += new Vector3(Mathf.Cos(wanderJitter), 0.0f, Mathf.Sin(wanderJitter));
		wanderTarget.Normalize();

		// Put point back onto the circle
		wanderTarget *= wanderCircleRadius;
		Vector3 target = position + wanderTarget;

		return target - position;
	}

	private Vector3 PatrolCircle()
	{
		// Calculate centre of circle
		Vector3 circlePos = position + velocity;
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
		Vector3 desired = target - position;
		desired *= maxSpeed;
		desired -= velocity;

		return desired;
	}

	private Vector3 ObstacleAvoidance()
	{
		// Detection area is proportional to agent velocity
		const float minRayLength = 1.5f;
		float rayLength = minRayLength + ((velocity.magnitude / maxSpeed) * minRayLength);

		int layerMask = (1 << (int)Layer.Environment);
		
		RaycastHit hitInfo;

		if (!Physics.SphereCast(new Ray(position - heading * 0.1f, heading), 1.0f, out hitInfo, rayLength, layerMask))
		{
			return Vector3.zero;
		}

		if(hitInfo.collider.CompareTag("WallTile"))
		{
			return Vector3.zero;
		}

		// Closer I am to the object the more strongly I want to move away
		float distanceMultiplier = 1.0f + (hitInfo.point - position).magnitude / rayLength;

		Vector3 ahead = position + (heading * rayLength);

		Vector3 avoidanceForce = Vector3.zero;
		avoidanceForce.x = ahead.x - hitInfo.collider.transform.position.x;
		avoidanceForce.z = ahead.z - hitInfo.collider.transform.position.z;

		return avoidanceForce;
	}

    private bool LineIntersectCircle(Vector3 ahead, Vector3 ahead2, Vector3 obstaclePosition, float obstacleRadius)
    {
        return Vector3.Distance(obstaclePosition, ahead) <= obstacleRadius ||
                Vector3.Distance(obstaclePosition, ahead2) <= obstacleRadius;
    }

	private Vector3 FollowLeader()
	{
		AISteeringAgent steering = pursuitOffsetLeader.GetComponentInChildren<AISteeringAgent>();
		if (steering == null)
			return Vector3.zero;

		const float behindLeader = 1.0f;


		Vector3 ahead = steering.transform.position + (steering.heading * behindLeader);

		Vector3 behind = steering.transform.position + (steering.heading * -behindLeader);

		const float leaderSightRadius = 1.0f;

		Vector3 followForce = Vector3.zero;

		if (Vector3.Distance(pursuitOffsetLeader.transform.position, position) <= leaderSightRadius ||
			Vector3.Distance(ahead, position) <= leaderSightRadius)
		{
			followForce += Evade(pursuitOffsetLeader.gameObject);
		}

		followForce += Arrive(pursuitOffsetLeader.gameObject, 10.0f);

		followForce += Separate(Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies);

		return followForce;
	}

	private Vector3 PathFollow()
	{
		if (path == null)
			return Vector3.zero;

		Vector3 targetNode = path.Nodes[currentPathNode];

		if (Vector3.Distance(position, targetNode) <= path.nodeRadius)
		{
			currentPathNode++;

			if (currentPathNode >= path.Nodes.Count)
			{
				if (currentPathNode == path.Nodes.Count)
				{
					if(OnPathCompleted != null)
					{
						OnPathCompleted.Invoke();
					}
				}

				currentPathNode = path.Nodes.Count - 1;
			}
		}

		return Seek(targetNode);
	}

	private Vector3 Flock(List<Character> flockers)
	{
		Vector3 separation = Separate(flockers);
		Vector3 alignment = Allignment(flockers);
		Vector3 cohesion = Cohesion(flockers);

		// Alter weights
		separation *= maxSpeed * 0.5f;
		alignment *= maxSpeed * 0.5f;
		cohesion *= maxSpeed * 0.5f;

		Vector3 flockForce = separation;
		flockForce += alignment;
		flockForce += cohesion;

		return flockForce;
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

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (!gameObject.activeInHierarchy)
		{
			return;
		}

		Color blue = new Color(0.0f, 0.0f, 1.0f, 0.75f);
		Color yellow = new Color(1.0f, 1.0f, 0.0f, 0.35f);

		posLastFrame = transform.position;

		Vector3 PosWithY = transform.position;
		PosWithY.y = 0.5f;

		Debug.DrawLine(PosWithY, PosWithY + velocity.normalized * 2.0f, yellow);
		Debug.DrawLine(PosWithY, PosWithY + heading * 2.0f, blue);

	}
#endif
}
