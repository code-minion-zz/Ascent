using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICondition_Sensor : AICondition 
{
    protected AISensor[] sensors;
    protected AIMindAgent agent;
    protected Transform transform;

	private int iSensed;

    public AICondition_Sensor(Transform transform, AIMindAgent agent, params AISensor[] sensors)
    {
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
                return true;
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
