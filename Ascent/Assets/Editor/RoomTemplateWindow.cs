#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class RoomTemplateWindow : EditorWindow
{
	private string roomName;
	private int numberOfTilesX;
	private int numberOfTilesY;
	private RoomGeneration roomGenRef;

	public void Initialise(RoomGeneration generator)
	{
		roomGenRef = generator;
	}

	void OnGUI()
	{
		roomName = EditorGUILayout.TextField("Room Name", roomName);

		if (GUILayout.Button("11x11 (22x22)")) CreateRoom(11, 11);
		if (GUILayout.Button("11x9 (22x18)")) CreateRoom(11, 9);
		if (GUILayout.Button("11x7 (22x14)")) CreateRoom(11, 7);
		if (GUILayout.Button("11x5 (22x10)")) CreateRoom(11, 5);
		if (GUILayout.Button("11x3 (22x6)")) CreateRoom(11, 3);

		if (GUILayout.Button("9x9 (18x18)")) CreateRoom(9, 9);
		if (GUILayout.Button("9x7 (18x14) Standard")) CreateRoom(9, 7);
		if (GUILayout.Button("9x5 (18x10)")) CreateRoom(9, 5);
		if (GUILayout.Button("9x3 (18x6)")) CreateRoom(9, 3);

		if (GUILayout.Button("7x7 (14x14)")) CreateRoom(7, 7);
		if (GUILayout.Button("7x5 (14x10)")) CreateRoom(7, 5);
		if (GUILayout.Button("7x3 (14x6)")) CreateRoom(7, 3);

		if (GUILayout.Button("5x5 (10x10)")) CreateRoom(5, 5);
		if (GUILayout.Button("5x3 (10x6)")) CreateRoom(5, 3);

		if (GUILayout.Button("3x3 (6x6)")) CreateRoom(3, 3);
	}

	private void CreateRoom(int numberOfTilesX, int numberOfTilesY)
	{
		//RoomProperties roomProperties = roomGenRef.CreateNewRoom(numberOfTilesX * 2, numberOfTilesY * 2, roomName);
        RoomProperties room = roomGenRef.CreateNewRoom(RoomConnectionType.Empty, numberOfTilesX, numberOfTilesY);
        room.Name = roomName;
        room.ConstructRoom();
        roomGenRef.PlaceGroundTiles(room);
        roomGenRef.PlaceWalls(room);
		this.Close();
	}
}
#endif