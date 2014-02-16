using UnityEngine;
using System.Collections;

public class SecondaryStatItemProperty : ItemProperty
{
	protected EStats statType;
	public EStats StatType
	{
		get { return statType; }
		set { statType = value; }
	}

	protected float buffValue;
	public float BuffValue
	{
		get { return buffValue; }
		set { buffValue = value; }
	}

	protected StatusEffect.EApplyMethod buffType;
	public StatusEffect.EApplyMethod BuffType
	{
		get { return buffType; }
		set { buffType = value; }
	}

	protected SecondaryStats stats;
	public SecondaryStats SecondaryStats
	{
		get { return stats; }
	}

	protected StatusEffect.EEffectType type;
	public StatusEffect.EEffectType Type
	{
		get { return type; }
		set { type = value; }
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

	public void AddBuff(float initialValue, ref float statValue)
	{
		float sign = 1.0f;
		if (type ==  StatusEffect.EEffectType.Debuff)
		{
			sign *= -1.0f;
		}

		if (buffType == StatusEffect.EApplyMethod.Fixed)
		{
			statValue += buffValue * sign;
		}
		else // Add percentage gain
		{
			statValue += ((initialValue * buffValue) * sign);
		}
	}


    public override void Initialise() { }
    public override void CheckCondition() { }
    public override void DoAction() { }
}
