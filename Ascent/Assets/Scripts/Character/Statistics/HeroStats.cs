using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroStats : CharacterStats 
{
	protected Hero hero;
	protected HeroStatGrowth growth;

	protected int experience;
	protected int gold;

	protected int expRequiredAt1;
	protected int expRequiredAt30;

	public HeroStats(Hero hero)
	{
		Initialise(hero.HeroClass);
		level = 1;
		this.hero = hero;
	}

	public HeroStats(Hero hero, HeroSaveData data)
	{
		Initialise(data.heroClass);

		this.growth = data.growth;
		this.experience = data.experience;
		this.gold = data.gold;
		level = data.level;
		this.hero = hero;
	}


	public int Experience
	{
		get { return experience; }
		set { experience = value; }
	}

    public float ExperienceGainBonus
    {
        get 
        {
            // Check equipment, status effects, blessings, floor bonus.

            // Equipment
            float experienceBonus = 0.0f; 
            AccessoryItem[] accessories = hero.Backpack.AccessoryItems;
            foreach (AccessoryItem item in accessories)
            {
                if (item != null)
                {
                    experienceBonus += item.ExperienceGainBonus;
                }
            }

            // Status effects
            List<StatusEffect> statusEffects = hero.StatusEffects;
            foreach (StatusEffect effect in statusEffects)
            {
                if (effect is ExperienceBuff)
                {
                    experienceBonus += ((ExperienceBuff)effect).ExperienceGainBonus;
                }
				else if (effect is BlessingOfWisdom)
				{
					experienceBonus += ((BlessingOfWisdom)effect).ExperienceGainBonus;
				}
            }
      
            // Floor bonus
            experienceBonus += Game.Singleton.Tower.ExperienceGainBonus;

            return experienceBonus; 
        }
    }

	public int Gold
	{
		get { return gold; }
		set { gold = value; }
	}

    public float GoldGainBonus
    {
        get
        {
            // Equipment
            float goldBonus = 0.0f;
            AccessoryItem[] accessories = hero.Backpack.AccessoryItems;
            foreach (AccessoryItem item in accessories)
            {
                if (item != null)
                {
                    goldBonus += item.GoldGainBonus;
                }
            }

            // Status effects
            List<StatusEffect> statusEffects = hero.StatusEffects;
            foreach (StatusEffect effect in statusEffects)
            {
                if (effect is GoldBuff)
                {
                    goldBonus += ((GoldBuff)effect).GoldGainBonus;
                }
				else if (effect is BlessingOfWealth)
				{
					goldBonus += ((BlessingOfWealth)effect).GoldGainBonus;
				}
            }

            // Floor bonus
            goldBonus += Game.Singleton.Tower.GoldGainBonus;

            return goldBonus;
        }
    }

	public HeroStatGrowth Growth
	{
		get { return growth; }
	}

	public int RequiredExperience
	{
		// RequiredEXP@LV1 + ((RequiredEXP@LV30 - RequiredEXP@LV1) * ((Level / MaxLevel)^2))
		get { return (int)(expRequiredAt1 + (expRequiredAt30 - expRequiredAt1) * Mathf.Pow((float)Level / (float)StatGrowth.KMaxLevel, 2.0f)); }
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

    public override float SpecialPerStrike
    {
        get { return GetDerivedValue(base.SpecialPerStrike, EStats.SpecialPerStrike); }
    }
#endregion

    public float GetDerivedValue(float baseValue, EStats statType)
	{
		float withAccPrimary = AddAccessoriesPrimaryStats(baseValue, statType);
        float withAccProps = AddAccessoriesProperties(withAccPrimary, statType);
        float withBuffs = AddStatusEffects(withAccProps, statType);
		return withBuffs;
	}

	public float AddAccessoriesPrimaryStats(float statValue, EStats statType)
	{

		switch (statType)
		{
			case EStats.Power: // Fall
			case EStats.Finesse: // Fall
			case EStats.Vitality: // Fall
			case EStats.Spirit:
				{
					// Accessories usually have primary stats.

					Backpack backPack = hero.Backpack;

					int itemCount = backPack.AllItems.Length;
					
					if (itemCount > 0)
					{
						int i;
						for (i = 0; i < itemCount; ++i)
						{
							Item item = backPack.AllItems[i];
							if (item == null)
							{
								continue;
							}
							if (item is ConsumableItem)
							{
								continue;
							}
                            if (((AccessoryItem)item).IsBroken)
                            {
                                continue;
                            }

                            statValue += (int)((AccessoryItem)item).PrimaryStats.GetStat(statType);
						}
					}
				}
				break;
            case EStats.Health:
            case EStats.Special:
            case EStats.Attack:
            case EStats.PhysicalDefence:
            case EStats.MagicalDefence:
            case EStats.DodgeChance:
            case EStats.CriticalHitChance:
            case EStats.CriticalHitMutliplier:
                {
                    Backpack backPack = hero.Backpack;

                    int itemCount = backPack.AllItems.Length;
                    if (itemCount > 0)
                    {
                        int statsFromItems = 0;

                        int i;
                        for (i = 0; i < itemCount; ++i)
                        {
                            Item item = backPack.AllItems[i];
                            if (item == null)
                            {
                                continue;
                            }
                            if (item is ConsumableItem)
                            {
                                continue;
                            }
                            if (((AccessoryItem)item).IsBroken)
                            {
                                continue;
                            }
                            statsFromItems += (int)((AccessoryItem)item).PrimaryStats.GetRootStat(statType);
                        }

                        switch (statType)
                        {
                            case EStats.Health: statValue += secondaryStatsGrowth.healthPerVit * statsFromItems; break;
                            case EStats.Special: statValue += secondaryStatsGrowth.specialPerSpirit * statsFromItems; break;
                            case EStats.Attack: statValue += secondaryStatsGrowth.attackPerPow * statsFromItems; break;
                            case EStats.PhysicalDefence: statValue += secondaryStatsGrowth.physicalDefPerVit * statsFromItems; break;
                            case EStats.MagicalDefence: statValue += secondaryStatsGrowth.magicalDefPerSpr * statsFromItems; break;
                            case EStats.DodgeChance: statValue += secondaryStatsGrowth.dodgePerFin * statsFromItems; break;
                            case EStats.CriticalHitChance: statValue += secondaryStatsGrowth.critPerFin * statsFromItems; break;
                            case EStats.CriticalHitMutliplier: statValue += secondaryStatsGrowth.critMultPerFin * statsFromItems; break;
                            default: break;
                        }
                    }
                }
                break;
            case EStats.SpecialPerStrike: // Not used
                break;
			default: { Debug.LogError(statType + " is Unhandled case."); }break;
		}
		
		return statValue;
	}

	public float AddAccessoriesProperties(float statValue, EStats statType)
	{
		Backpack backPack = hero.Backpack;

		foreach (AccessoryItem item in backPack.AccessoryItems)
		{
			if (item == null)
			{
				continue;
			}
            if (item.IsBroken)
            {
                continue;
            }
			foreach(ItemProperty property in item.ItemProperties)
			{
				if (property is SecondaryStatItemProperty)
				{
					if (((SecondaryStatItemProperty)property).StatType == statType)
					{
						((SecondaryStatItemProperty)property).AddBuff(GetBaseStat(statType), ref statValue);
					}
				}
			}
		}

		return statValue;
	}

	public float AddStatusEffects(float statValue, EStats statType)
	{
		List<StatusEffect> buffList = hero.StatusEffects;

		int buffCount = buffList.Count;
		if (buffCount > 0)
		{		
			for (int i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is PrimaryStatModifierEffect)
				{
					if (((PrimaryStatModifierEffect)buffList[i]).StatType == statType)
					{
						((PrimaryStatModifierEffect)buffList[i]).AddBuff(GetBaseStat(statType), ref statValue);
					}
				}
                else if (buffList[i] is SecondaryStatModifierEffect)
                {
                    if (((SecondaryStatModifierEffect)buffList[i]).statType == statType)
                    {

                        ((SecondaryStatModifierEffect)buffList[i]).AddBuff(GetBaseStat(statType), ref statValue);
                    }
                }
			}
		}

		return statValue;
	}

	private void Initialise(Hero.EHeroClass heroClass)
	{
		expRequiredAt1 = 1000;
		expRequiredAt30 = 1000000;

		// TODO: Data drive these values
		switch (heroClass)
		{
			case Hero.EHeroClass.Warrior:
				{
					primaryStats = new PrimaryStats()
					{
						power = 0,
						finesse = 0,
						vitality = 0,
						spirit = 0
					};

					primaryStatsGrowth = new PrimaryStatsGrowthRates()
					{
						minPower = primaryStats.power,
						maxPower = 68,
						minFinesse = primaryStats.finesse,
						maxFinesse = 34,
						minVitality = primaryStats.vitality,
						maxVitality = 39,
						minSpirit = primaryStats.spirit,
						maxSpirit = 34
					};

					secondaryStats = new SecondaryStats()
					{
						health = (float)Game.Singleton.startingHealth,
						special = (float)Game.Singleton.startingSpecial,
						attack = 1.0f,
						physicalDefense = 1.0f,
						magicalDefense = 1.0f,
						criticalHitChance = 5.0f,
						criticalHitMultiplier = 25.0f,
						dodgeChance = 2.5f,
                        specialPerStrike = 1.0f,
					};

					secondaryStatsGrowth = new SecondaryStatsGrowthRates()
					{
						healthPerVit = 5.0f,
						specialPerSpirit = 1.0f,
						attackPerPow = 1.0f,
						physicalDefPerVit = 1.0f,
						magicalDefPerSpr = 1.0f,
						critPerFin = 0.15f,
						critMultPerFin = 0.5f,
						dodgePerFin = 0.15f,
					};
				}
				break;
			case Hero.EHeroClass.Rogue:
				{
					primaryStats = new PrimaryStats()
					{
						power = 10,
						finesse = 5,
						vitality = 10,
						spirit = 5
					};

					primaryStatsGrowth = new PrimaryStatsGrowthRates()
					{
						minPower = primaryStats.power,
						maxPower = 68,
						minFinesse = primaryStats.finesse,
						maxFinesse = 34,
						minVitality = primaryStats.vitality,
						maxVitality = 39,
						minSpirit = primaryStats.spirit,
						maxSpirit = 34
					};

					secondaryStats = new SecondaryStats()
					{
						health = 20.0f,
						special = 15.0f,
						physicalDefense = 1.0f,
						magicalDefense = 1.0f,
						criticalHitChance = 5.0f,
						criticalHitMultiplier = 25.0f,
						dodgeChance = 2.5f,
					};

					secondaryStatsGrowth = new SecondaryStatsGrowthRates()
					{
						healthPerVit = 5.0f,
						specialPerSpirit = 1.0f,
						physicalDefPerVit = 1.0f,
						magicalDefPerSpr = 1.0f,
						critPerFin = 0.15f,
						critMultPerFin = 0.5f,
						dodgePerFin = 0.15f,
					};
				}
				break;
			case Hero.EHeroClass.Mage:
				{
					primaryStats = new PrimaryStats()
					{
						power = 10,
						finesse = 5,
						vitality = 10,
						spirit = 5
					};

					primaryStatsGrowth = new PrimaryStatsGrowthRates()
					{
						minPower = primaryStats.power,
						maxPower = 68,
						minFinesse = primaryStats.finesse,
						maxFinesse = 34,
						minVitality = primaryStats.vitality,
						maxVitality = 39,
						minSpirit = primaryStats.spirit,
						maxSpirit = 34
					};

					secondaryStats = new SecondaryStats()
					{
						health = 20.0f,
						special = 15.0f,
						physicalDefense = 1.0f,
						magicalDefense = 1.0f,
						criticalHitChance = 5.0f,
						criticalHitMultiplier = 25.0f,
						dodgeChance = 2.5f,
					};

					secondaryStatsGrowth = new SecondaryStatsGrowthRates()
					{
						healthPerVit = 5.0f,
						specialPerSpirit = 1.0f,
						physicalDefPerVit = 1.0f,
						magicalDefPerSpr = 1.0f,
						critPerFin = 0.15f,
						critMultPerFin = 0.5f,
						dodgePerFin = 0.15f,
					};
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case");
				}
				break;

		}
	}
}

