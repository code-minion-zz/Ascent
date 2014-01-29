using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public static class AscentGameSaver
{
	private const string fileName = "ascent_save.xml";
	private static string saveFilePath = Application.persistentDataPath + "\\" + fileName;

	public delegate void OnHeroSaveListChanged();
	public static event OnHeroSaveListChanged OnHeroSaveListChangedEvent;

	private static GameSaveData gameSave;
	public static GameSaveData SaveData
	{
		get { return gameSave; }
	}


	/// <summary>
	/// Saves serialised GameSave
	/// </summary>
	public static void SaveGame()
	{
		string saveString = XMLSerialiser.SerializeObject(gameSave);
		
#if UNITY_WEBPLAYER
		PlayerPrefs.SetString(fileName, saveString);
		PlayerPrefs.Save();
#else
		XMLSerialiser.CreateXML(saveFilePath, saveString);
#endif
	}

	/// <summary>
	/// Attempts to load an existing GameSave. 
	/// Creates a GameSave if it fails.
	/// </summary>
	/// <returns> True if a GameSave was successfully loaded. </returns>
	public static bool LoadGame()
	{		
		if (DoesFilePathExist(saveFilePath))
		{
#if UNITY_WEBPLAYER
			gameSave = XMLSerialiser.DeserializeObject(PlayerPrefs.GetString(fileName), "GameSaveData") as GameSaveData;
#else
			// An existing save exists to load it in.
			gameSave = XMLSerialiser.DeserializeObject(XMLSerialiser.LoadXML(saveFilePath), "GameSaveData") as GameSaveData;
#endif

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
	public static void SaveHero(Hero hero, bool saveImmediately)
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

        if (saveImmediately)
        {
            // Immediately save change to file
            SaveGame();

            // Notify delegates so all UI can be updated.
            NotifyHeroSaveListChanged();
        }
	}

	/// <summary>
	/// Constructs a Hero out of HeroSaveData.
	/// </summary>
	/// <param name="data"> HeroSaveData to convert into a hero. </param>
	/// <returns> The loaded Hero. </returns>
	public static Hero LoadHero(HeroSaveData data)
	{
		return HeroFactory.CreateNewHero(data.classType);
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
#if UNITY_WEBPLAYER
		return (PlayerPrefs.HasKey(fileName));
#else
		FileInfo t = new FileInfo(path);

		return t.Exists;
#endif
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

    public static int SortListByDateAscending(HeroSaveData a, HeroSaveData b)
    {

        if (a.saveTime > b.saveTime) return -1;
        if (a.saveTime < b.saveTime) return 1;
        return 0;
    }

    public static int SortListByDateDescending(HeroSaveData a, HeroSaveData b)
    {

        if (a.saveTime > b.saveTime) return 1;
        if (a.saveTime < b.saveTime) return -1;
        return 0;
    }
}