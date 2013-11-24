using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	#region Fields

	private static Game singleton;
	public static Game Singleton
	{
		get 
		{
			//if(singleton == null)
			//{
			//    Debug.Log("no game");	
			//}
			return singleton; 
		}
		private set { singleton = value; }
	}

	// Number of players
    public Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[3];
    public bool visualDebuggerPrefab = true;

	private List<Player> players;
    private Tower tower;

	public string levelName;
	private bool loadingLevel = false;

	private bool initialised = false;
	
	#endregion	
	
	#region Properties

    public int NumberOfPlayers
    {
        get { return playerCharacterType.Length; }
    }

    public List<Player> Players
    {
        get { return players; }
    }

	public Tower Tower
    {
        get { return tower; }
    }
	
	#endregion
	
	#region Initialization

	public void OnEnable()
	{
		if (Singleton == null)
			Singleton = this;
	}

	public void Initialise(GameInitialiser.GameInitialisationValues initValues)
	{
		playerCharacterType = initValues.playerCharacterType;

		Application.targetFrameRate = initValues.targetFrameRate;

		visualDebuggerPrefab = initValues.useVisualDebugger;

		Initialise();
	}

	public void Initialise()
	{
		OnEnable();

		DontDestroyOnLoad(gameObject);
		// Add monoehaviour components

		InputManager.Initialise();

		CreatePlayers();

		if (visualDebuggerPrefab)
		{
			Instantiate(Resources.Load("Prefabs/VisualDebugger"));
		}

		tower = GetComponent<Tower>();
	}

    // This function is always called immediately when Instantiated and is called before the Start() function
    // The game should add all components here so that they are ready for use when other objects require them.
    void Awake()
    {
		if (GameObject.Find("Game Initialiser") == null)
		{
			Initialise();
		}
    }
	
	// Use this for initialization
	void Start () 
	{
		// Not used atm...
	}

    void Update()
    {
		InputManager.Update();
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

			int iDevice = i;

			if (InputManager.Devices.Count > 1)
			{
				iDevice += 1;
			}

			InputDevice device = InputManager.GetDevice(iDevice);
			if (device == null)
			{
				device = InputManager.GetDevice(0);
			}

			newPlayer.BindInputDevice(device);

			//newPlayer.SetInputDevice(InputManager.GetDevice(i));


			newPlayer.CreateHero(playerCharacterType[i]);
		}
    }

	public void SetPlayers(List<Player> players)
	{
		this.players = players;
	}

	public void LoadLevel(string level)
	{
		levelName = level;
		Application.LoadLevel("LoadingScreen");
	}
	
	#endregion

}
