using UnityEngine;
using System.Collections;

public class SilenceDebuff : StatusEffect
{
	public SilenceDebuff()
    {
		type = EEffectType.Debuff;
    }

	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		overridePrevious = true;

		if (target.IsVulnerableTo(EStatus.Silence) || caster == target)
		{
			base.ApplyStatusEffect(caster, target, duration);

			target.Status |= EStatus.Silence;

			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Silenced!", Color.yellow);
		}
		else
		{
			ProcessImmuneEffect(target);
		}
	}

	protected override void EndEffect()
	{
		target.Status &= ~EStatus.Silence;
	}
}
