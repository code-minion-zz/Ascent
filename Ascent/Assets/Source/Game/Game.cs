// Desc: Persistent Singleton that manages the players and game states

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	public enum GameState
	{
		NONE,

		MainMenu,
		CharSelect,
		Town,
		Tower,

		MAX,
	}

    #region Fields

    public static Game Singleton;
    // Number of players
    private List<Player> players;
    private InputHandler inputHandler;
    private Floor floor;
	private GameState state;
	public int targetFrameRate = 60;

    #endregion

    #region Properties

    public int NumberOfPlayers
    {
        get { return players.Count; }
    }

    public InputHandler InputHandler
    {
        get { return inputHandler; }
    }

    public List<Player> Players
    {
        get { return players; }
    }

    public Floor Floor
    {
        get { return floor; }
    }

    #endregion

    #region Initialization

    public void OnEnable()
    {
        if (Singleton == null)
            Singleton = this;
    }


	// Initialise the game through GameInitialiser
    public void Initialise(GameInitialiser.GameInitialisationValues initValues)
    {
        DontDestroyOnLoad(gameObject);

        Character.EHeroClass[] playerCharacterType = initValues.playerCharacterType;
		targetFrameRate = initValues.targetFrameRate;
        Application.targetFrameRate = initValues.targetFrameRate;

        // Add monoehaviour components
        inputHandler = gameObject.GetComponent("InputHandler") as InputHandler;

        CreatePlayers(playerCharacterType);

        if (initValues.useVisualDebugger)
        {
            Instantiate(Resources.Load("Prefabs/VisualDebugger"));
        }

        floor = GetComponent<Floor>();
    }

	// Initialise the game the normal way
	public void Initialise()
	{
		DontDestroyOnLoad(gameObject);

		Application.targetFrameRate = targetFrameRate;

		inputHandler = gameObject.GetComponent("InputHandler") as InputHandler;

		InControl.InputManager.OnDeviceAttached += OnDeviceAttached;
		InControl.InputManager.OnDeviceDetached += OnDeviceDetached;

		players = new List<Player>();
	}

    // This function is always called immediately when Instantiated and is called before the Start() function
    // The game should add all components here so that they are ready for use when other objects require them.

    void Awake()
    {
		Initialise();
    }

	void OnDestroy()
	{
		InControl.InputManager.OnDeviceAttached -= OnDeviceAttached;
		InControl.InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

    // Use this for initialization
    void Start()
    {
        // Not used atm...
    }

    void Update()
    {
        // Not used atm...
    }

	public void CreatePlayer()
	{
		if(players.Count < 3)
		{
			GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            Player newPlayer = player.GetComponent<Player>();

			players.Add(newPlayer);
		}
	}

	public void RemovePlayer(Player player)
	{
		players.Remove(player);
		Destroy(player.gameObject);
	}

    private void CreatePlayers(Character.EHeroClass[] playerCharacterType)
    {
        players = new List<Player>();

        for (int i = 0; i < NumberOfPlayers; ++i)
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            Player newPlayer = player.GetComponent<Player>();
            newPlayer.PlayerID = i;
            players.Add(newPlayer);

            int iDevice = i;

            if (inputHandler.NumberOfDevices > 1)
            {
                iDevice += 1;
            }
			
            InControl.InputDevice device = inputHandler.GetDevice(iDevice);
            if (device == null)
            {
                device = inputHandler.GetDevice(0);
            }

            newPlayer.SetInputDevice(device);

            //newPlayer.SetInputDevice(inputHandler.GetDevice(i));


            newPlayer.CreateHero(playerCharacterType[i]);
        }
    }

	public void OnDeviceAttached(InControl.InputDevice device)
	{
		// Request player/s press start/enter on the device
	}

	public void OnDeviceDetached(InControl.InputDevice device)
	{
		// If on floor or in menus - pause game and request player replug in the controller. 
		// Has a timer that will expire so other players may continue.
		// Unbind it
	}

    #endregion

}
