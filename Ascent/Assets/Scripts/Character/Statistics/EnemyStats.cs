using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : CharacterStats 
{
    public Enemy enemy;
	public float experienceBounty;
	public float goldBounty;

	public PrimaryStats PrimaryStats
	{
		get { return primaryStats; }
		set { primaryStats = value; }
	}

	public PrimaryStatsGrowthRates PrimaryStatsGrowthRates
	{
		get { return primaryStatsGrowth; }
		set { primaryStatsGrowth = value; }
	}

	public SecondaryStats SecondaryStats
	{
		get { return secondaryStats; }
		set { secondaryStats = value; }
	}


	public SecondaryStatsGrowthRates SecondaryStatsGrowthRates
	{
		get { return secondaryStatsGrowth; }
		set { secondaryStatsGrowth = value; }
	}

	public float ExperienceBounty
	{
		get { return experienceBounty; }
		set { experienceBounty = value; }
	}

	public float GoldBounty
	{
		get { return goldBounty; }
		set { goldBounty = value; }
	}


#region PrimaryStats

    public override int Power
    {
        get { return (int)GetDerivedValue(base.Power, EStats.Power); }
    }

    public override int Finesse
    {
        get { return (int)GetDerivedValue(base.Finesse, EStats.Finesse); }
    }

    public override int Vitality
    {
        get { return (int)GetDerivedValue(base.Vitality, EStats.Vitality); }
    }

    public override int Spirit
    {
        get { return (int)GetDerivedValue(base.Spirit, EStats.Spirit); }
    }

#endregion


#region SecondaryStats

    public override int MaxHealth
    {
        get { return (int)GetDerivedValue(base.MaxHealth, EStats.Health); }
    }

    public override int MaxSpecial
    {
        get { return (int)GetDerivedValue(base.MaxSpecial, EStats.Special); }
    }

    public override int Attack
    {

        get { return (int)GetDerivedValue(base.Attack, EStats.Attack); }
    }

    public override int PhysicalDefense
    {
        get { return (int)GetDerivedValue(base.PhysicalDefense, EStats.PhysicalDefence); }
    }

    public override int MagicalDefense
    {
        get { return (int)GetDerivedValue(base.MagicalDefense, EStats.MagicalDefence); }
    }

    public override float CriticalHitChance
    {
        get { return GetDerivedValue(base.CriticalHitChance, EStats.CriticalHitChance); }
    }

    public override float CritalHitMultiplier
    {
        get { return GetDerivedValue(base.CritalHitMultiplier, EStats.CriticalHitMutliplier); }
    }

    public override float DodgeChance
    {
        get { return GetDerivedValue(base.DodgeChance, EStats.DodgeChance); }
    }

#endregion


    public float GetDerivedValue(float baseValue, EStats statType)
    {
        float withBuffs = AddStatusEffects(baseValue, statType);
        return withBuffs;
    }


    public float AddStatusEffects(float statValue, EStats statType)
    {
        List<StatusEffect> statusEffectList = enemy.StatusEffects;

        int statusEffectCount = statusEffectList.Count;

        if (statusEffectCount > 0)
        {
            for (int i = 0; i < statusEffectCount; ++i)
            {
                if (statusEffectList[i] is BaseStatBuff)
                {
                    if (((BaseStatBuff)statusEffectList[i]).type == statType)
                    {
                        ((BaseStatBuff)statusEffectList[i]).AddBuff(GetBaseStat(statType),ref  statValue);
                    }
                }
                else if (statusEffectList[i] is SecondaryStatBuff)
                {
                    if (((SecondaryStatBuff)statusEffectList[i]).statType == statType)
                    {
                        ((SecondaryStatBuff)statusEffectList[i]).AddBuff(GetBaseStat(statType),ref statValue);
                    }
                }
            }
        }

        return statValue;
    }
}
