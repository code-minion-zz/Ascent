using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICondition_Sensor : AICondition 
{
    protected AISensor[] sensors;
    protected AIMindAgent agent;
    protected Transform transform;

    public AICondition_Sensor(Transform transform, AIMindAgent agent, params AISensor[] sensors)
    {
        this.transform = transform;
        this.sensors = sensors;
        this.agent = agent;
    }

    public override bool HasBeenMet()
    {
        List<Character> sensed = agent.SensedCharacters;
        foreach (AISensor s in sensors)
        {
            if (s.Sense(transform, ref sensed))
            {
                return true;
            }
        }

        return false;
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
