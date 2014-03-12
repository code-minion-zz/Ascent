#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoomTemplateWindow : EditorWindow
{
	private string roomName;
	private int numberOfTilesX;
	private int numberOfTilesY;
	private RoomGeneration roomGenRef;
    private static List<string> templateSizes = new List<string>();
    private bool rotated;
    private bool buildWalls;
    private bool populateRandomMisc;
    private bool populateMonsters;
    private int tileSize;
    private int selectedTemplate;

	public void Initialise(RoomGeneration generator)
	{
		roomGenRef = generator;
        roomName = "New Room";
        buildWalls = true;
        tileSize = 2;

        templateSizes.Add("11x11");
        templateSizes.Add("11x9");
        templateSizes.Add("11x7");
        templateSizes.Add("11x5");
        templateSizes.Add("11x3");
        templateSizes.Add("9x9");
        templateSizes.Add("9x7");
        templateSizes.Add("9x5");
        templateSizes.Add("9x3");
        templateSizes.Add("7x7");
        templateSizes.Add("7x5");
        templateSizes.Add("7x3");
        templateSizes.Add("5x5");
        templateSizes.Add("5x3");
        templateSizes.Add("3x3");
	}

	void OnGUI()
	{
		roomName = EditorGUILayout.TextField("Room name", roomName);
        rotated = EditorGUILayout.Toggle("Flip width/height", rotated);
        buildWalls = EditorGUILayout.Toggle("Build walls", buildWalls);
        populateRandomMisc = EditorGUILayout.Toggle("Populate misc objects", populateRandomMisc);
        populateMonsters = EditorGUILayout.Toggle("Populate monsters", populateMonsters);
        tileSize = EditorGUILayout.IntField("Tile size", tileSize);

        bool createRoom = false;
        int width = 0;
        int height = 0;

        selectedTemplate = EditorGUILayout.Popup("Choose size", selectedTemplate, templateSizes.ToArray());

        if (templateSizes[selectedTemplate] != null)
        {
            string[] parts = templateSizes[selectedTemplate].Split("x, ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int tilesX = Int32.Parse(parts[0]);
            int tilesY = Int32.Parse(parts[1]);

            string buttonText = "Create room";

            if (rotated == false)
            {
                buttonText = "Create Room " + tilesX + "x" + tilesY + " (" + tilesX * tileSize +
                "x" + tilesY * tileSize + ")";
            }
            else
            {
                buttonText = "Create Room " + tilesY + "x" + tilesX + " (" + tilesY * tileSize +
                "x" + tilesX * tileSize + ")";
            }

            if (GUILayout.Button(buttonText))
            {
                width = tilesX;
                height = tilesY;
                createRoom = true;
            }
        }

        if (createRoom)
        {
            if (rotated)
            {
                int temp = height;
                height = width;
                width = temp;
            }

            CreateRoom(width, height);
        }
	}

	private void CreateRoom(int numberOfTilesX, int numberOfTilesY)
	{
        RoomProperties room = roomGenRef.CreateNewRoom(RoomConnectionType.Empty, numberOfTilesX, numberOfTilesY, tileSize);
        room.Name = roomName;
        room.ConstructRoom();
        roomGenRef.PlaceGroundTiles(room);

        if (buildWalls == true)
        {
            roomGenRef.PlaceWalls(room);
        }

        if (populateRandomMisc == true)
        {
            roomGenRef.PopulateMiscObjects(room);
        }

        if (populateMonsters == true)
        {
            roomGenRef.PopulateMonsters(1, room, Rarity.many);
        }

		this.Close();
	}
}
#endif