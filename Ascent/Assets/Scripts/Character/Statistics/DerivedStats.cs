using UnityEngine;
using System.Collections;


/// <summary>
/// TODO : Use properly
/// </summary>
public class DerivedStats
{
	protected int curHealth =1;
	protected int maxHealth =1;
	
	protected int curSpecial;
	protected int maxSpecial;

	protected int lives;
			
	// derived stats
	// offensive
	protected int attack;				// Power * Mods (example : mods = inner fire 10% attack bonus)
	protected float criticalChance;		// Chance to do more damage
	protected float criticalBonus;		// How much stronger are crits?
	
	// defensive
	protected int 	armour;				// Contributes to Phys & Mag resistances, gained from buffs and equipment.
	protected float physicalResistance;	// Reduces Phys damage taken by %. Value from 0 to 1.
	protected float magicalResistance;	// Reduces Mag damage taken by %. Value from 0 to 1.
		
	protected float blockChance;
	protected float dodgeChance;

	public bool dirty = true;			// if stat is dirty, everything needs to be recalculated

	public event CharacterStatisticEventHandler onMaxHealthChanged;
	public event CharacterStatisticEventHandler onMaxSpecialChanged;
	public event CharacterStatisticEventHandler onCurHealthChanged;
	public event CharacterStatisticEventHandler onCurSpecialChanged;
	//public event CharacterStatisticEventHandler onAttackChanged;
	//public event CharacterStatisticEventHandler onCritChanceChanged;
	//public event CharacterStatisticEventHandler onCritBonusChanged;
	//public event CharacterStatisticEventHandler onArmorChanged;
	//public event CharacterStatisticEventHandler onPhysResChanged;
	//public event CharacterStatisticEventHandler onMagResChanged;
	//public event CharacterStatisticEventHandler onBlockChanged;
	//public event CharacterStatisticEventHandler onDodgeChanged;
	public event CharacterStatisticEventHandler onAnyStatChanged;

	public DerivedStats(BaseStats _base)
	{
		_base.onPowerChanged += HandlePowerChanged;
		_base.onFinesseChanged += HandleFinesseChanged;
		_base.onSpiritChanged += HandleSpiritChanged;
		_base.onVitalityChanged += HandleVitalityChanged;

		MaxHealth = _base.Vitality * 10;
		//CurrentHealth = MaxHealth;
	}

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
			// scales current health up to new maximum
			float percentage = 0;
			if (maxHealth != 0)
			{
				percentage =  curHealth/maxHealth;
			}
			maxHealth = value;
			curHealth =(int) percentage * maxHealth;
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
             if (curSpecial > maxSpecial)
             {
                 curSpecial = maxSpecial;
             }
             else if (curSpecial < 0)
             {
                 curSpecial = 0;
             }
 
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

	public int Lives
	{
		get { return lives; }
		set
		{
			lives = value;
		}
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
	
	void HandlePowerChanged(float value)
	{
		Attack = (int)value;
	}
	
	void HandleFinesseChanged(float value)
	{
		criticalChance = value;
		criticalBonus = value;
	}
	
	void HandleVitalityChanged(float value)
	{
		PhysicalResistance = value;
		MaxHealth = (int)value * 10;
	}
	
	void HandleSpiritChanged(float value)
	{
		MagicalResistance = value;
		MaxSpecial = (int)value * 10;
	}
}
