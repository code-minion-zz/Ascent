using UnityEngine;
using System.Collections;

public class HealthRegenBuff : Buff 
{
    private Buff.EBuffType buffType;
    private int ticks;
    private float totalAmount;
    private bool[] tickedOver;

    private float timeAccum;
    private float timePerTick;

    public virtual void ApplyStatusEffect(Character caster, Character target, EBuffType buffType, int ticks, float totalAmount, float duration)
    {
        this.ticks = ticks;
        this.totalAmount = totalAmount;
        this.buffType = buffType;
  
        base.ApplyStatusEffect(caster, target, duration);

        timePerTick = duration / ticks;
        tickedOver = new bool[(int)(duration / ticks)];

        target.AddStatusEffect(this);
    }

    protected override void ProcessEffect()
    {
        timeAccum += Time.deltaTime;
        if (timeAccum >= timePerTick)
        {
            timeAccum -= timePerTick;
            target.Stats.CurrentHealth += (int)Mathf.Max((totalAmount / (float)ticks), 1.0f);
        }
    }
}
