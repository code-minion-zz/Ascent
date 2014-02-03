using UnityEngine;
using System.Collections;

public abstract class CharacterStats  
{
	protected int level;
	protected int currentHealth;
	protected int currentSpecial;

	#region DONOTTOUCH
	/// <summary>
	/// Seriously, don't touch these.
	/// These stats should not be modified, they are calculated according to level.
	/// </summary>
	protected PrimaryStats primaryStats;
	protected PrimaryStatsGrowthRates primaryStatsGrowth;

	protected SecondaryStats secondaryStats;
	protected SecondaryStatsGrowthRates secondaryStatsGrowth;

	public delegate void CharacterStatisticEventHandler(float newMin);
	#endregion DONOTTOUCH

#pragma warning disable 0067
	public event CharacterStatisticEventHandler onMaxHealthChanged;
	public event CharacterStatisticEventHandler onCurHealthChanged;

	public event CharacterStatisticEventHandler onMaxSpecialChanged;
	public event CharacterStatisticEventHandler onCurSpecialChanged;

	public event CharacterStatisticEventHandler onExpChanged;
#pragma warning restore 0067

	public void Reset()
	{
		currentHealth = MaxHealth;
		currentSpecial = MaxSpecial;
	}


#region PrimaryStats

	public virtual int Level
	{
		get { return level; }
		set { level = value; }
	}

	public virtual int Power
	{
		// BasePOW + ((MaxPOW - BasePOW) * ((Level - 1) / MaxLevel))
		get { return (int)(primaryStatsGrowth.minPower + ((primaryStatsGrowth.maxPower -  primaryStatsGrowth.minPower) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
	}

	public virtual int Finesse
	{
		// BaseFIN + ((MaxFIN - BaseFIN) * ((Level - 1) / MaxLevel))
		get { return (int)(primaryStatsGrowth.minFinesse + ((primaryStatsGrowth.maxFinesse - primaryStatsGrowth.minFinesse) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
	}

	public virtual int Vitality
	{
		// BaseVIT + ((MaxVIT - BaseVIT) * ((Level - 1) / MaxLevel))
		get { return (int)(primaryStatsGrowth.minVitality + ((primaryStatsGrowth.maxVitality - primaryStatsGrowth.minVitality) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
	}

	public virtual int Spirit
	{
		// BaseSPR + ((MaxSPR - BaseSPR) * ((Level - 1) / MaxLevel))
		get { return (int)(primaryStatsGrowth.minSpirit + ((primaryStatsGrowth.maxSpirit - primaryStatsGrowth.minSpirit) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
	}

#endregion

#region SecondaryStats

	public virtual int MaxHealth
	{
		// Base HP + (VIT * HP per VIT)
		get
		{ return (int)(secondaryStats.health + (primaryStats.vitality * secondaryStatsGrowth.healthPerVit)); }
	}

	public virtual int CurrentHealth
	{
		get { return currentHealth; }
		set 
		{
			if (currentHealth != value)
			{
				if (onCurHealthChanged != null)
				{
					onCurHealthChanged.Invoke(value);
				}
			}

			currentHealth = value;
		}
	}

	public virtual int MaxSpecial
	{
		// Base SP + (SPR * SP per SPR)
		get { return (int)(secondaryStats.special + (primaryStats.spirit * secondaryStatsGrowth.specialPerSpirit)); }
	}

	public virtual int CurrentSpecial
	{
		get { return currentSpecial; }
		set
		{
			if (currentSpecial != value)
			{
				if (onCurSpecialChanged != null)
				{
					onCurSpecialChanged.Invoke(value);
				}
			}

			currentSpecial = value;
		}
	}

	public virtual int Attack
	{
		get { return (int)(secondaryStats.attack + (secondaryStatsGrowth.attackPerPow * (float)Power)); }
	}

	public virtual int PhysicalDefense
	{
		get { return (int)(secondaryStats.physicalDefense + (secondaryStatsGrowth.physicalDefPerVit * (float)Vitality)); }
	}

	public virtual int MagicalDefense
	{
		get { return (int)(secondaryStats.magicalDefense + (secondaryStatsGrowth.magicalDefPerSpr * (float)Spirit)); }
	}

	public virtual float CriticalHitChance
	{
		// BaseCritChance + (CritChancePerFIN * FIN)
		get { return secondaryStats.criticalHitChance + (secondaryStatsGrowth.critPerFin * (float)Finesse); }
	}

	public virtual float CritalHitMultiplier
	{
		// BaseCritMutlipler + (CritMultiplierPerFIN * FIN)
		get { return secondaryStats.criticalHitChance + (secondaryStatsGrowth.critMultPerFin * (float)Finesse); }
	}

	public virtual float DodgeChance
	{
		// BaseDodgeChance + (DodgeChancePerFIN * FIN)
		get { return secondaryStats.dodgeChance + (secondaryStatsGrowth.dodgePerFin * (float)Finesse); }
	}

#endregion

}
