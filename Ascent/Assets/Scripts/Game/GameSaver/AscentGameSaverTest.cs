using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class AscentGameSaverTest : MonoBehaviour
{
	HeroSaveDataList loadedHeroSaveDatas;
	HeroSaveData loadedSave;

	GUIText textControls;
	GUIText textSaves;
	GUIText textSave;

	const KeyCode saveKey = KeyCode.F5;
	const KeyCode saveAllKey = KeyCode.F5;
	const KeyCode loadAllKey = KeyCode.F1;
	const KeyCode loadKey = KeyCode.F2;
	const KeyCode createSavesKey = KeyCode.F10;

	int selectedSave = -1;

	List<HeroSaveData> heroSaves;

	void Start()
	{
		AscentGameSaver.LoadGameDataSave();
		AscentGameSaver.OnHeroSaveListChangedEvent += OnListUpdate;

		textControls = GameObject.Find("Controls").guiText;
		textControls.text += "\n\nSave: " + saveKey + "\n" +
							"LoadAll :" + loadAllKey + "\n" +
							"CreateTestSaves :" + createSavesKey + "\n" +
							"ChooseSave: Up and Down";
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
		if(Input.GetKeyUp(KeyCode.F1))
		{
			Warrior war = HeroFactory.CreateNewHero(Hero.EHeroClass.Warrior) as Warrior;
			war.Initialise(null, null);
			AscentGameSaver.CreateNewHeroSave(war);
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
		else if(Input.GetKeyUp(KeyCode.Delete))
		{
			if (selectedSave != -1)
			{
				AscentGameSaver.DeleteHeroSave(heroSaves[selectedSave]);
				selectedSave = -1;
			}
		}
	}

	void OnGUI()
	{
		GUI.TextArea(new Rect(100, 200, 100, 25), "Selected: " + selectedSave);
	}

	public void OnListUpdate()
	{
		Debug.Log("UPDATED");
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
