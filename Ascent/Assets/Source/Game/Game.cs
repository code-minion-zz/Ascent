using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	#region Fields
	
	public static Game Singleton;
	// Number of players
	public int NumberOfPlayers = 3;
	// The player prefab
	public Transform PlayerPrefab;
	// The camera prefab
	public Transform CameraPrefab;
	// Camera offset
	public float cameraOffset = 6.0f;

    public bool visualDebuggerPrefab = true;

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

    public Camera MainCamera
    {
        get { return CameraPrefab.camera; }
    }
	
	#endregion
	
	#region Initialization
	public void OnEnable()
	{
		if (Singleton == null)
			Singleton = this;
	}

    // This function is always called immediately when Instantiated and is called before the Start() function
    // The game should add all components here so that they are ready for use when other objects require them.
    void Awake()
    {
        // Add monoehaviour components
        inputHandler = GameObject.AddComponent("InputHandler") as InputHandler;
    }
	
	// Use this for initialization
	void Start () 
	{
        CreatePlayers();
		
		// Create the camera
		CameraPrefab = Instantiate(CameraPrefab) as Transform;

        if (visualDebuggerPrefab)
        {
            Instantiate(Resources.Load("Prefabs/VisualDebugger"));
        }
	}

    private void CreatePlayers()
    {
        // Initialize the list of players.
        players = new List<Player>();

        GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
        //if (startPoints.Length != 3)
        //{
        //    Debug.LogError("Need three starting points");
        //}

        // Create the players
        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            // Setup the player and their positions 
            // Setup the spawning point
            Vector3 pos = startPoints[i].transform.position;
            PlayerPrefab = Instantiate(PlayerPrefab, pos, Quaternion.identity) as Transform;
            // Get the Player class from the prefab component
            Player newPlayer = PlayerPrefab.GetComponent<Player>();
            newPlayer.PlayerID = i;
            newPlayer.name = "Player " + i;
            // Add the player to the list.
            players.Add(newPlayer);
        }
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
        Vector3 totalVector = Vector3.zero;

        // Add up all the vectors
        foreach (Player player in players)
        {
            if (player != null)
            {
                totalVector += player.Position;
            }
        }

        // Calculate camera position based off players
        float x = totalVector.x / players.Count;
        float y = CameraPrefab.position.y;
        float z = (totalVector.z / players.Count) - cameraOffset;

        Vector3 newVector = new Vector3(x, y, z);

        // Set the position of our camera.
        CameraPrefab.position = Vector3.Lerp(CameraPrefab.position, newVector, 2.0f * Time.deltaTime);

	}
	
	#endregion
}
