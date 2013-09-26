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
	
	#endregion
	
	#region Initialization
	// Use this for initialization
	void Start () 
	{
		// Add monoehaviour components
		inputHandler = (InputHandler)transform.gameObject.AddComponent("InputHandler");
		
		// Initialize the list of players.
		players = new List<Player>();
		
		// Create the players
		for (int i = 0; i < NumberOfPlayers; ++i)
		{
			// Setup the player and their positions 
			Player newPlayer = new Player(i);
			// Setup the spawning point
			Vector3 pos = new Vector3(Random.Range(0, 5), 1, Random.Range(0, 5));
			// The player instance shall have a cloned instance of the player prefab
			newPlayer.ObjectTransform = (Transform)Instantiate(PlayerPrefab, pos, Quaternion.identity);			
			newPlayer.Start();
			
			// Add the player to the list.
			players.Add(newPlayer);
		}
		
		// Create the camera
		CameraPrefab = (Transform)Instantiate(CameraPrefab);
	}
	
	#endregion
	
	#region Update
	
	// Update is called once per frame
	void Update () 
	{
		// Update Camera
		UpdateCamPos();
		// Update player
		foreach (Player player in players)
		{
			player.Update();
			
		}
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
		CameraPrefab.position = new Vector3(x, y, z);
		
	}
	
	#endregion
}
