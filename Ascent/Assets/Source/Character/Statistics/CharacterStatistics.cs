using UnityEngine;
using System.Collections;

public class CharacterStatistics
{
    protected int curHealth;
	protected int maxHealth;
	
	protected int curSpecial;
	protected int maxSpecial;
	
	protected int level;
	protected int curExperience; // Also holds monster bounty 
	protected int maxExperience;
	protected int currency; // Also holds monster bounty
	
	// primary stats
	protected int power;
	protected int finesse;
	protected int vitality;
	protected int spirit;
	
	// derived stats
	protected int attack;
	protected int armour;
	protected float damageMitigation;
	protected float resistance;
	
	protected float criticalChance;
	protected float criticalDamage;
	
	protected float blockChance;
	protected float dodgeChance;

	public  int CurrentHealth
	{
		get { return curHealth; }
		set { curHealth = value; }
	}

	public  int MaxHealth
	{
		get { return maxHealth; }
		set { maxHealth = value; }
	}

	public  int CurrentSpecial
	{
		get { return curSpecial; }
		set { curSpecial = value; }
	}

	public  int MaxSpecial
	{
		get { return maxSpecial; }
		set { maxSpecial = value; }
	}

	public  int Level
	{
		get { return level; }
		set { level = value; }
	}

	public  int CurrentExperience
	{
		get { return curExperience; }
		set { curExperience = value; }
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

	public  float DamageMitigation
	{
		get { return damageMitigation; }
		set { damageMitigation = value; }
	}

	public  float Resistance
	{
		get { return resistance; }
		set { resistance = value; }
	}

	public  float CriticalChance
	{
		get { return criticalChance; }
		set { criticalChance = value; }
	}

	public  float CriticalDamage
	{
		get { return criticalDamage; }
		set { criticalDamage = value; }
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
}
