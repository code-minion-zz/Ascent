using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles generation of a room for use in the floor generation.
/// </summary>
/// 
public class RoomGeneration 
{
	private GameObject floorObject;
	private GameObject wallObject;
	private GameObject wallCorner;
	private GameObject wallWindow;
	private GameObject doorObject;

	public RoomGeneration()
	{
		floorObject = Resources.Load("Prefabs/RoomWalls/GroundTile_2x2") as GameObject;
		wallObject = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
		wallCorner = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
		wallWindow = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
		doorObject = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;
	}

	/// <summary>
	/// Creates a new room and intializes variables.
	/// </summary>
	/// <returns>The new room.</returns>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// <param name="name">Name.</param>
	public RoomProperties CreateNewRoom(int width, int height, string name)
	{
		GameObject roomGo = new GameObject(name);
		Room room = roomGo.AddComponent<Room>();
		
		// Add necessary nodes.
		room.tag = "RoomRoot";
		GameObject envGo = room.AddNewParentCategory("Environment", LayerMask.NameToLayer("Environment"));
		GameObject doorGo = room.AddSubParent("Doors", envGo, LayerMask.NameToLayer("Environment")) as GameObject;
		doorGo.AddComponent<Doors>();
		room.AddSubParent("Walls", envGo, LayerMask.NameToLayer("Environment"));
		
		room.AddNewParentCategory("Monsters", LayerMask.NameToLayer("Monster"));
		room.AddNewParentCategory("Items", LayerMask.NameToLayer("Items"));
		room.AddNewParentCategory("Lights", LayerMask.NameToLayer("Default"));
		
		room.Initialise();
		
		// Handle creation of the ground tiles.
		RoomProperties newRoom = new RoomProperties(room);
		newRoom.SetRoomTiles(width, height);
		
		int numberOfTilesX = (int)(width * 0.5f);
		int numberOfTilesY = (int)(height * 0.5f);
		
		// Create the floor tiles and positions.
		for (int i = 0; i < numberOfTilesX; ++i)
		{
			for (int j = 0; j < numberOfTilesY; ++j)
			{
				// Create the floor.
				GameObject floorGo = GameObject.Instantiate(floorObject, Vector3.zero, floorObject.transform.rotation) as GameObject;
				float tileSizeX = floorObject.transform.localScale.x;
				float tileSizeY = floorObject.transform.localScale.y;
				float halfTileX = 1.0f;
				float halfTileY = 1.0f;
				floorGo.transform.parent = room.GetNodeByLayer("Environment").transform;
				Vector3 tilePosition = new Vector3(-(width * 0.5f) + halfTileX + (i * tileSizeX), 0.0f, -(height * 0.5f) + halfTileY + (j * tileSizeY));
				floorGo.transform.position = tilePosition;
				floorGo.name = "GroundTile[" + i + ", " + j + "]";
				
				newRoom.RoomTiles[i,j].Position = tilePosition;
			}
		}
		
		// Apply the new dimensions to the navMesh.
		room.NavMesh.transform.localScale = new Vector3(width - 1.0f, height - 1.0f, 0.0f);

		// TODO: Fix the camera setup for this room.
		room.minCamera.x = -width * 0.15f;
		room.minCamera.z = -height * 0.15f;
		room.maxCamera.z = height * 0.15f;
		room.maxCamera.x = width * 0.15f;
		
		return newRoom;
	}
}