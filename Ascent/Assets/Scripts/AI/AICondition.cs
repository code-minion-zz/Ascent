using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIConditionType
{
    ActionCooldown,
    ActionEnd,
    Attacked,
    HP,
    ReachedTarget,
    Sensor,
    SP,
    SurroundedSensor,
    ConditionTimer
}

[System.Serializable]
public class AIConditionSetting
{
    public AIConditionType conditionType;
}

[System.Serializable]
public class AICondition 
{
    public enum ESign
    {
        GreaterThan,
        LessThan,
        Equal,
        EqualOrGreater,
        EqualOrLess
    }

    public enum EType
    {
        Percentage,
        Fixed,
    }

    public virtual void Reset()
    {

    }

    public virtual bool HasBeenMet()
    {
        return false;
    }

#if UNITY_EDITOR
    public virtual void DebugDraw()
    {

    }

	public override string ToString()
	{
		return base.ToString();
	}
#endif
}
