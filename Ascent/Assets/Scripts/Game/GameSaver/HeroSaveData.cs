using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class HeroSaveData 
{
    // Save specific
    public float saveTime;
    public ulong id;

    // Hero Statistics
    public int level;
	public Character.EHeroClass type;
	
	// Hero Backpack 
    public Backpack backpack;
    
    // Hero Inventory
    public HeroInventory inventory;

	// Abilities
    public Action actions;

	public HeroSaveData()
	{
		saveTime = Time.time;
		level = 1;
	}

	public override string ToString()
	{
		string toString = "HeroSave\nTime: " + saveTime + "\n" +
							"ID: " + id;

		return toString;
	}
}
