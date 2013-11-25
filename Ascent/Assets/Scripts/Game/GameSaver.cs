﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GameSaver
{
    const int maxSlots = 10;

    static List<HeroSave> LoadAllHeroSaves()
    {
        List<HeroSave> heroSaves = new List<HeroSave>();

        // TODO: Open XML save file
        // Populate list with all heroSaves
        // return the list

        return heroSaves;
    }

    static void DeleteSlot(HeroSave hero)
    {
        // Check if there is a hero in the slot
        // Delete that hero
    }

    static void CreateSlot(HeroSave hero)
    {
        // Check if there is an empty slot else inform the players that something needs to be deleted.
        // put new hero in
    }

    static void SaveGame()
    {
        // Grab binary formatter
        BinaryFormatter bin = new BinaryFormatter();

        // Create save file
        FileStream file = File.Create(Application.persistentDataPath + "/playersaves.dat");

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

            bin.Serialize(file, p);
        }
    }
}