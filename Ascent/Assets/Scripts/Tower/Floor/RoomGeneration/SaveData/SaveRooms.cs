using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveRooms
{
    /// <summary>
    /// Saves a room to the file path specified.
    /// </summary>
    /// <param name="room">The room properties to save.</param>
    /// <param name="filePath">The file path for the room.</param>
    public void SaveRoom(RoomProperties room, string filePath)
    {
        RoomSaves newRoom = new RoomSaves();
        newRoom.saves.Add(room);
        XMLSerialiser.SerializeObjectBin(filePath, newRoom);
    }

    /// <summary>
    /// Loads a room from the file path specified.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public RoomProperties LoadRoom(string filePath, bool isResource)
    {
        RoomSaves savedRoom = (RoomSaves)XMLSerialiser.DeserializeObjectBin(filePath, isResource);
        return (savedRoom.saves[0]);
    }
}
