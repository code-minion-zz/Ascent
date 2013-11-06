using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	#region Fields

	public static Game Singleton;
	// Number of players
    public Character.ECharacterClass[] playerCharacterType = new Character.ECharacterClass[3];

    public bool visualDebuggerPrefab = true;

	// List of player objects
	private List<Player> players;
	// The input handler which players will use.
	private InputHandler inputHandler;
	
	#endregion	
	
	#region Properties

    public int NumberOfPlayers
    {
        get { return playerCharacterType.Length; }
    }
	
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

	public void OnEnable()
	{
		if (Singleton == null)
			Singleton = this;
	}

    // This function is always called immediately when Instantiated and is called before the Start() function
    // The game should add all components here so that they are ready for use when other objects require them.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // Add monoehaviour components
        inputHandler = gameObject.AddComponent("InputHandler") as InputHandler;

        CreatePlayers();

        if (visualDebuggerPrefab)
        {
            Instantiate(Resources.Load("Prefabs/VisualDebugger"));
        }
    }
	
	// Use this for initialization
	void Start () 
	{

	}

    void Update()
    {
        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            players[i].Update();
        }
    }

    private void CreatePlayers()
    {
        players = new List<Player>();

        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            Player newPlayer = new Player();
            newPlayer.PlayerID = i;
            players.Add(newPlayer);
            newPlayer.CreateHero(playerCharacterType[i]);
        }
    }
	
	#endregion

}
