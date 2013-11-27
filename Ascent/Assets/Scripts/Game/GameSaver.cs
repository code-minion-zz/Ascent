using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public static class GameSaver
{
    public const int maxSlots = 10;

    public static List<HeroSave> LoadAllHeroSaves()
    {
        List<HeroSave> heroSaves = new List<HeroSave>();

        // TODO: Open XML save file
        // Populate list with all heroSaves
        // return the list

        return heroSaves;
    }

    public static void DeleteSlot(HeroSave hero)
    {
        // Check if there is a hero in the slot
        // Delete that hero
    }

    public static void CreateSlot(HeroSave hero)
    {
        // Check if there is an empty slot else inform the players that something needs to be deleted.
        // put new hero in
    }

    public static HeroSave LoadSlot()
    {
        return null;
    }

    public static void SaveGame()
    {
        SimpleJSON.JSONNode node = new SimpleJSON.JSONNode();
        node.SaveToFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/playersaves.json");

        //// Grab binary formatter
        //BinaryFormatter bin = new BinaryFormatter();
        
        //// Create save file
        //FileStream file = File.Create(Application.persistentDataPath + "/playersaves.dat");

        // Save the heros
        List<Player> players = Game.Singleton.Players;

        foreach (Player p in players)
        {
             //TODO: Save to XML file.
             //Save p.Hero into a file
             //Save into tower progression
             //Save statistics etc...
             //Save inventory
             //Hero slot

            //bin.Serialize(file, p);
        }
    }

    public static void CreateTestSaves()
    {
        //JSONNode saveNode = JSON.Parse(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/playersaves.json");
        JSONClass saveNode = new JSONClass();
        
        //Debug.Log(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/playersaves.json");

        saveNode["Hero"] = "Mana";

        Debug.Log(saveNode["Hero"]);

        //JSONData data = new JSONData("Mana");

        //saveNode.Add("Hero", data);

        //string hello = saveNode["Hero"];

        //Debug.Log(hello);

        saveNode.SaveToFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/playersaves.json");
    }
}