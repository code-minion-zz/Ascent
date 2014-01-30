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

        public int maxPower;
        public int maxFinesse;
        public int maxVitality;
        public int maxSpirit;

        public int physicalDefense;
        public int magicalDefense;

        public int healthPerVit;
        public int specialPerSpirit;
        public float critPerFinesse;
        public float critMultPerFinesse;
        public float dodgePerFinesse;
        public int defPerVitality;
        public int defPerSpirit;
	}
	// Warrior
	public static TBaseStats Warrior = new TBaseStats 
	{
        health = 20,
        special = 15,

        power = 10,
        finesse = 5,
        vitality = 10,
        spirit = 5,

        maxPower = 68,
        maxFinesse = 34,
        maxVitality = 39,
        maxSpirit = 34,

        physicalDefense = 1,
        magicalDefense = 1,

        healthPerVit = 5,
        specialPerSpirit = 1,
        critPerFinesse = 0.15f,
        critMultPerFinesse = 0.5f,
        dodgePerFinesse = 0.15f,
        defPerVitality = 1,
        defPerSpirit = 1
	};
	
	// Rogue
	public static TBaseStats Rogue = new TBaseStats
	{
        health = 20,
        special = 15,

        power = 10,
        finesse = 5,
        vitality = 10,
        spirit = 5,

        maxPower = 68,
        maxFinesse = 34,
        maxVitality = 39,
        maxSpirit = 34,

        physicalDefense = 1,
        magicalDefense = 1,

        healthPerVit = 5,
        specialPerSpirit = 1,
        critPerFinesse = 0.15f,
        critMultPerFinesse = 0.5f,
        dodgePerFinesse = 0.15f,
        defPerVitality = 1,
        defPerSpirit = 1
	};

	// Mage
	public static TBaseStats Mage = new TBaseStats
	{
        health = 20,
        special = 15,

        power = 10,
        finesse = 5,
        vitality = 10,
        spirit = 5,

        maxPower = 68,
        maxFinesse = 34,
        maxVitality = 39,
        maxSpirit = 34,

        physicalDefense = 1,
        magicalDefense = 1,

        healthPerVit = 5,
        specialPerSpirit = 1,
        critPerFinesse = 0.15f,
        critMultPerFinesse = 0.5f,
        dodgePerFinesse = 0.15f,
        defPerVitality = 1,
        defPerSpirit = 1
	};

	public static BaseStats GetNewBaseStatistics(Character.EHeroClass heroType, int level)
	{
		BaseStats stats = new BaseStats();

		switch (heroType)
		{
			case Character.EHeroClass.Warrior:
				{
                    stats.health = HeroBaseStats.Warrior.health;
                    stats.special = HeroBaseStats.Warrior.special;

                    stats.Power = HeroBaseStats.Warrior.power;
					stats.Finesse = HeroBaseStats.Warrior.finesse;
					stats.Vitality = HeroBaseStats.Warrior.vitality;
					stats.Spirit = HeroBaseStats.Warrior.spirit;

                    stats.physicalDefense = HeroBaseStats.Warrior.physicalDefense;
					stats.magicalDefense = HeroBaseStats.Warrior.magicalDefense;

                    stats.healthPerVit = HeroBaseStats.Warrior.healthPerVit;
                    stats.specialPerSpirit = HeroBaseStats.Warrior.specialPerSpirit;
                    stats.critPerFinesse = HeroBaseStats.Warrior.critPerFinesse;
                    stats.critMultPerFinesse = HeroBaseStats.Warrior.critMultPerFinesse;
                    stats.dodgePerFinesse = HeroBaseStats.Warrior.dodgePerFinesse;
                    stats.defPerVitality = HeroBaseStats.Warrior.defPerVitality;
                    stats.defPerSpirit = HeroBaseStats.Warrior.defPerSpirit;
                    stats.Level = level;
				}
				break;
			case Character.EHeroClass.Rogue:
				{
                    stats.health = HeroBaseStats.Rogue.health;
                    stats.special = HeroBaseStats.Rogue.special;

                    stats.Power = HeroBaseStats.Rogue.power;
                    stats.Finesse = HeroBaseStats.Rogue.finesse;
                    stats.Vitality = HeroBaseStats.Rogue.vitality;
                    stats.Spirit = HeroBaseStats.Rogue.spirit;

                    stats.healthPerVit = HeroBaseStats.Rogue.healthPerVit;
                    stats.specialPerSpirit = HeroBaseStats.Rogue.specialPerSpirit;
                    stats.critPerFinesse = HeroBaseStats.Rogue.critPerFinesse;
                    stats.critMultPerFinesse = HeroBaseStats.Rogue.critMultPerFinesse;
                    stats.dodgePerFinesse = HeroBaseStats.Rogue.dodgePerFinesse;
                    stats.defPerVitality = HeroBaseStats.Rogue.defPerVitality;
                    stats.defPerSpirit = HeroBaseStats.Rogue.defPerSpirit;
				}
				break;
			case Character.EHeroClass.Mage:
				{
                    stats.health = HeroBaseStats.Mage.health;
                    stats.special = HeroBaseStats.Mage.special;

                    stats.Power = HeroBaseStats.Mage.power;
                    stats.Finesse = HeroBaseStats.Mage.finesse;
                    stats.Vitality = HeroBaseStats.Mage.vitality;
                    stats.Spirit = HeroBaseStats.Mage.spirit;

                    stats.healthPerVit = HeroBaseStats.Mage.healthPerVit;
                    stats.specialPerSpirit = HeroBaseStats.Mage.specialPerSpirit;
                    stats.critPerFinesse = HeroBaseStats.Mage.critPerFinesse;
                    stats.critMultPerFinesse = HeroBaseStats.Mage.critMultPerFinesse;
                    stats.dodgePerFinesse = HeroBaseStats.Mage.dodgePerFinesse;
                    stats.defPerVitality = HeroBaseStats.Mage.defPerVitality;
                    stats.defPerSpirit = HeroBaseStats.Mage.defPerSpirit;
				}
				break;
			default:
				{
                    UnityEngine.Debug.LogError("Tried to make character of invalid type.");
				}
				break;
		}

		return stats;
	}
}
