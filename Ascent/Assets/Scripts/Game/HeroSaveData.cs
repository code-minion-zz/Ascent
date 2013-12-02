using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class HeroSaveData 
{
	public int level;
	public Character.EHeroClass type;
	public float saveTime;
	public ulong id;
	// Inventory
	// Abilities
	// Experience 
	public Data manage;
	public struct Data
	{
		//public int goldCount;
		//public int floorLevel;
		//public int gameTime;
		//public int maxHealth;
		//public int maxSpecial;
	}

	public HeroSaveData()
	{
		saveTime = Time.time;
		//manage.goldCount = 1;
		//manage.floorLevel = 1;
		//manage.gameTime = 1;
		//manage.maxHealth = 1;
		//manage.maxSpecial = 1;
		level = 1;
	}

	public override string ToString()
	{
		string toString = "HeroSave\nTime: " + saveTime + "\n" +
							"ID: " + id;

		return toString;
	}
}
