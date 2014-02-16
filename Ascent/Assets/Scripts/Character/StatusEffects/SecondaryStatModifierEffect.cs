using UnityEngine;
using System.Collections;

public class SecondaryStatModifierEffect : StatusEffect 
{
	protected EApplyMethod buffType;
	public EApplyMethod BuffType
	{
		get { return buffType; }
		set { buffType = value; }
	}

	protected float buffValue;
	public float BuffValue
	{
		get { return buffValue; }
		set { buffValue = value; }
	}

	public EStats statType;
	protected EStats StatType
	{
		get { return statType; }
		set { statType = value; }
	}

	protected SecondaryStats stats;
	public SecondaryStats SecondaryStats
	{
		get { return stats; }
	}

	public float Attack
	{
		get { return stats.attack; }
	}

	public float CriticalHitChance
	{
		get { return stats.criticalHitChance; }
	}

	public float MDefense
	{
		get { return stats.magicalDefense; }
	}

	public float PDefense
	{
		get { return stats.physicalDefense; }
	}

	public virtual void ApplyStatusEffect(Character caster, Character target, EApplyMethod buffType, float buffValue, float duration)
	{
		this.buffType = buffType;
		this.buffValue = buffValue;

		base.ApplyStatusEffect(caster, target, duration);

		target.ApplyStatusEffect(this);
	}


	public void AddBuff(float initialValue, ref float statValue)
	{
		float sign = 1.0f;
		if(type == EEffectType.Debuff)
		{
			sign *= -1.0f;
		}

		if (buffType == EApplyMethod.Fixed)
		{
			statValue += buffValue * sign;
		}
		else // Add percentage gain
		{
			statValue += ((initialValue * buffValue) * sign);
		}
	}
}
