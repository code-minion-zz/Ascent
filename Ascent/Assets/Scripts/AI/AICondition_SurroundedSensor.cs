using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICondition_SurroundedSensor : AICondition
{
    protected AISensor[] sensors;
    protected AIMindAgent agent;
    protected Transform transform;

    protected int numberRequriedToBeSurrounded;

    private int iSensed;

    public AICondition_SurroundedSensor(Transform transform, AIMindAgent agent, int numberRequriedToBeSurrounded, params AISensor[] sensors)
    {
        this.numberRequriedToBeSurrounded = numberRequriedToBeSurrounded;
        this.transform = transform;
        this.sensors = sensors;
        this.agent = agent;
    }

    public override bool HasBeenMet()
    {
        List<Character> sensed = agent.SensedCharacters;
        sensed = new List<Character>();
        foreach (AISensor s in sensors)
        {
            if (s.Sense(transform, ref sensed))
            {
                iSensed = sensed.Count;
                agent.SensedCharacters = sensed;

				if (iSensed >= numberRequriedToBeSurrounded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        iSensed = 0;
        return false;
    }

    public override string ToString()
    {
        return "Sensor: " + iSensed;
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        foreach (AISensor s in sensors)
        {
            s.DebugDraw();
        }
    }
#endif
}
