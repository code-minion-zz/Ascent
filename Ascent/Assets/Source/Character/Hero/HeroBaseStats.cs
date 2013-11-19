// Developed by Mana Khamphanpheng 2013

// Dependencies
using System.Collections;

public static class HeroBaseStats 
{
	public struct TBaseStats
	{
		public int health;
		public int special;
		public int power;
		public int finesse;
		public int vitality;
		public int spirit;
	}
	// Warrior
	public static TBaseStats Warrior = new TBaseStats 
	{
		health = 65,
		special = 20,
		power = 5,
		finesse = 3,
		vitality = 4,
		spirit = 2
	};
	
	// Rogue
	public static TBaseStats Rogue = new TBaseStats
	{
		health = 65,
		special = 20,
		power = 5,
		finesse = 3,
		vitality = 4,
		spirit = 2
	};

	// Mage
	public static TBaseStats Mage = new TBaseStats
	{
		health = 65,
		special = 20,
		power = 5,
		finesse = 3,
		vitality = 4,
		spirit = 2
	};

	public static CharacterStatistics GetNewBaseStatistics(Character.EHeroClass heroType)
	{
		CharacterStatistics stats = new CharacterStatistics();

		switch (heroType)
		{
			case Character.EHeroClass.Warrior:
				{
					stats.CurrentHealth = HeroBaseStats.Warrior.health;
					stats.MaxHealth = HeroBaseStats.Warrior.health;
					stats.Power = HeroBaseStats.Warrior.power;
					stats.Finesse = HeroBaseStats.Warrior.finesse;
					stats.Vitality = HeroBaseStats.Warrior.vitality;
					stats.Spirit = HeroBaseStats.Warrior.spirit;
                    stats.CurrentExperience = 0;
                    stats.MaxExperience = 1000;
				}
				break;
			case Character.EHeroClass.Rogue:
				{
					stats.CurrentHealth = HeroBaseStats.Rogue.health;
					stats.MaxHealth = HeroBaseStats.Rogue.health;
					stats.Power = HeroBaseStats.Rogue.power;
					stats.Finesse = HeroBaseStats.Rogue.finesse;
					stats.Vitality = HeroBaseStats.Rogue.vitality;
					stats.Spirit = HeroBaseStats.Rogue.spirit;
                    stats.CurrentExperience = 0;
                    stats.MaxExperience = 1000;
				}
				break;
			case Character.EHeroClass.Mage:
				{
					stats.CurrentHealth = HeroBaseStats.Mage.health;
					stats.MaxHealth = HeroBaseStats.Mage.health;
					stats.Power = HeroBaseStats.Mage.power;
					stats.Finesse = HeroBaseStats.Mage.finesse;
					stats.Vitality = HeroBaseStats.Mage.vitality;
					stats.Spirit = HeroBaseStats.Mage.spirit;
                    stats.CurrentExperience = 0;
                    stats.MaxExperience = 1000;
				}
				break;
			default:
				{
					//Debug.LogError("Tried to make character of invalid type.");
				}
				break;
		}

		return stats;
	}
}
