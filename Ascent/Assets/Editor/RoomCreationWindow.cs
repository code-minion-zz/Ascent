#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class RoomCreationWindow : EditorWindow
{
    private string roomName;
    private int numberOfTilesX;
    private int numberOfTilesY;
    private RoomGeneration roomGenRef;
    private bool buildWalls;
    private int tileSize;

    public void Initialise(RoomGeneration generator)
    {
        roomGenRef = generator;
        roomName = "New Room";
        tileSize = 2;
    }

    void OnGUI()
    {
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        buildWalls = EditorGUILayout.Toggle("Build walls", buildWalls);
        tileSize = EditorGUILayout.IntField("Tile size", tileSize);
        numberOfTilesX = EditorGUILayout.IntField("Number Of Tiles X", numberOfTilesX);
        numberOfTilesY = EditorGUILayout.IntField("Number Of Tiles Y", numberOfTilesY);

        if (GUILayout.Button("Create"))
        {
            RoomProperties room = roomGenRef.CreateNewRoom(RoomConnectionType.Empty, numberOfTilesX, numberOfTilesY, tileSize);
            room.Name = roomName;
            room.ConstructRoom();
            roomGenRef.PlaceGroundTiles(room);

            if (buildWalls == true)
            {
                roomGenRef.PlaceWalls(room);
            }

            this.Close();
        }
    }
}
#endif