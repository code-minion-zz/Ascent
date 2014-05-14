using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
			base.ApplyStatusEffect(caster, target);

			bool wasApplied = false;
			// Hack to make sure that effect was applied
			List<StatusEffect> statusEffects = target.StatusEffects;

			for (int i = 0; i < statusEffects.Count; ++i)
			{
				if (statusEffects[i].GetType() == this.GetType())
				{
					wasApplied = true;
					break;
				}
			}
			if (!wasApplied)
			{
				return;
			}

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
				effect = EffectFactory.Singleton.CreateStunnedEffect(stunTarget, target).GetComponent<StunnedEffect>();
			}


			target.Status |= EStatus.Stun;
			//target.StatusColour |= EStatusColour.Yellow;

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
			effect.FadeOutAndDie();
		}
			target.Status &= ~EStatus.Stun;
			//target.StatusColour &= ~EStatusColour.Yellow;
	}
}
