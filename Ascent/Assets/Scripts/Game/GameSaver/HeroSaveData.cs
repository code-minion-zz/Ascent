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
	public Character.EHeroClass classType;
	public BaseStats baseStats;
	public Backpack backpack;
	public HeroInventory inventory;
	public List<Action> abilities;
	public uint highestFloor;

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
		classType = hero.ClassType;
		baseStats = hero.CharacterStats;
		backpack = hero.HeroBackpack;
		inventory = hero.HeroInventory;
		highestFloor = hero.HighestFloorReached;

		// Also store the uid into the hero so it knows where to save itself later
		hero.SaveUID = uid;
	}

	public override string ToString()
	{
		string toString = "Lv" + baseStats.Level + " " + classType + "\n" +
							 saveTime;


		return toString;
	}
}
