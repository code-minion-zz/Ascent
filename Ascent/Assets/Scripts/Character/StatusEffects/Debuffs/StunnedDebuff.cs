using UnityEngine;
using System.Collections;
using System;

public class StunnedDebuff : StatusEffect
{
	public StunnedDebuff()
	{

	}

	public StunnedDebuff(Character caster, Character target, float duration)
	{
		ApplyStatusEffect(caster, target, duration);
	}

	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		base.ApplyStatusEffect(caster, target, duration);

		if (target.IsVulnerableTo(EStatus.Stun) || caster == target)
		{
			target.Status |= EStatus.Stun;

			target.SetColor(Color.yellow);
			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Stunned!", Color.yellow);
		}
	}

	protected override void EndEffect()
	{
		target.Status &= ~EStatus.Stun;

		target.ResetColor();
	}
}
