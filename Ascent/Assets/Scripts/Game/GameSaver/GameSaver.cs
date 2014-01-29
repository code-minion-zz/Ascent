using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public static class GameSaver
{
	public static XMLSerialiser.DirectoryTarget targetDirection = XMLSerialiser.DirectoryTarget.APPLICATION;
    public const int maxSlots = 10;
	public static HeroSaveDataList heroSaves;

	public static HeroSaveDataList LoadAllHeroSaves()
    {
		heroSaves = XMLSerialiser.DeserializeObject(XMLSerialiser.LoadXML(targetDirection, "", "HeroSaveDataList.xml"), "HeroSaveDataList") as HeroSaveDataList;

		return heroSaves;
    }

    public static void DeleteSlot(HeroSaveData hero)
    {
		heroSaves.heroSaves.Remove(hero);
    }

    public static void CreateSlot(HeroSaveData hero)
    {
		if (heroSaves.heroSaves.Count < maxSlots)
		{
			heroSaves.heroSaves.Add(hero);
		}
    }

    public static void SaveGame(HeroSaveDataList saves)
    {
		XMLSerialiser.CreateXML(targetDirection, "", "HeroSaveDataList.xml", XMLSerialiser.SerializeObject(heroSaves));

	   // // Save the heros
	   // List<Player> players = Game.Singleton.Players;

	   // foreach (Player p in players)
	   // {
	   //      //TODO: Save to XML file.
	   //      //Save p.Hero into a file
	   //      //Save into tower progression
	   //      //Save statistics etc...
	   //      //Save inventory
	   //      //Hero slot

	   //     //bin.Serialize(file, p);
	   // }
    }

    public static void CreateTestSaves()
    {
		HeroSaveDataList saves = new HeroSaveDataList();

		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(1) });
		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(2) });
		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(3) });
		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(4) });
		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(5) });
		//saves.heroSaves.Add(new HeroSaveData() { uid = GetUniqueID(6) });

		XMLSerialiser.CreateXML(targetDirection, "", "HeroSaveDataList.xml", XMLSerialiser.SerializeObject(saves));
    }

	public static ulong GetUniqueID(int iAddionalSeed)
	{
		var random = new System.Random();
	
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

		string uniqueID = iAddionalSeed + "-"
			+ Application.systemLanguage                 //Language
		   + "-" + Application.platform                           //Device   
		   + "-" + System.String.Format("{0:X}", System.Convert.ToInt32(timestamp))          //Time
		   + "-" + System.String.Format("{0:X}", System.Convert.ToInt32(Time.time * 1000000))     //Time in game
		   + "-" + System.String.Format("{0:X}", random.Next(1000000000));          //random number

		
		return (ulong)uniqueID.GetHashCode();
	}
}