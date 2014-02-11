using UnityEngine;
using System.Collections;

public class AttackBuff : SecondaryStatBuff
{
    public override void ApplyStatusEffect(Character caster, Character target, float duration)
    {
        base.ApplyStatusEffect(caster, target, duration);

        target.AddStatusEffect(this);
    }
}
