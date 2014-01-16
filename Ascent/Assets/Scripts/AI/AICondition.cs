using UnityEngine;
using System.Collections;

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
#endif
}
