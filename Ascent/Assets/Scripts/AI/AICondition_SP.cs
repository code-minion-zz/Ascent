using UnityEngine;
using System.Collections;

public class AICondition_SP : AICondition
{
    private DerivedStats stats;
    private EType type;
    private ESign sign;
    private float value;

    public AICondition_SP(DerivedStats stats, EType type, ESign sign, float value)
    {
        this.stats = stats;
        this.type = type;
        this.sign = sign;
        this.value = value;
    }

    public override bool HasBeenMet()
    {
        float curValue = stats.CurrentSpecial;

        if (type == EType.Percentage)
        {
            curValue = curValue / stats.MaxSpecial;
        }

        if (Evaluate(curValue, value, sign))
        {
            return true;
        }

        return false;
    }

    protected bool Evaluate(float a, float b, ESign sign)
    {
        switch (sign)
        {
            case ESign.Equal: return a == b;
            case ESign.GreaterThan: return a > b;
            case ESign.EqualOrGreater: return a >= b;
            case ESign.LessThan: return a < b;
            case ESign.EqualOrLess: return a <= b;
        }

        return false;
    }
}
