using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
    public delegate void SceneLoaded();
    public SceneLoaded OnSceneLoadedEvent;

    public enum EGameState
    {
        MainMenu,
        Town,
        Tower,
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

	private bool loadingLevel = false;
	private bool initialised = false;

    private EGameState gameState;
    public EGameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }
	
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
        }


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

    public void OnSceneLoaded()
    {
        if (OnSceneLoadedEvent != null)
        {
            OnSceneLoadedEvent.Invoke();
        }
    }

    public void OnLevelWasLoaded(int iLevelID)
    {
        Debug.Log("Loaded");
        OnSceneLoaded();

        // Only when coming from the loading screen.
        // Check which state we heading to. 
        // Call the appropriate OnSceneLoaded function.
        // 13th
    }
	
	#endregion

}
