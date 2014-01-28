using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class AscentGameSaverTest : MonoBehaviour
{
	GUIText textControls;
	GUIText textSaves;
	GUIText textSave;

	int selectedSave = -1;

	List<HeroSaveData> heroSaves;

	void Start()
	{
		AscentGameSaver.LoadGame();
		AscentGameSaver.OnHeroSaveListChangedEvent += OnListUpdate;

		textControls = GameObject.Find("Controls").guiText;
		textControls.text += "\nF1: " + "Create Save" + "\n" +
							"Del :" + "Delete" + "\n" +
							"Ret :" + "Load" + "\n" +
							"Up and Down";
		textControls.richText = true;

		OnListUpdate();
	}

	void OnDestroy()
	{
		AscentGameSaver.OnHeroSaveListChangedEvent -= OnListUpdate;
	}


	void Update()
	{
		// Create hero
		// This can be used in CharSelectScreen when a new character is made.
		// i.e. Save when all players enter the game.
		if(Input.GetKeyUp(KeyCode.F1))
		{
			Warrior war = HeroFactory.CreateNewHero(Hero.EHeroClass.Warrior) as Warrior;
			war.Initialise(null, null);
			AscentGameSaver.CreateNewHeroSave(war);
			Destroy(war.gameObject);
		}

		// Delete highlighted hero
		// This can be used in CharSelectScreen to delete a save from the list of saves.
		if (Input.GetKeyUp(KeyCode.Delete))
		{
			if (selectedSave != -1)
			{
				AscentGameSaver.DeleteHeroSave(heroSaves[selectedSave]);
				selectedSave = -1;
			}
		}

		// Load highlighted hero
		// This can be used in CharSelectScreen to load a save from the list of saves.
		// The input device just needs to be given with the initialisation function.
		if (Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			if (selectedSave != -1)
			{
				// Try get the Xbox controller
				InputDevice device = InputManager.GetDevice(1);
				if(device == null)
				{
					// else get the keyboard
					device = InputManager.GetDevice(0);
				}

				Hero LoadedHero = AscentGameSaver.LoadHero(heroSaves[selectedSave]);
				LoadedHero.Initialise(device, heroSaves[selectedSave]);
				LoadedHero.HeroController.CanUseInput = true;
				selectedSave = -1;
			}
		}


		// Through menu
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			--selectedSave;

			if (selectedSave < 0)
			{
				selectedSave = heroSaves.Count - 1;
			}
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			++selectedSave;

			if (selectedSave >= heroSaves.Count)
			{
				selectedSave = 0;
			}
		}
	}

	void OnGUI()
	{
		GUI.TextArea(new Rect(100, 200, 100, 25), "Selected: " + selectedSave);
	}

	public void OnListUpdate()
	{
		heroSaves = AscentGameSaver.SaveData.heroSaves;

		heroSaves.Sort(SortListByDateAscending);

		textSaves = GameObject.Find("Saves").guiText;
		textSaves.text = "All Loaded Saves\n\n";
		textSaves.richText = true;

		int iCount = 0;
		foreach (HeroSaveData hero in AscentGameSaver.SaveData.heroSaves)
		{
			textSaves.text += iCount++ + " " + hero.ToString() + "\n \n";
		}


		//textSave = GameObject.Find("Save").guiText;
		//textSave.text = "Loaded Save\n\n";
		//textSave.richText = true;
	}

	public int SortListByDateAscending(HeroSaveData a, HeroSaveData b)
	{

		if (a.saveTime > b.saveTime) return -1;
		if (a.saveTime < b.saveTime) return 1;
		return 0;
	}

	public int SortListByDateDescending(HeroSaveData a, HeroSaveData b)
	{

		if (a.saveTime > b.saveTime) return 1;
		if (a.saveTime < b.saveTime) return -1;
		return 0;
	}

}
