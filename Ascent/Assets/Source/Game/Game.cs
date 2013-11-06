using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	#region Fields

	public static Game Singleton;
	// Number of players
    public Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[3];

	// The player prefab
	public Transform PlayerPrefab;

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
		// Not used atm...
	}

    void Update()
    {
		// Not used atm...
    }

    private void CreatePlayers()
    {
        players = new List<Player>();

        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            Player newPlayer = player.GetComponent<Player>();
            newPlayer.PlayerID = i;
            players.Add(newPlayer);
            newPlayer.SetInputDevice(inputHandler.GetDevice(1));
            newPlayer.CreateHero(playerCharacterType[i]);
			
        }
    }
	
	#endregion

}
