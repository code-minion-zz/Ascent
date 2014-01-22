using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AISteeringAgent  
{
    private Vector3 startPos;
    public Vector3 StartPosition
    {
        get { return startPos; }
        set { startPos = value; }
    }

    private Vector3 targetPos;

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
        if (hasTarget)
        {
            if (targetCharacter != null)
            {
				if (motor.UsingMovementForce)
				{
                    if (IsRunningAway)
                    {
                        motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(motor.transform.position - targetCharacter.transform.position, Vector3.up), rotationSpeed);
                    }
                    else
                    {
                        //motor.transform.LookAt(targetCharacter.transform.position);
                        motor.transform.rotation = Quaternion.RotateTowards(motor.transform.rotation, Quaternion.LookRotation(targetCharacter.transform.position - motor.transform.position, Vector3.up), rotationSpeed);
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
				if (motor.UsingMovementForce)
				{
					//motor.transform.LookAt(targetPos);
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

            motor.Move(motor.transform.forward);
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

                    Debug.DrawLine(new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), new Vector3(pos.x, 0.2f, pos.z), red, 0.01f);
                    Debug.DrawLine(new Vector3(posLastFrame.x, 0.2f, posLastFrame.z), new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), green, 0.5f);
                }
                else
                {
                    Debug.DrawLine(new Vector3(startPos.x, 0.2f, startPos.z), new Vector3(pos.x, 0.2f, pos.z), red);
                    Debug.DrawLine(new Vector3(startPos.x, 0.2f, startPos.z), new Vector3(motor.transform.position.x, 0.2f, motor.transform.position.z), green, 0.25f);
                }

                posLastFrame = motor.transform.position;
            }

            Debug.DrawLine(motor.transform.position, motor.transform.position + motor.TargetVelocity * 1.5f, yellow);
            Debug.DrawLine(motor.transform.position, motor.transform.position + motor.transform.forward * 1.5f, blue);

        }
    }
#endif
}
