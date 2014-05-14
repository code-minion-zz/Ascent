using UnityEngine;
using System.Collections;
using System;

public class StunnedDebuff : StatusEffect
{
	private StunnedEffect effect;

	public StunnedDebuff()
	{
		type = EEffectType.Debuff;
	}

	public StunnedDebuff(Character caster, Character target, float duration)
	{
		type = EEffectType.Debuff;

        //if (duration > 0.0f)
        //{
			timed = true;
			this.duration = duration;
		//}
	}

	public override void ApplyStatusEffect(Character caster, Character target)
	{
		overridePrevious = true;

		if (target.IsVulnerableTo(EStatus.Stun) || caster == target)
		{
			if (!target.IsInState(EStatus.Stun))
			{
				Transform stunTarget = target.stunEffectPosition;
				if (stunTarget == null)
				{
					stunTarget = target.transform.FindChild("Stun");

					if (stunTarget == null)
					{
						stunTarget = target.transform;
					}
				}
				effect = EffectFactory.Singleton.CreateStunnedEffect(stunTarget).GetComponent<StunnedEffect>();
			}

			base.ApplyStatusEffect(caster, target);

			target.Status |= EStatus.Stun;
			target.StatusColour |= EStatusColour.Yellow;

			target.Motor.StopMotion();
			target.Motor.StopMovingAlongGrid();	
		}
		else
		{
            ProcessImmuneEffect(target);
		}
	}

	protected override void EndEffect()
	{
		if (effect != null)
		{
			target.Status &= ~EStatus.Stun;
			target.StatusColour &= ~EStatusColour.Yellow;

			effect.FadeOutAndDie();
		}
	}
}
