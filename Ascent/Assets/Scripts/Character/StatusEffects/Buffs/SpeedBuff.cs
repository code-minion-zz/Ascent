using UnityEngine;
using System.Collections;

public class SpeedBuff : SecondaryStatModifierEffect
{
	public SpeedBuff()
    {
        statType = EStats.MovementSpeed;
    }

	public override void ApplyStatusEffect(Character caster, Character target, EApplyMethod buffType, float buffValue, float duration)
	{
		base.ApplyStatusEffect(caster, target, buffType, buffValue, duration);

		// Up the speed of the target
		if(buffType == EApplyMethod.Fixed)
		{
			target.Motor.BuffBonusSpeed += buffValue;
		}
		else // buffType == EBuffType.Percentage
		{
			target.Motor.BuffBonusSpeed += target.Motor.OriginalSpeed * buffValue;
		}
	}

	protected override void EndEffect()
	{
		// Down the speed of the target
		if (buffType == EApplyMethod.Fixed)
		{
			target.Motor.BuffBonusSpeed -= buffValue;
		}
		else // buffType == EBuffType.Percentage
		{
			target.Motor.BuffBonusSpeed -= target.Motor.OriginalSpeed * buffValue;
		}

		base.EndEffect();
	}
}