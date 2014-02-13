using UnityEngine;
using System.Collections;

public class BaseStatBuff : Buff
{
    public EStats type;
    protected PrimaryStats stats;

    public float Power
    {
        get { return stats.power; }
    }

    public float Finesse
    {
        get { return stats.finesse; }
    }

    public float Vitality
    {
        get { return stats.finesse; }
    }

    public float Spirit
    {
        get { return stats.spirit; }
    }

    public PrimaryStats PrimaryStats
    {
        get { return stats; }
    }

    public void AddBuff(float initialValue, ref float statValue)
    {
        statValue += stats.GetStat(type);
    }
}