using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISteeringAgent  
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private Character targetCharacter = null;
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

    protected bool hasTarget = false;

    public delegate void TargetReached();
    public event TargetReached OnTargetReached;

    public void Initialise(CharacterMotor motor)
    {
        this.motor = motor;
    }

    public void Process()
    {
        if (hasTarget)
        {
            if (targetCharacter != null)
            {
                motor.Move(motor.transform.forward);
                motor.transform.LookAt(targetCharacter.transform.position);

                if (MathUtility.IsWithinCircle(motor.transform.position, targetCharacter.transform.position, closeEnoughRange))
                {
                    if (OnTargetReached != null)
                    {
                        OnTargetReached.Invoke();
                    }
                    hasTarget = false;
                    targetCharacter = null;
                }
            }
            else
            {
                motor.Move(motor.transform.forward);
                motor.transform.LookAt(targetPos);

                if (MathUtility.IsWithinCircle(motor.transform.position, targetPos, closeEnoughRange))
                {
                    if (OnTargetReached != null)
                    {
                        OnTargetReached.Invoke();
                    }
                    hasTarget = false;
                }
            }
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        startPos = motor.transform.position;
        targetPos = targetPosition;
        hasTarget = true;
    }

    public void SetTarget(Character character)
    {
        targetCharacter = character;
        startPos = motor.transform.position;
        hasTarget = true;
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
        if(motor != null)
        {
            if(hasTarget)
            {
                Vector3 pos = targetPos;
                if(targetCharacter != null)
                {
                    pos = targetCharacter.transform.position;
                }

                Debug.DrawLine(new Vector3(startPos.x, 1.0f, startPos.z), new Vector3(pos.x, 1.0f, pos.z), Color.red);
                Debug.DrawLine(new Vector3(startPos.x, 1.0f, startPos.z), new Vector3(motor.transform.position.x, 1.0f, motor.transform.position.z), Color.green);
            }
        }
    }
#endif
}
