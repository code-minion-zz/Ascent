using UnityEngine;
using System.Collections;

public class SleepingDebuff : StatusEffect
{
	public SleepingDebuff(Character caster, Character target, float duration)
	{
		type = EEffectType.Debuff;
		ApplyStatusEffect(caster, target, duration);
	}

	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		overridePrevious = true;

		base.ApplyStatusEffect(caster, target, duration);

		if (target.IsVulnerableTo(EStatus.Frozen) || caster == target)
		{
			target.Status |= EStatus.Sleep;
			target.StatusColour |= EStatusColour.Blue;

			target.Motor.StopMotion();
			target.Motor.StopMovingAlongGrid();

			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Sleeping!", Color.blue);
		}
	}

	protected override void EndEffect()
	{
		target.Status &= ~EStatus.Sleep;
		target.StatusColour &= ~EStatusColour.Blue;
	}
}
