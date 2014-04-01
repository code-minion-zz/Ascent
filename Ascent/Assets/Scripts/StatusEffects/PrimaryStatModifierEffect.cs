using UnityEngine;
using System.Collections;

public class PrimaryStatModifierEffect : StatusEffect 
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
	public EStats StatType
	{
		get { return statType; }
		set { statType = value; }
	}

	protected PrimaryStats stats;
	public PrimaryStats PrimaryStats
	{
		get { return stats; }
	}

	public float Power
	{
		get { return stats.power; }
	}

	public float Finesse
	{
		get { return stats.finesse; }
	}

	public float Vitality
	{
		get { return stats.finesse; }
	}

	public float Spirit
	{
		get { return stats.spirit; }
	}


	public void AddBuff(float initialValue, ref float statValue)
	{
		float sign = 1.0f;
		if (type == EEffectType.Debuff)
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
