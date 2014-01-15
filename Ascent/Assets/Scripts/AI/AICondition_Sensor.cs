using UnityEngine;
using System.Collections;

public class AICondition_Sensor : AICondition 
{
    AISensor[] sensors;

    public AICondition_Sensor(params AISensor[] sensors)
    {
        this.sensors = sensors;
    }

    public override bool HasBeenMet()
    {
        foreach (AISensor s in sensors)
        {
            if (s.Sense())
            {
                return true;
            }
        }

        return false;
    }
}
