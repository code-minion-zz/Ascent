using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
    public enum EGameState
    {
        MainMenu,
        Town,
        Tower,
		Loading,
    }

	#region Fields

	private static Game singleton;
	public static Game Singleton
	{
		get 
		{
			return singleton; 
		}
		private set { singleton = value; }
	}

	// Number of players
    public Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[3];
    public bool visualDebuggerPrefab = true;
    public string levelName;
    public GameObject Cameras;

	private List<Player> players;
    private Tower tower;

    private EGameState gameState;
    public EGameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

	private EGameState gameStateToLoad;
	
	#endregion	
	
	#region Properties

    public int NumberOfPlayers
    {
		get { return players.Count; }//playerCharacterType.Length; }
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

        gameState = initValues.initialGameState;

        Initialise();

		OnLevelWasLoaded(0);
	}

	public void Initialise()
	{
		OnEnable();

		DontDestroyOnLoad(gameObject);

		InputManager.Initialise();

		CreatePlayers();

		if (visualDebuggerPrefab)
		{
			Instantiate(Resources.Load("Prefabs/VisualDebugger"));
		}

		tower = GetComponent<Tower>();
	}

    void Update()
    {
		InputManager.Update();
    }

	// This is a helper function to create players with heroes at any stage of the game
    private void CreatePlayers()
	{
		players = new List<Player>();

		for (int i = 0; i < playerCharacterType.Length; ++i)
		{
			GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;
            player.transform.localScale = Vector3.one;

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

			newPlayer.CreateHero(playerCharacterType[i]);
		}
    }

	public void SetPlayers(List<Player> players)
	{
		this.players = players;
	}

	public void LoadLevel(string level, EGameState state)
	{
		// This state will be used to handle the initialisation of the new scene
		gameStateToLoad = state;
		gameState = EGameState.Loading;
		
		// The Loading screen will grab this string and then load the correct scene
		levelName = level;

		Application.LoadLevel("LoadingScreen");
	}

    public void OnLevelWasLoaded(int iLevelID)
    {
        // Only when coming from the loading screen.
		switch (gameState)
		{
			case EGameState.MainMenu:
				{

				}
				break;
			case EGameState.Tower:
				{
					tower.InitialiseFloor();
				}
				break;
			case EGameState.Town:
				{

				}
				break;
			case EGameState.Loading:
				{
					gameState = gameStateToLoad;
				}
				break;
		}
    }
	
	#endregion

}
