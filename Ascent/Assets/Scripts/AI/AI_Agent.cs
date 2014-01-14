using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Agent : MonoBehaviour 
{
    public Character actor;
    protected List<AI_Behaviour> behaviours = new List<AI_Behaviour>();
    private Room containedRoom;

	AI_State curState = AI_State.DELAY;
    float timeAccum = 0.0f;

    protected List<AI_Sensor> sensors = new List<AI_Sensor>();

    protected enum AI_State
    {
        IDLE = 0,
        WANDER,
        ATTACK,
        CHARGE,

	    NEXT,
		DELAY,
        MAX,

    }

    protected enum AI_Sense_Type
    {
        ALL,
        ALLIES,
        ENEMIES,
    }

    private Vector3 curPos;
    private Vector3 targetPos;

    private Quaternion curRot;
    private Quaternion targetRot;

    private Character targetChar;

    private float[] stateTimes = new float[(int)AI_State.MAX] { 0.25f, 2.25f, 1.5f, 0.0f, 0.0f, 5.0f };


    public void Awake()
    {
        if(actor == null)
        {
            Debug.LogError("This agent does not have an actor to manipulate.", this);
        }
    }

    public void Start()
    {
        AI_Sensor[] attachedSensors = gameObject.GetComponents<AI_Sensor>() as AI_Sensor[];

        foreach (AI_Sensor sense in attachedSensors)
        {
            sensors.Add(sense);
        }

		stateTimes[(int)curState] = Random.Range(0.0f, 1.5f);
    }

    public void OnEnable()
    {
        containedRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

    }

    public void Process()
    {
		if (actor.IsDead)
		{
			return;
		}
        //foreach(AI_Behaviour b in behaviours)
        //{
        //    // Also check for exit or continue conditions....
        //    b.Process();
        //}

        timeAccum += Time.deltaTime;
        if (timeAccum > stateTimes[(int)curState])
        {
            timeAccum = stateTimes[(int)curState];
        }

        switch(curState)
        {
			case AI_State.DELAY:
				{
					if (timeAccum >= stateTimes[(int)curState])
					{
						ChangeState(AI_State.IDLE);
					}
				}
				break;
            case AI_State.IDLE:
                {
                    if(timeAccum >= stateTimes[(int)curState])
                    {
                        ChangeState(AI_State.NEXT);

                        curPos = actor.transform.position;
                        SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(curPos, 7.5f));
                    }
                }
                break;
            case AI_State.WANDER:
                {
                    MoveTowardTarget();

					//List<Character> sensedCharacters = Sense(AI_Sense_Type.ENEMIES);
					//if (sensedCharacters.Count > 0)
					//{
					//    targetChar = GetClosestSensedCharacter(ref sensedCharacters);

					//    if (targetChar != null)
					//    {
					//        SetTargetPosition(targetChar.transform.position);

					//        ChangeState(AI_State.NEXT);
					//    }
					//}

					if (timeAccum >= stateTimes[(int)curState])
					{
						ChangeState(AI_State.IDLE);
					}
                }
                break;
            case AI_State.ATTACK:
                {
					//if (timeAccum >= stateTimes[(int)curState] * 0.5f)
					//{
					//    SetTargetAttackPosition(targetChar.transform.position);
					//}
					//else if(timeAccum >= stateTimes[(int)curState] * 0.75f)
					//{
					//    SetTargetAttackPosition(targetChar.transform.position);
					//}
                    MoveTowardTarget();

                    if (timeAccum >= stateTimes[(int)curState])
                    {
                        ChangeState(AI_State.IDLE);
                    }
                }
                break;
            case AI_State.CHARGE:
                {
                    if (timeAccum >= stateTimes[(int)curState])
                    {
                        ChangeState(AI_State.NEXT);
                    }
                }
                break;
        }


		Debug.DrawLine(new Vector3(curPos.x, 1.0f, curPos.z), new Vector3(targetPos.x, 1.0f, targetPos.z), Color.red);
		Debug.DrawLine(new Vector3(curPos.x, 1.0f, curPos.z), new Vector3(actor.transform.position.x, 1.0f, actor.transform.position.z), Color.green);
    }

    protected List<Character> Sense(AI_Sense_Type senseType)
    {
        List<Character> sensedCharacters = new List<Character>();

        switch (senseType)
        {
            case AI_Sense_Type.ALL:
                {
                    foreach (AI_Sensor sensor in sensors)
                    {
						if (sensor.enabled)
						{
							sensor.SenseAll(ref sensedCharacters);
						}
                    }
                }
            break;
            case AI_Sense_Type.ALLIES:
                {
                    foreach (AI_Sensor sensor in sensors)
					{
						if (sensor.enabled)
						{
							sensor.SenseAllies(ref sensedCharacters);
						}
                    }
                }
            break;
            case AI_Sense_Type.ENEMIES:
                {
                    foreach (AI_Sensor sensor in sensors)
                    {
						if (sensor.enabled)
						{
							sensor.SenseEnemies(ref sensedCharacters);
						}
                    }
                }
            break;
        }

        return sensedCharacters;
    }

    public Character GetClosestSensedCharacter(ref List<Character> sensedCharacters)
    {
        Character closestCharacter = null;
        float closestDistance = 1000000.0f;

        foreach (Character c in sensedCharacters)
        {
            float distance = (actor.transform.position - c.transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCharacter = c;
            }
        }

        return closestCharacter;
    }

    protected void ChangeState(AI_State state)
    {
        if (state == AI_State.NEXT)
        {
            ++curState;

            if (curState == AI_State.MAX)
            {
                curState = 0;
            }
        }
        else
        {
            curState = state;
        }

        timeAccum = 0.0f;
    }


    public void SetTargetPosition(Vector3 targetPosition)
    {
        curPos = transform.position;
        targetPos = targetPosition;
		targetPos.y = transform.position.y;

       // curRot = transform.rotation;
        //targetRot = Quaternion.LookRotation(targetPos - curPos, Vector3.up);
		//actor.transform.rotation = targetRot;

	
    }

	public void SetTargetAttackPosition(Vector3 targetPosition)
	{
		curRot = transform.rotation;
		targetRot = Quaternion.LookRotation(targetPos - curPos, Vector3.up);
	}

    public void MoveTowardTarget()
    {
        float doubleTime = timeAccum * 2.0f;

		//actor.transform.rotation = Quaternion.Slerp(curRot, targetRot, doubleTime / (stateTimes[(int)curState] * 0.5f));

		
		//if(curState == AI_State.ATTACK)
		//{
		//    actor.rigidbody.AddForce(actor.transform.forward * 150.0f);
		//}
		//else
		//{
		//    actor.transform.position = Vector3.Lerp(curPos, targetPos, timeAccum / stateTimes[(int)curState]);
		//}


		actor.Motor.Move(actor.transform.forward);
		//actor.GetComponent<CharacterController>().Move(actor.transform.forward * 3.0f * Time.deltaTime);
		actor.transform.LookAt(targetPos);

		if(MathUtility.IsWithinCircle(actor.transform.position, targetPos, 0.25f))
		{
			ChangeState(AI_State.IDLE);
		}
    }
}
