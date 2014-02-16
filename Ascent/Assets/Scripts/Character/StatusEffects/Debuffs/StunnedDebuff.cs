using UnityEngine;
using System.Collections;
using System;

public class StunnedDebuff : StatusEffect
{
	public StunnedDebuff()
	{
		type = EEffectType.Debuff;
	}

	public StunnedDebuff(Character caster, Character target, float duration)
	{
		ApplyStatusEffect(caster, target, duration);
	}

	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		overridePrevious = true;

		if (target.IsVulnerableTo(EStatus.Stun) || caster == target)
		{
			base.ApplyStatusEffect(caster, target, duration);

			target.Status |= EStatus.Stun;
			target.StatusColour |= EStatusColour.Yellow;

			target.Motor.StopMotion();
			target.Motor.StopMovingAlongGrid();


			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Stunned!", Color.yellow);
		}
		else
		{
			ProcessImmuneEffect();
		}
	}

	protected override void EndEffect()
	{
		target.Status &= ~EStatus.Stun;
		target.StatusColour &= ~EStatusColour.Yellow;
	}
}
