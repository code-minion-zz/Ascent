using UnityEngine;
using System.Collections;

public delegate void CharacterStatisticEventHandler(float newValue);

// TODO: USE PROPERLY
public class BaseStats
{
    const int KMaxLevel = 30;
    const int KExpRequiredAt1 = 1000;
    const int KExpRequiredAt30 = 1000000;

	protected int level;
	protected int curExperience; 		// Also holds enemy bounty 
	//protected int maxExperience;		// Always 1000 exp to level up for the time being
    protected int currency; 			// Also holds enemy bounty
	
	// primary stats
    public int health;
    public int special;

	protected int power;				// Increases Attack
	protected int finesse;				// Increases chance for crit, dodge, block, etc
	protected int vitality;				// Increases Max HP, Physical Resistance, HP Regen
	protected int spirit;				// Increases Max Special, Magic Resistance

    public int physicalDefense;
    public int magicalDefense;

    public int healthPerVit;
    public int specialPerSpirit;
    public float critPerFinesse;
    public float critMultPerFinesse;
    public float dodgePerFinesse;
    public int defPerVitality;
    public int defPerSpirit;
		
#pragma warning disable 0067
	public event CharacterStatisticEventHandler onLevelChanged;
	public event CharacterStatisticEventHandler onExpChanged;
	//public event CharacterStatisticEventHandler onMoneyChanged;
	public event CharacterStatisticEventHandler onPowerChanged;
	public event CharacterStatisticEventHandler onFinesseChanged;
	public event CharacterStatisticEventHandler onVitalityChanged;
	public event CharacterStatisticEventHandler onSpiritChanged;
	public event CharacterStatisticEventHandler onAnyStatChanged;


	public  int Level
	{
		get { return level; }
		set 
		{ 
			level = value;

            LevelUp();

			if (onLevelChanged != null)			onLevelChanged(level);			
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

	public  int CurrentExperience
	{
		get { return curExperience; }
		set { 
			curExperience = value; 			
			if (onExpChanged != null)			onExpChanged(curExperience);			
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

    public int MaxExperience
    {
        get { return CalculateRequiredExperience(level); }
    }

	public  int ExperienceBounty
	{
		get { return curExperience; }
		set { curExperience = value; }
	}

	public  int CurrencyBounty
	{
		get { return currency; }
		set { currency = value; }
	}

	public  int Currency
	{
		get { return currency; }
		set { currency = value; }
	}

	public  int Power
	{
		get { return power; }
		set { power = value; }
	}

	public  int Finesse
	{
		get { return finesse; }
		set { finesse = value; }
	}

	public  int Vitality
	{
		get { return vitality; }
		set 
		{ 
			vitality = value;
			if (onVitalityChanged != null)		onVitalityChanged(value);
		}
	}

	public  int Spirit
	{
		get { return spirit; }
		set { spirit = value; }
	}

    public int CalculateRequiredExperience(int _level)
    {
        float maxLevel = (float)KMaxLevel;
        return (int)(KExpRequiredAt1 + (KExpRequiredAt30 - KExpRequiredAt1) * (level / maxLevel));
    }

    private void LevelUp()
    {
        float maxLevel = (float)KMaxLevel;
        Power = (int)(HeroBaseStats.Warrior.power + ((HeroBaseStats.Warrior.maxPower - HeroBaseStats.Warrior.power) * ((level - 1) / maxLevel)));
        Finesse = (int)(HeroBaseStats.Warrior.finesse + ((HeroBaseStats.Warrior.maxFinesse - HeroBaseStats.Warrior.finesse) * ((level - 1) / maxLevel)));
        Vitality = (int)(HeroBaseStats.Warrior.vitality + ((HeroBaseStats.Warrior.maxVitality - HeroBaseStats.Warrior.vitality) * ((level - 1) / maxLevel)));
        Spirit = (int)(HeroBaseStats.Warrior.spirit + ((HeroBaseStats.Warrior.maxSpirit - HeroBaseStats.Warrior.spirit) * ((level - 1) / maxLevel)));
    }
}
