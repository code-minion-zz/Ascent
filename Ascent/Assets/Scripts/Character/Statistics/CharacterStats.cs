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
        get { return BasePower; }
	}

	public virtual int Finesse
	{
        get { return BaseFinesse;}
	}

	public virtual int Vitality
	{
        get { return BaseVitality; }
	}

	public virtual int Spirit
	{
        get {  return BaseSpirit; }
	}

#endregion

    #region BasePrimary

    public int BasePower
    {	
        // BasePOW + ((MaxPOW - BasePOW) * ((Level - 1) / MaxLevel))
        get { return (int)(primaryStatsGrowth.minPower + ((primaryStatsGrowth.maxPower - primaryStatsGrowth.minPower) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
    }

    public int BaseFinesse
    {
        // BaseFIN + ((MaxFIN - BaseFIN) * ((Level - 1) / MaxLevel))
        get { return (int)(primaryStatsGrowth.minFinesse + ((primaryStatsGrowth.maxFinesse - primaryStatsGrowth.minFinesse) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
    }

    public int BaseVitality
    {
        // BaseVIT + ((MaxVIT - BaseVIT) * ((Level - 1) / MaxLevel))
        get { return (int)(primaryStatsGrowth.minVitality + ((primaryStatsGrowth.maxVitality - primaryStatsGrowth.minVitality) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
    }

    public int BaseSpirit
    {
        // BaseSPR + ((MaxSPR - BaseSPR) * ((Level - 1) / MaxLevel))
        get { return (int)(primaryStatsGrowth.minSpirit + ((primaryStatsGrowth.maxSpirit - primaryStatsGrowth.minSpirit) * (((float)Level - 1.0f) / (float)StatGrowth.KMaxLevel))); }
    }

    #endregion

#region SecondaryStats

	public virtual int MaxHealth
	{
        get{ return BaseMaxHealth;}
	}

	public virtual int CurrentHealth
	{
		get { return currentHealth; }
		set 
		{
            int maxHealth = MaxHealth;
            if (value > maxHealth)
            {
                value = maxHealth;
            }

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
         get{ return BaseMaxSpecial;}
	}

	public virtual int CurrentSpecial
	{
		get { return currentSpecial; }
		set
		{
            int maxSpecial = MaxSpecial;
            if (value > maxSpecial)
            {
                value = maxSpecial;
            }
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
        get { return BaseAttack; }
	}

	public virtual int PhysicalDefense
	{
        get { return BasePhysicalDefense; }
	}

	public virtual int MagicalDefense
	{
        get { return BaseMagicalDefense; }
	}

	public virtual float CriticalHitChance
	{
        get { return BaseCriticalHitChance; }
	}

	public virtual float CritalHitMultiplier
	{
        get { return BaseCritalHitMultiplier; }
	}

	public virtual float DodgeChance
	{
        get { return BaseDodgeChance; }
	}

    public virtual float SpecialPerStrike
    {
        get { return BaseSpecialPerStrike; }
    }

#endregion


    #region BaseSecondary

    public int BaseMaxHealth
    {
		// Base HP + (VIT * HP per VIT)
		get { return (int)(secondaryStats.health + (primaryStats.vitality * secondaryStatsGrowth.healthPerVit)); }
    }

    public int BaseMaxSpecial
    {
        // Base SP + (SPR * SP per SPR)
        get { return (int)(secondaryStats.special + (primaryStats.spirit * secondaryStatsGrowth.specialPerSpirit)); }
    }

    public int BaseAttack
    {
        get { return (int)(secondaryStats.attack + (secondaryStatsGrowth.attackPerPow * (float)Power)); }
    }

    public int BasePhysicalDefense
    {
        get { return (int)(secondaryStats.physicalDefense + (secondaryStatsGrowth.physicalDefPerVit * (float)Vitality)); }
    }

    public int BaseMagicalDefense
    {
        get { return (int)(secondaryStats.magicalDefense + (secondaryStatsGrowth.magicalDefPerSpr * (float)Spirit)); }
    }

    public float BaseCriticalHitChance
    {
        // BaseCritChance + (CritChancePerFIN * FIN)
        get { return secondaryStats.criticalHitChance + (secondaryStatsGrowth.critPerFin * (float)Finesse); }
    }

    public float BaseCritalHitMultiplier
    {
        // BaseCritMutlipler + (CritMultiplierPerFIN * FIN)
        get { return secondaryStats.criticalHitChance + (secondaryStatsGrowth.critMultPerFin * (float)Finesse); }
    }

    public float BaseDodgeChance
    {
        // BaseDodgeChance + (DodgeChancePerFIN * FIN)
        get { return secondaryStats.dodgeChance + (secondaryStatsGrowth.dodgePerFin * (float)Finesse); }
    }

    public float BaseSpecialPerStrike
    {
        // BaseDodgeChance + (DodgeChancePerFIN * FIN)
        get { return secondaryStats.specialPerStrike; }
    }
    
    #endregion


    /// <summary>
    ///  Will grab the most derived value of the stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetStat(EStats stat)
    {
        switch (stat)
        {
            case EStats.Power: return Power;
            case EStats.Finesse: return Finesse;
            case EStats.Vitality: return Vitality;
            case EStats.Spirit: return Spirit;
            case EStats.Health: return (float)MaxHealth;
            case EStats.Special: return (float)MaxSpecial;
            case EStats.Attack: return Attack;
            case EStats.PhysicalDefence: return (float)PhysicalDefense;
            case EStats.MagicalDefence: return (float)MagicalDefense;
            case EStats.DodgeChance: return DodgeChance;
            case EStats.CriticalHitChance: return CriticalHitChance;
            case EStats.CriticalHitMutliplier: return CritalHitMultiplier;
            case EStats.SpecialPerStrike: return SpecialPerStrike;
            default: { Debug.LogError("Unhandled case"); } break;
        }
        return 0.0f;
    }

    /// <summary>
    ///  Will grab the base stat for the given level.
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetBaseStat(EStats stat)
    {
        switch (stat)
        {
            case EStats.Power: return BasePower;
            case EStats.Finesse: return BaseFinesse;
            case EStats.Vitality: return BaseVitality;
            case EStats.Spirit: return BaseSpirit;
            case EStats.Health: return (float)BaseMaxHealth;
            case EStats.Special: return (float)BaseMaxSpecial;
            case EStats.Attack: return BaseAttack;
            case EStats.PhysicalDefence: return (float)BasePhysicalDefense;
            case EStats.MagicalDefence: return (float)BaseMagicalDefense;
            case EStats.DodgeChance: return BaseDodgeChance;
            case EStats.CriticalHitChance: return BaseCriticalHitChance;
            case EStats.CriticalHitMutliplier: return BaseCritalHitMultiplier;
            case EStats.SpecialPerStrike: return BaseSpecialPerStrike;
            default: { Debug.LogError("Unhandled case"); } break;
        }
        return 0.0f;
    }

}
