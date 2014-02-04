using UnityEngine;
using System.Collections;

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

	public int Gold
	{
		get { return gold; }
		set { gold = value; }
	}

	public HeroStatGrowth Growth
	{
		get { return growth; }
	}

	public int CurrentExperience
	{
		get { return experience; }
		set { experience = value; }
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
		get { return (int)GetDerivedValue(base.CriticalHitChance, EStats.CriticalHitChance); }
	}

	public override float CritalHitMultiplier
	{
		get { return (int)GetDerivedValue(base.CritalHitMultiplier, EStats.CriticalHitMutliplier); }
	}

	public override float DodgeChance
	{
		get { return (int)GetDerivedValue(base.DodgeChance, EStats.DodgeChance); }
	}

#endregion

	public float GetDerivedValue(float baseValue, EStats statType)
	{
		float withAccPrimary = AddAccessoriesPrimaryStats(baseValue, statType);
		float withAccProps = AddAccessoriesProperties(withAccPrimary, statType);
        float withBuffs = AddBuffs(withAccProps, statType);
		return (int)withBuffs;
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
							statValue += (int)((AccessoryItem)item).PrimaryStats.GetStat(statType);
						}
					}
				}
				break;
			case EStats.Health:break;
			case EStats.Special:break;
			case EStats.Attack:break;
			case EStats.PhysicalDefence:break;
			case EStats.MagicalDefence:break;
			case EStats.DodgeChance:break;
			case EStats.CriticalHitChance:break;
			case EStats.CriticalHitMutliplier:break;
			default: { Debug.LogError("Unhandled case."); }break;
		}
		
		return statValue;
	}

	public float AddAccessoriesProperties(float statValue, EStats statType)
	{
		// TODO: Parse through accessory properties to look for stat changing ones.
		// TODO: Add the stat changing ones to the value.

		return statValue;
	}

	public float AddBuffs(float statValue, EStats statType)
	{
		// NOTE: This only looks for base stat buffs
		// TODO: Parse for secondary stats aswell
		BetterList<Buff> buffList = hero.BuffList;

		int buffCount = buffList.size;

		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					if (((BaseStatBuff)buffList[i]).type == statType)
					{
						((BaseStatBuff)buffList[i]).AddBuff(statValue);
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
						attack = 0.0f,
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

