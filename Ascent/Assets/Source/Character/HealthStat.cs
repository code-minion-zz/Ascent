using UnityEngine;
using System.Collections;
using System;

public class HealthStat
{
    #region Fields

    private float min;
    private float max;

    #endregion

    #region Properties

    public float Min
    {
        get { return min; }
        set { min = value; }
    }

    public float Max
    {
        get { return max; }
        set { max = value; }
    }

    #endregion

    #region Intialization

    public HealthStat()
    {

    }

    public HealthStat(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

    #endregion

    #region Operations

    public void Set(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

    // Stat vs Stat
    public static HealthStat operator +(HealthStat _healthLeft, HealthStat _healthRight)
    {
        _healthLeft.min += _healthRight.min;
        return _healthLeft;
    }

    public static HealthStat operator -(HealthStat _healthLeft, HealthStat _healthRight)
    {
        _healthLeft.min += _healthRight.min;
        return _healthLeft;
    }

    // Stat vs Float
    public static HealthStat operator +(HealthStat _healthLeft, float _healthRight)
    {
        _healthLeft.min += _healthRight;
        return _healthLeft;
    }

    public static HealthStat operator -(HealthStat _healthLeft, float _healthRight)
    {
        _healthLeft.min -= _healthRight;
        return _healthLeft;
    }

    public static bool operator <=(HealthStat _healthLeft, float _healthRight)
    {
        return (_healthLeft.min <= _healthRight);
    }

    public static bool operator >=(HealthStat _healthLeft, float _healthRight)
    {
        return (_healthLeft.min >= _healthRight);
    }

    public override string ToString()
    {
        return ("HealthStat" + ": " + min + " / " +  max);
    }

    #endregion
}
