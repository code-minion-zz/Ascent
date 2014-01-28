using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public static class AscentGameSaver
{
	public delegate void OnHeroSaveListChanged();
	public static event OnHeroSaveListChanged OnHeroSaveListChangedEvent;

	private static GameSaveData gameSave;
	public static GameSaveData SaveData
	{
		get { return gameSave; }
	}

	private static string saveFilePath = Application.persistentDataPath + "\\ascent_save.xml";


	/// <summary>
	/// Saves serialised GameSave
	/// </summary>
	public static void SaveGame()
	{
		string saveString = XMLSerialiser.SerializeObject(gameSave);
		XMLSerialiser.CreateXML(saveFilePath, saveString);
	}

	/// <summary>
	/// Attempts to load an existing GameSave. 
	/// Creates a GameSave if it fails.
	/// </summary>
	/// <returns> True if a GameSave was successfully loaded. </returns>
	public static bool LoadGameDataSave()
	{
		if (DoesFilePathExist(saveFilePath))
		{
			// An existing save exists to load it in.
			gameSave = XMLSerialiser.DeserializeObject(XMLSerialiser.LoadXML(saveFilePath), "GameSaveData") as GameSaveData;

			return true;
		}

		// No existing save exists so create one.
		CreateNewGameDataSave();

		return false;
	}

	/// <summary>
	/// Creates a new game save data to store global game progress. 
	/// Should only be called once the very first time the player starts the game.
	/// This cannot be deleted. Unless user goes into application data.
	/// </summary>
	private static void CreateNewGameDataSave()
	{
		gameSave = new GameSaveData();
		SaveGame();
	}

	/// <summary>
	/// Creates a new hero save to store individual hero/player progress.
	/// The new save is automatically added to the list of hero saves.
	/// Notifies any delegates of the change to the list.
	/// </summary>
	public static void CreateNewHeroSave(Hero hero)
	{
		// Add hero to the save
		gameSave.heroSaves.Add(new HeroSaveData(hero));

		// Immediately save change to file
		SaveGame();

		// Notify delegates so all UI can be updated.
		NotifyHeroSaveListChanged();
	}

	/// <summary>
	/// Constructs HeroSave data out of Hero.
	/// Finds the Hero in the HeroSave list and writes over it.
	/// </summary>
	/// <param name="data"> Hero to save. </param>
	public static void SaveHero(Hero hero)
	{
		// Find the  heroes data
		HeroSaveData saveData = null;
		int saveIndex = 0;
		for(; saveIndex < gameSave.heroSaves.Count; ++saveIndex)
		{
			saveData = gameSave.heroSaves[saveIndex];

			if(saveData.uid == hero.SaveUID)
			{
				break;
			}
		}

		// The hero oddly does not have a save. Make one now.
		if (saveIndex >= gameSave.heroSaves.Count)
		{
			CreateNewHeroSave(hero);
		}
		else // Found the heroes data. Assign to it to replace the old save.
		{
			gameSave.heroSaves[saveIndex] = new HeroSaveData(hero);
		}

		// Immediately save change to file
		SaveGame();

		// Notify delegates so all UI can be updated.
		NotifyHeroSaveListChanged();
	}

	/// <summary>
	/// Constructs a Hero out of HeroSaveData.
	/// </summary>
	/// <param name="data"> HeroSaveData to convert into a hero. </param>
	/// <returns> The loaded Hero. </returns>
	public static Hero LoadHero(HeroSaveData data)
	{
		Hero loadedHero = HeroFactory.CreateNewHero(data.classType);
		GameObject loadedHeroObject = loadedHero.gameObject;

		// Load base stats
		BaseStats baseStats = loadedHero.CharacterStats;

		// Load Inventory
		HeroInventory invetory = loadedHero.HeroInventory;

		// Load backback
		Backpack backpack = loadedHero.HeroBackpack;

		// Load Abilities
		List<Action> actions = loadedHero.Abilities;

		return loadedHero;
	}

	/// <summary>
	/// Deletes the given HeroSave from the list and immediately saves to file.
	/// The new save is automatically removed from list of hero saves.
	/// Notifies any delegates of the change.
	/// </summary>
	public static void DeleteHeroSave(HeroSaveData data)
	{
		// Remove the entry
		gameSave.heroSaves.Remove(data);

		// Immediately save the change to file
		SaveGame();

		// Notify delegates
		NotifyHeroSaveListChanged();
	}


	/// <summary>
	/// Notifies any delegates that the list of saves has changed.
	/// </summary>
	public static void NotifyHeroSaveListChanged()
	{
		if (OnHeroSaveListChangedEvent != null)
		{
			OnHeroSaveListChangedEvent.Invoke();
		}
	}

	public static bool DoesFilePathExist(string path)
	{
		FileInfo t = new FileInfo(path);
		return t.Exists;
	}

	public static ulong GetUniqueID()
	{
		var random = new System.Random();

		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

		string uniqueID = Application.systemLanguage                 //Language
		   + "-" + Application.platform                           //Device   
		   + "-" + System.String.Format("{0:X}", System.Convert.ToInt32(timestamp))          //Time
		   + "-" + System.String.Format("{0:X}", System.Convert.ToInt32(Time.time * 1000000))     //Time in game
		   + "-" + System.String.Format("{0:X}", random.Next(1000000000));          //random number


		return (ulong)uniqueID.GetHashCode();
	}
}