using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class GameSaverTests : MonoBehaviour 
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

    void Start()
    {
        textControls = GameObject.Find("Controls").guiText;
        textControls.text += "\n\nSave: " + saveKey + "\n" + 
                            "LoadAll :" + loadAllKey + "\n" +
                            "ChooseSave: Up and Down";
		textControls.richText = true;

        textSaves = GameObject.Find("Saves").guiText;
        textSaves.text = "All Loaded Saves\n\n";
		textSaves.richText = true;

        textSave = GameObject.Find("Save").guiText;
        textSave.text = "Loaded Save\n\n";
		textSave.richText = true;
    }


	void Update () 
    {
        ProcessInput();

        if (loadedHeroSaveDatas != null)
        {
			textSaves.text = "All Loaded Saves\n\n";

			for (int i = 0; i < loadedHeroSaveDatas.heroSaves.Count; ++i)
			{
				if (i == selectedSave)
				{
					textSaves.text += "<b>";
				}

				textSaves.text += loadedHeroSaveDatas.heroSaves[i].ToString() + "\n\n";

				if (i == selectedSave)
				{
					textSaves.text += "</b>";
				}
			}
        }

		if (loadedSave != null)
		{
			textSave.text = "Loaded Save\n\n";
			textSave.text += loadedSave.ToString();
		}
		else
		{
			textSave.text = "Loaded Save\n\n";
		}
	}

    void ProcessInput()
    {
        if (Input.GetKeyUp(saveAllKey))
        {
            // Save
			GameSaver.SaveGame(loadedHeroSaveDatas);
        }

        else if (Input.GetKeyUp(loadAllKey))
        {
            // Load
            loadedHeroSaveDatas = GameSaver.LoadAllHeroSaves();
			Debug.Log("Load all");

			selectedSave = -1;
        }

        else if (Input.GetKeyUp(createSavesKey))
        {
            // Create Saves
            GameSaver.CreateTestSaves();
            Debug.Log("Saves Created");

			textControls.text = "Controls\n\n";
			textControls.text += "Save: " + saveKey + "\n" +
								"LoadAll :" + loadAllKey + "\n" +
								"ChooseSave: Up and Down";
			textControls.richText = true;

			textSaves = GameObject.Find("Saves").guiText;
			textSaves.text = "All Loaded Saves\n\n";
			textSaves.richText = true;

			textSave = GameObject.Find("Save").guiText;
			textSave.text = "Loaded Save\n\n";
			textSave.richText = true;

			selectedSave = -1;
        }

		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			if(loadedHeroSaveDatas != null)
			{
				selectedSave++;

				if (selectedSave >= loadedHeroSaveDatas.heroSaves.Count)
				{
					selectedSave = 0;
				}
			}
		}
		else if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			if (loadedHeroSaveDatas != null)
			{
				selectedSave--;

				if (selectedSave < 0)
				{
					selectedSave = loadedHeroSaveDatas.heroSaves.Count - 1;
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.Return))
		{
			if (loadedHeroSaveDatas != null)
			{
				loadedSave = loadedHeroSaveDatas.heroSaves[selectedSave];

			}
		}

		if(Input.GetKeyUp(KeyCode.Delete))
		{
			
			if (loadedHeroSaveDatas != null)
			{
				if (selectedSave > -1 && selectedSave < loadedHeroSaveDatas.heroSaves.Count)
				{

					if (loadedHeroSaveDatas.heroSaves[selectedSave] == loadedSave)
					{
						Debug.Log("same");
						loadedSave = null;
					}

					GameSaver.DeleteSlot(loadedHeroSaveDatas.heroSaves[selectedSave]);
					//loadedHeroSaveDatas.heroSaves.Remove();
				}
			}
		}

    }
}
