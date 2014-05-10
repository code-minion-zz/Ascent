using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
#if UNITY_WEBPLAYER
	public const float KfWebVersion = 0.1f;
#endif

	public static int KIMaxPlayers = 4;

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
        Tower,
		TowerPlayer1,
        TowerPlayer2,
        TowerPlayer3,
		TowerPlayer4,
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
        get { return gameState == EGameState.Tower || 
                    gameState == EGameState.TestTower || 
                    gameState == EGameState.TowerPlayer1 ||
                    gameState == EGameState.TowerPlayer2 ||
                    gameState == EGameState.TowerPlayer3 ||
					gameState == EGameState.TowerPlayer4;
		}
	}

	public EGameState gameStateToLoad;
	
	#endregion	
	
	#region Properties

    public int NumberOfPlayers
    {
		get { return players.Count; }//playerCharacterType.Length; }
    }

    public List<Player> Players
    {
        get { return players; }
        set { players = value; }
    }

	public List<Hero> AliveHeroes
	{
		get 
		{
			List<Hero> aliveHeroes = new List<Hero>();

			foreach(Player p in players)
			{
				if (!p.Hero.IsDead)
					aliveHeroes.Add(p.Hero);
			}

			return aliveHeroes;
		}
	}

	public int maxFloor = 2;
	public int startingHealth = 20;
	public int startingSpecial = 9;
	public bool invincible = false;

	public float fadeIntoLevelTime = 1.5f;
	public float fadeOutOfLevelTime = 1.5f;
	public float fadeOutFromGameOverTime = 1.5f;
	public float fadeToGameOverTime = 1.5f;

	public float showLevelStartMessageTimer = 2.0f;
	public float showGameOvermessageTimer = 3.0f;
	public float showLevelCompleteMessageTimer = 2.0f;

	public float GameOverDelay
	{
		get { return fadeOutFromGameOverTime + showGameOvermessageTimer + 0.5f; }
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

	private void Initialise()
	{
		UnityEngine.Random.seed = (int)System.DateTime.Now.TimeOfDay.Ticks;

        Application.targetFrameRate = targetFrameRate;

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
		GameObject effectFactory = GameObject.Instantiate(Resources.Load("Prefabs/EffectFactory")) as GameObject;
        effectFactory.name = "EffectFactory";


		// Set the game state as the test state specified
		gameState = testState;
		gameStateToLoad = testState;
		InitialiseState();
		SoundManager.Initialise();

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

		int[] usedSaves = new int[4] {-1,-1, -1, -1};

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
						i != usedSaves[2] &&
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
                    Application.LoadLevel("TestTower");
				}
				break;
            case EGameState.TowerPlayer1:
                {
                    Application.LoadLevel("P1Floor1");
                }
                break;
            case EGameState.TowerPlayer2:
                {
                    Application.LoadLevel("P1Floor1");
                }
                break;
            case EGameState.TowerPlayer3:
                {
                    Application.LoadLevel("P1Floor1");
                }
                break;
			case EGameState.TowerPlayer4:
				{
					Application.LoadLevel("P1Floor1");
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

                        players.Clear();
                        Tower.currentFloorNumber = 0;
                        Tower.numberOfPlayers = 0;
                        Tower.lives = 0;
                        Tower.keys = 0;
                        Tower.initialised = false;
                        Destroy(tower.CurrentFloor);
					}
				}
				break;
			case EGameState.TestTower:
            case EGameState.Tower:
            case EGameState.TowerPlayer1:
            case EGameState.TowerPlayer2:
            case EGameState.TowerPlayer3:
			case EGameState.TowerPlayer4:
				{

					if (tower == null)
						return;

                    if (players != null)
                    {
                        foreach (Player p in players)
                        {
                            if (p != null)
                            {
                                p.Hero.gameObject.SetActive(true);
                            }
                        }
                    }
					tower.InitialiseTower();
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
					//Debug.LogError("Unhandled case");
				}
				break;
		}

		gameState = gameStateToLoad;

		gameStateToLoad = EGameState.None;
	}
	
	#endregion

	public Player GetPlayer(Hero hero)
	{
		foreach(Player p in players)
		{
			if(p.Hero == hero)
			{
				return p;
			}
		}

		return null;
	}

}
