using UnityEngine;
using System.Collections;

public class CharacterStatistics
{
    protected int curHealth;
	protected int maxHealth;
	
	protected int curSpecial;
	protected int maxSpecial;
	
	protected int level;
	protected int curExperience; 		// Also holds monster bounty 
	protected int maxExperience;
	protected int currency; 			// Also holds monster bounty
	
	// primary stats
	protected int power;				// Increases Attack
	protected int finesse;				// Increases chance for crit, dodge, block, etc
	protected int vitality;				// Increases Max HP, Physical Resistance, HP Regen
	protected int spirit;				// Increases Max Special, Magic Resistance
	
	// derived stats
	// offensive
	protected int attack;				// Power + Equipment + Buffs + Mods, usually affects damage
	protected float criticalChance;		// Chance to do more damage
	protected float criticalBonus;		// How much stronger are crits?
	
	// defensive
	protected int armour;				// Contributes to Phys & Mag resistances, gained from buffs and equipment.
	protected float physicalResistance;	// Reduces Phys damage taken by %. Value from 0 to 1.
	protected float magicalResistance;	// Reduces Mag damage taken by %. Value from 0 to 1.
		
	protected float blockChance;
	protected float dodgeChance;
	
	public delegate void CharacterStatisticEventHandler(float newValue);
	public event CharacterStatisticEventHandler onMaxHealthChanged;
	public event CharacterStatisticEventHandler onMaxSpecialChanged;
	public event CharacterStatisticEventHandler onCurHealthChanged;
	public event CharacterStatisticEventHandler onCurSpecialChanged;
	public event CharacterStatisticEventHandler onLevelChanged;
	public event CharacterStatisticEventHandler onExpChanged;
	//public event CharacterStatisticEventHandler onMoneyChanged;
	//public event CharacterStatisticEventHandler onPowerChanged;
	//public event CharacterStatisticEventHandler onFinesseChanged;
	//public event CharacterStatisticEventHandler onVitalityChanged;
	//public event CharacterStatisticEventHandler onSpiritChanged;
	//public event CharacterStatisticEventHandler onAttackChanged;
	//public event CharacterStatisticEventHandler onCritChanceChanged;
	//public event CharacterStatisticEventHandler onCritBonusChanged;
	//public event CharacterStatisticEventHandler onAnyOffensiveChanged;
	//public event CharacterStatisticEventHandler onArmorChanged;
	//public event CharacterStatisticEventHandler onPhysResChanged;
	//public event CharacterStatisticEventHandler onMagResChanged;
	//public event CharacterStatisticEventHandler onBlockChanged;
	//public event CharacterStatisticEventHandler onDodgeChanged;
	//public event CharacterStatisticEventHandler onAnyDefensiveChanged;
	public event CharacterStatisticEventHandler onAnyStatChanged;

	public  int CurrentHealth
	{
		get { return curHealth; }
		set 
		{ 
			curHealth = value;
			if (onCurHealthChanged != null)		onCurHealthChanged(curHealth);
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

	public  int MaxHealth
	{
		get { return maxHealth; }
		set 
		{ 
			maxHealth = value;
			if (onMaxHealthChanged != null)		onMaxHealthChanged(maxHealth);
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

	public  int CurrentSpecial
	{
		get { return curSpecial; }
		set 
		{
			curSpecial = value;
			if (onCurSpecialChanged != null)	onCurSpecialChanged(curSpecial);
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

	public  int MaxSpecial
	{
		get { return maxSpecial; }
		set { 
			maxSpecial = value; 
			if (onMaxSpecialChanged != null)	onMaxSpecialChanged(maxSpecial);
			if (onAnyStatChanged != null)		onAnyStatChanged(0);
		}
	}

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

	public  int MaxExperience
	{
		get { return maxExperience; }
		set { maxExperience = value; }
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
		set { vitality = value; }
	}

	public  int Spirit
	{
		get { return spirit; }
		set { spirit = value; }
	}

	public  int Attack
	{
		get { return attack; }
		set { attack = value; }
	}

	public  int Armour
	{
		get { return armour; }
		set { armour = value; }
	}

	public  float PhysicalResistance
	{
		get { return physicalResistance; }
		set { physicalResistance = value; }
	}

	public  float MagicalResistance
	{
		get { return magicalResistance; }
		set { magicalResistance = value; }
	}

	public  float CriticalChance
	{
		get { return criticalChance; }
		set { criticalChance = value; }
	}

	public  float CriticalBonus
	{
		get { return criticalBonus; }
		set { criticalBonus = value; }
	}

	public  float BlockChance
	{
		get { return blockChance; }
		set { blockChance = value; }
	}

	public  float DodgeChance
	{
		get { return dodgeChance; }
		set { dodgeChance = value; }
	}

    public void ResetHealth()
    {
        // We use the properties because this notifies events tied to the health to also update,
        // in cases such as GUI.
        CurrentHealth = MaxHealth;
    }
}
