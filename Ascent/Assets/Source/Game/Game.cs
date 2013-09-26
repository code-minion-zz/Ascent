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
		
		// Set the position of our camera.
		CameraPrefab.position = new Vector3(totalVector.x / players.Count, CameraPrefab.position.y, totalVector.z / players.Count);		
	}
	
	#endregion
}
