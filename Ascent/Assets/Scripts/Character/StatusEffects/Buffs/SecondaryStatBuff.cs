using UnityEngine;
using System.Collections;

public class SecondaryStatBuff : Buff
{
    protected EBuffType buffType;
    public EBuffType BuffType
    {
        get { return buffType; }
        set { buffType = value; }
    }

    protected float buffValue;
    public float BuffValue
    {
        get { return buffValue; }
        set { buffValue = value; }
    }


    public EStats statType;
    protected EStats StatType
    {
        get { return statType; }
        set { statType = value; }
    }

    protected SecondaryStats stats;
    public SecondaryStats SecondaryStats
    {
        get { return stats; }
    }

    public float Attack
    {
        get { return stats.attack; }
    }

    public float CriticalHitChance
    {
        get { return stats.criticalHitChance; }
    }

    public float MDefense
    {
        get { return stats.magicalDefense; }
    }

    public float PDefense
    {
        get { return stats.physicalDefense; }
    }


    public virtual void ApplyStatusEffect(Character caster, Character target, EBuffType buffType, float buffValue, float duration)
    {
        this.buffType = buffType;
        this.buffValue = buffValue;

        base.ApplyStatusEffect(caster, target, duration);

        target.AddStatusEffect(this);
    }


    public void AddBuff(float initialValue, ref float statValue)
    {
        if (buffType == EBuffType.Fixed)
        {
            statValue += buffValue;
        }
        else // Add percentage gain
        {
            statValue += ((initialValue * buffValue));
        }
    }
}
