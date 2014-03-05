using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
#if UNITY_WEBPLAYER
	public const float KfWebVersion = 0.1f;
#endif

	// GameTest Values
	public Game.EGameState testState = Game.EGameState.Tower;
	public Character.EHeroClass[] testCharacters;
	public int targetFrameRate = 60;

    public enum EGameState
    {
		None = -1, 
        MainMenu,
		HeroSelect,
        City,
		FloorSummary,
        TestTower,
		Loading,
		Tower
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
    private Character.EHeroClass[] playerCharacterType = new Character.EHeroClass[3];
    private string levelName;
	public string LevelName
	{
		get { return levelName; }
		set { levelName = value; }
	}

	private List<Player> players;
    private Tower tower;

    private EGameState gameState;
    public EGameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

	//private bool firstState = true;

	public bool InTower
	{
		get { return gameState == EGameState.Tower || gameState == EGameState.TestTower; }
	}

	private EGameState gameStateToLoad;

    private EffectFactory effectFactory;

    public EffectFactory EffectFactory
    {
        get 
        {
            if (effectFactory == null)
            {
                effectFactory = this.gameObject.AddComponent<EffectFactory>();
                return effectFactory;
            }

            return effectFactory; 
        }
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

    public int AlivePlayerCount
    {
        get 
        {
            int count = 0;
            foreach (Player p in players)
            {
                if (!p.Hero.IsDead)
                {
                    ++count;
                }
            }

            return count;
        }
    }

	public Tower Tower
    {
        get { return tower; }
    }

	private bool isWide;
	public bool IsWideScreen
	{
		get { return isWide; }
	}
	
	#endregion
	
	#region Initialization


    static bool initialised = false;
	public void OnEnable()
	{
		if (Singleton == null)
		{
			Singleton = this;
		}
		
        if (!initialised)
        {
            Initialise();
        }
        else
        {
            GameObject theGameObject = GameObject.Find("Game");
            if (theGameObject != null)
            {
                Debug.Log("Game already exists no need to initialise a new one.");
                // Get rid of the game initialiser
                Destroy(this.gameObject);
                return;
            }

            if (theGameObject == null)
            {
                theGameObject = GameObject.Find("Game(Clone)");
                if (theGameObject != null)
                {
                    Debug.Log("Game already exists no need to initialise a new one.");
                    // Get rid of the game initialiser
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
	}

	public void Start()
	{
		
	}

	private void Initialise()
	{
		if (players == null)
		{
			players = new List<Player>();
		}
		UnityEngine.Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;

		float aspectRatio = ((float)Screen.width / (float)Screen.height);
		if (aspectRatio >= 1.6f) // 16:9 
		{
			isWide = true;
		}
		else // Default to 4:3
		{
			isWide = false;
		}
		Debug.Log("Aspect:" + Screen.width + "x" + Screen.height + " | " + aspectRatio + " | " + (isWide ? "16:9 (Wide)" : "4:3 (Normal)"));

		// Never destroy this object unless the game closes itself.
		DontDestroyOnLoad(gameObject);

		// Load all save games. (Make a save file if none exists).
		AscentGameSaver.LoadGame();

		// Seed for the entire game. This script is executed first, no need to seed again.
		Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;

		// Set target frame rate
		Application.targetFrameRate = targetFrameRate;

		// Init the InputManager for the rest of the game
		InputManager.Initialise();

		// Set test characters
		if(testCharacters != null)
		{
			playerCharacterType = testCharacters;
			if (playerCharacterType.Length > 0)
			{
				CreateTestPlayers();
			}
		}

       
		// Add some necessary components
		tower = GetComponent<Tower>();
		effectFactory = gameObject.AddComponent<EffectFactory>();

		// Set the game state as the test state specified
		gameState = testState;
		gameStateToLoad = testState;
		InitialiseState();

		// Set this so it does not get initialised again
		initialised = true;
	}

    void Update()
    {
		InputManager.Update();
    }

	// This is a helper function to create players with heroes at any stage of the game
    private void CreateTestPlayers()
	{
		players = new List<Player>();

		int[] usedSaves = new int[3] {-1,-1, -1};

		for (int i = 0; i < playerCharacterType.Length; ++i)
		{
			// Create the test player object
			GameObject player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
			player.transform.parent = transform;
			player.transform.localPosition = Vector3.zero;
			player.transform.localRotation = Quaternion.identity;
			player.transform.localScale = Vector3.one;

			// Add the player component to it
			Player newPlayer = player.GetComponent<Player>();
			newPlayer.PlayerID = i;

			// Add player to list
			players.Add(newPlayer);

			// Assign it a device (devices can be doubled up if there arent enough)
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

			// Attempt to load an existing save to set for the player
			Hero hero = null;

			var heroSaves = AscentGameSaver.SaveData.heroSaves;
			if(heroSaves.Count > 0)
			{
				foreach(HeroSaveData save in heroSaves)
				{
					if(save.heroClass == playerCharacterType[i] &&
						i != usedSaves[0] &&
						i != usedSaves[1] &&
						i < heroSaves.Count)
					{
						hero = AscentGameSaver.LoadHero(heroSaves[i]);
						hero.Initialise(device, heroSaves[i]);
						hero.transform.parent = newPlayer.transform;
						usedSaves[i] = i;
                        break;
					}
				}
			}

			// Assign the loaded hero else create a new one.
			if (hero == null)
			{
				// Create the hero for the player
				newPlayer.CreateHero(playerCharacterType[i]);
			}
			else
			{
				newPlayer.Hero = hero;
			}

            player.name = "Player " + i;
		}
    }

	public void SetPlayers(List<Player> players)
	{
		this.players = players;
	}

	public void LoadLevel(EGameState state)
	{
		gameStateToLoad = state;

		switch (state)
		{
			case EGameState.MainMenu:
				{
					Application.LoadLevel("MainMenu");
				}
				break;
			case EGameState.HeroSelect:
				{
					Application.LoadLevel("HeroSelect");
				}
				break;
			case EGameState.City:
				{
					Application.LoadLevel("City");
				}
				break;
			case EGameState.Tower:
				{
					Application.LoadLevel("Tower");
				}
				break;
			case EGameState.TestTower:
				{
					Application.LoadLevel("TestTower");
				}
				break;
			case EGameState.FloorSummary:
				{
					Application.LoadLevel("FloorSummary");
				}
				break;
			case EGameState.Loading:
				{
					Application.LoadLevel("Loading");
				}
				break;
			case EGameState.None: // Fall
			default:
				{
					Debug.LogError("Unhandled case.");
				}
				break;
		}

	}

	public void OnLevelWasLoaded(int iLevelID)
	{
		//if (firstState)
		//{
		//    Debug.Log("FIRST");
		//    firstState = false;
		//    return;
		//}

		if(gameStateToLoad != EGameState.None)
		{
			InitialiseState();
		}
	}

	public void InitialiseState()
	{
		switch (gameStateToLoad)
		{
			case EGameState.MainMenu:// Fall
			case EGameState.HeroSelect: 
				{
					InputManager.UnbindAllDevices();
					if(players != null)
					{
						foreach(Player p in players)
						{
							if (p != null)
							{
								Destroy(p.gameObject);
							}
						}
					}
				}
				break;
			case EGameState.TestTower:
				{
					foreach (Player p in players)
					{
						if (p != null)
						{
							p.Hero.gameObject.SetActive(true);
						}
					}
					tower.InitialiseTestFloor();
				}
				break;
			case EGameState.Tower:
				{
					foreach (Player p in players)
					{
						if (p != null)
						{
							p.Hero.gameObject.SetActive(true);
						}
					}
                    tower.InitialiseTestFloor();
				}
				break;
			case EGameState.FloorSummary: // Fall
			case EGameState.City:
				{
					foreach (Player p in players)
					{
						if (p != null)
						{
							p.Hero.gameObject.SetActive(false);
						}
					}
				}
				break;
			case EGameState.Loading:
				{
					gameState = gameStateToLoad;
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case");
				}
				break;
		}

		gameState = gameStateToLoad;

		gameStateToLoad = EGameState.None;
	}
	
	#endregion

}
