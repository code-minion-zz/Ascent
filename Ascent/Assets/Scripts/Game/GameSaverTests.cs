using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSaverTests : MonoBehaviour 
{
    List<HeroSave> loadedHeroSaves;
    HeroSave loadedSave;

    GUIText textControls;
    GUIText textSaves;
    GUIText textSave;

    const KeyCode saveKey = KeyCode.F5;
    const KeyCode loadAllKey = KeyCode.F1;
    const KeyCode loadKey = KeyCode.F2;
    const KeyCode createSavesKey = KeyCode.F10;

    void Start()
    {
        textControls = GameObject.Find("Controls").guiText;
        textControls.text += "\n\nSave: " + saveKey + "\n" + 
                            "LoadAll :" + loadAllKey + "\n" +
                            "LoadCur :" + loadKey + "\n" +
                            "ChooseSave: Up and Down";

        textSaves = GameObject.Find("Saves").guiText;
        textSaves.text = "All Loaded Saves\n\n";

        textSave = GameObject.Find("Save").guiText;
        textSave.text = "Loaded Save\n\n";
    }

	void Update () 
    {
        ProcessInput();

        if (loadedHeroSaves != null)
        {
            // draw these out 
        }

        if (loadedSave != null)
        {
            // draw this out
        }
	}

    void ProcessInput()
    {
        if (Input.GetKeyUp(saveKey))
        {
            // Save

            GameSaver.SaveGame();
        }

        else if (Input.GetKeyUp(loadAllKey))
        {
            // Load
            loadedHeroSaves = GameSaver.LoadAllHeroSaves();
        }

        else if (Input.GetKeyUp(loadKey))
        {
            // Load
            GameSaver.LoadSlot();
        }

        else if (Input.GetKeyUp(createSavesKey))
        {
            // Create Saves
            GameSaver.CreateTestSaves();
            Debug.Log("Saves Created");
        }

    }
}
