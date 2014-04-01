using UnityEngine;
using System.Collections;

public class ShockedDebuff : TicksOverTimeEffect
{
	public ShockedDebuff()
    {
		type = EEffectType.Debuff;
    }

	protected override void Tick()
	{
		target.HitTaken = true;
	}

	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		overridePrevious = true;

		if (target.IsVulnerableTo(EStatus.Shock) || caster == target)
		{
			base.ApplyStatusEffect(caster, target, duration);

			target.Status |= EStatus.Shock;

			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Shocked!", Color.yellow);
		}
		else
		{
			ProcessImmuneEffect(target);
		}
	}

	protected override void EndEffect()
	{
		target.Status &= ~EStatus.Shock;
	}
}
