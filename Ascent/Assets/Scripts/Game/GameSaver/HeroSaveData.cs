using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class HeroSaveData 
{
    // System info
	public string name;
    public System.DateTime saveTime;
    public ulong uid;

	// Hero info
	public uint highestFloor;
	public int unasignedAbilityPoints;
	public int experience;
	public int gold;
	public int level;
	public Character.EHeroClass heroClass;
	public HeroStatGrowth growth;
	public Backpack backpack;
	public HeroInventory inventory;
    public HeroAbilityLoadout loadout;
	public List<Ability> abilities;


	public HeroSaveData()
	{
	}

	public HeroSaveData(Hero hero)
	{
		// System info
		name = "DefaultName";
		saveTime = System.DateTime.Now;
		uid = AscentGameSaver.GetUniqueID();

		// Hero info
        unasignedAbilityPoints = hero.unasignedAbilityPoints;
		experience = hero.HeroStats.Experience;
		gold = hero.HeroStats.Gold;
		level = hero.HeroStats.Level;
		heroClass = hero.HeroClass;
		growth = hero.HeroStats.Growth;
		backpack = hero.Backpack;
		inventory = hero.HeroInventory;
        loadout = hero.HeroLoadout;
		//abilities = hero.Abilities;
		highestFloor = hero.HighestFloorReached;

		// Also store the uid into the hero so it knows where to save itself later
		hero.SaveUID = uid;
	}

	public override string ToString()
	{
		string toString = "Lv" + level + " " + heroClass + "\n" +
							 saveTime + "\n" +
							 "uid " + uid;


		return toString;
	}
}
