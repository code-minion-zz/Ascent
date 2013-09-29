using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	#region Fields
	// Number of players
	public int NumberOfPlayers = 3;
	// The player prefab
	public Transform PlayerPrefab;
	// The camera prefab
	public Transform CameraPrefab;
	// The door transform
	public Transform DoorPrefab;
	// Camera offset
	public float cameraOffset = 6.0f;
	// List of player objects
	private List<Player> players;
	// The input handler which players will use.
	private InputHandler inputHandler;
	
	#endregion	
	
	#region Properties
	
	public InputHandler InputHandler
	{
		get { return inputHandler; }
	}

    public List<Player> Players
    {
        get { return players; }
    }
	
	public GameObject GameObject
	{
		get { return transform.gameObject; }
	}
	
	#endregion
	
	#region Initialization
	// Use this for initialization
	void Start () 
	{
		// Add monoehaviour components
		inputHandler = (InputHandler)GameObject.AddComponent("InputHandler");
		
		// Initialize the list of players.
		players = new List<Player>();
		
		// Create the players
		for (int i = 0; i < NumberOfPlayers; ++i)
		{
			// Setup the player and their positions 
			// Setup the spawning point
			Vector3 pos = new Vector3(Random.Range(0, 5), 1, Random.Range(0, 5));
			PlayerPrefab = (Transform)Instantiate(PlayerPrefab, pos, Quaternion.identity);
			// Get the Player class from the prefab component
			Player newPlayer = PlayerPrefab.GetComponent<Player>();
			newPlayer.PlayerID = i;
			// Add the player to the list.
			players.Add(newPlayer);
		}
		
		// Create the camera
		CameraPrefab = (Transform)Instantiate(CameraPrefab);
		 
		// Setup doors
		SetupDoors();
	}
	
	private void SetupDoors()
	{
		// Instantiate door.
		//DoorPrefab = (Transform)Instantiate(DoorPrefab);
		
	}
	
	#endregion
	
	#region Update
	
	// Update is called once per frame
	void Update () 
	{
		// Update Camera
		UpdateCamPos();
	}
	
	void UpdateCamPos()
	{
		// Ulter position of the camera to center on the players
		Vector3 totalVector = new Vector3();
		
		// Add up all the vectors
		foreach (Player player in players)
		{
			if (player != null)
				totalVector += player.Position;
		}
		
		// Calculate camera position based off players
		float x =  totalVector.x / players.Count;
		float y = CameraPrefab.position.y;
		float z = (totalVector.z / players.Count) - cameraOffset;
		
		// Set the position of our camera.
		CameraPrefab.position = Vector3.Lerp(CameraPrefab.position, new Vector3(x, y, z), 1.0f * Time.deltaTime);
	}
	
	#endregion
}
