using UnityEngine;
using System.Collections;

public class PDefenceBuff : Buff
{
    protected float defenceBonusPercent = 0.25f;

    public override void ApplyBuff(Character caster, Character target, float duration)
    {
        base.ApplyBuff(caster, target, duration);

        target.AddBuff(this);
    }

    protected override void ProcessBuff()
    {
        // Don't need to do anything here...
    }

    protected override void EndBuff()
    {
        // Remove the buff from the owner
    }
}
