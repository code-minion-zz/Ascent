using UnityEngine;
using System.Collections;

public delegate void CharacterStatisticEventHandler(float newValue);

// TODO: USE PROPERLY
public class BaseStats
{
	protected int level;
	protected int curExperience; 		// Also holds enemy bounty 
	//protected int maxExperience;		// Always 1000 exp to level up for the time being
    protected int currency; 			// Also holds enemy bounty
	
	// primary stats
	protected int power;				// Increases Attack
	protected int finesse;				// Increases chance for crit, dodge, block, etc
	protected int vitality;				// Increases Max HP, Physical Resistance, HP Regen
	protected int spirit;				// Increases Max Special, Magic Resistance
		
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
}
