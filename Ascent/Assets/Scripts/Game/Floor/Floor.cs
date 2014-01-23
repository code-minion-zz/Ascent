using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour 
{
	public enum TransitionDirection
	{
		North = 0,
		South,
		East,
		West
	}

	public GameObject groundPrefab = Resources.Load("Prefabs/RoomWalls/Ground") as GameObject;
	public GameObject wallPrefab = Resources.Load("Prefabs/RoomWalls/Wall") as GameObject;
	public GameObject wallWindowPrefab = Resources.Load("Prefabs/RoomWalls/WallWindow") as GameObject;
	public GameObject doorPrefab = Resources.Load("Prefabs/RoomWalls/Door") as GameObject;
	public GameObject cornerPrefab = Resources.Load("Prefabs/RoomWalls/WallCorner") as GameObject;
	public GameObject archPrefab = Resources.Load("Prefabs/RoomWalls/Archway") as GameObject;

	private List<Player> players;
    private List<Enemy> enemies;
	private GameObject[] startPoints;
	private GameObject floorCamera;
	private Room currentRoom;
	private Room targetRoom;
	private FadePlane fadePlane;
    private Room[] allRooms;
    private FloorInstanceReward floorInstanceReward;
    private FloorGeneration floorGenerator;

	// Camera offset
	//private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;

    public Room CurrentRoom
    {
        get { return currentRoom; }
    }

	public Camera MainCamera
	{
		get { return floorCamera.camera; }
	}

	public FloorCamera FloorCamera
	{
		get { return floorCamera.GetComponent<FloorCamera>(); }
	}

	public GameObject[] StartPoints
	{
		get { return startPoints; }
	}

    public FloorInstanceReward FloorInstanceReward
    {
        get { return floorInstanceReward; }
    }

	public List<Enemy> Enemies
	{
		get { return enemies; }
	}

    public List<Player> Players
    {
        get { return players; }
    }

    public void Initialise(FloorGeneration floorGen)
    {
        floorGenerator = floorGen;
        InitialiseCamera();
        floorGenerator.GenerateFloor();
        Initialise();
    }

    // Initialize the camera first.
    private void InitialiseCamera()
    {
        // Create the floor's camera
        GameObject go = null;

        if (orthographicCamera)
        {
            go = Resources.Load("Prefabs/Floors/floorCameraOrtho") as GameObject;
        }
        else
        {
            go = Resources.Load("Prefabs/Floors/floorCamera") as GameObject;
        }

        floorCamera = Instantiate(go) as GameObject;
        floorCamera.name = "floorCamera";

        floorCamera.GetComponent<FloorCamera>().Initialise();

        go = new GameObject();
        go.name = "Cameras";
        go.tag = "Cameras";

        floorCamera.transform.parent = go.transform;
    }

	private void Initialise()
	{
		// Create HUD
		GameObject.Instantiate(Resources.Load("Prefabs/UI/HUD_backup"));

		// Initialise the players onto the start points
		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

        if (startPoints == null)
        {
            Debug.Log("Could not find StartPoints please make sure there is an object with tag StartPoint");
        }

		players = Game.Singleton.Players;

		for (int i = 0; i < players.Count; ++i)
		{
			Vector3 pos = startPoints[i].transform.position;
			players[i].Hero.transform.position = pos;
			players[i].Hero.transform.rotation = Quaternion.identity;
			players[i].Hero.transform.localScale = Vector3.one;
			players[i].Hero.SetActive(true);

            players[i].Hero.GetComponent<Hero>().onDeath += OnPlayerDeath;
            // Reset individual hero floor records.
            players[i].Hero.GetComponent<Hero>().ResetFloorStatistics();
		}

        // Finds all the rooms
		allRooms = GameObject.FindObjectsOfType<Room>() as Room[];
        currentRoom = GameObject.Find("StartRoom").GetComponent<Room>();

		// Initialise all the enemies
		enemies = new List<Enemy>();
		GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
		for (int i = 0; i < monsters.Length; ++i)
		{
			Enemy thisEnemy = monsters[i].GetComponent<Enemy>();

			if (thisEnemy != null)
			{
				thisEnemy.Initialise();
				thisEnemy.InitiliseHealthbar();
                thisEnemy.onDeath += OnEnemyDeath;
				enemies.Add(thisEnemy);
			}
		}

		GameObject go = Instantiate(Resources.Load("Prefabs/Floors/FadePlane")) as GameObject;
		fadePlane = go.GetComponent<FadePlane>();
		go.SetActive(false);

        currentRoom.gameObject.SetActive(true);

        // Create the floor record keeper.
        floorInstanceReward = new FloorInstanceReward(this);
	}

	public void AddEnemy(Enemy _enemy)
	{
		enemies.Add(_enemy);
	}

	#region Update

    /// <summary>
    /// Event called when a player has died on this floor.
    /// </summary>
    /// <param name="character">The character / hero that has died.</param>
    private void OnPlayerDeath(Character character)
    {
        Hero hero = character as Hero;

        if (hero != null)
        {
            if (hero.DerivedStats.Lives > 0)
            {
                if (currentRoom.startRoom)
                {
                    //hero.Respawn(startPoints[0].transform.position);
                }
                else
                {
                    //hero.Respawn(currentRoom.EntryDoor.transform.position);
                    //hero.Respawn(startPoints[0].transform.position);
                }

                //--hero.DerivedStats.Lives;
                hero.FloorStatistics.NumberOfDeaths++;
            }
            else
            {
                hero.gameObject.SetActive(false);
                hero.onDeath -= OnPlayerDeath;
            }
        }
    }

    /// <summary>
    /// Event called when an enemy has died on the floor.
    /// </summary>
    /// <param name="character">The enemy that has died.</param>
    private void OnEnemyDeath(Character character)
    {
        if (character.LastDamagedBy != null)
        {
            // This may break if the enemy was killed by something else such as a trap with no owner maybe?
            Hero hero = character.LastDamagedBy as Hero;
            hero.FloorStatistics.NumberOfMonstersKilled++;
        }

        // Give all players in the room the bounty.
        foreach (Player player in players)
        {
            Hero hero = player.Hero.GetComponent<Hero>();
            hero.FloorStatistics.ExperienceGained += character.CharacterStats.ExperienceBounty;
        }

        // Unsubscribe from listening to events from this enemy.
        character.onDeath -= OnEnemyDeath;
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.F1))
		{
			EndFloor();
		}

		//DEBUG

        if (currentRoom.Doors == null)
        {
            return;
        }

		Door[] roomDoors = currentRoom.Doors.doors;
		
		InputDevice input = Game.Singleton.Players[0].Input;

		if (Input.GetKeyUp(KeyCode.Keypad8) || input.DPadUp.WasReleased) // UP
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.North)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.North, d.targetDoor);
				}
			}
		}
		else if (Input.GetKeyUp(KeyCode.Keypad2) || input.DPadDown.WasReleased)
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.South)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.South, d.targetDoor);
				}
			};
		}
		else if (Input.GetKeyUp(KeyCode.Keypad4) || input.DPadLeft.WasReleased)
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.West)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.West, d.targetDoor);
				}
			}
		}
		else if (Input.GetKeyUp(KeyCode.Keypad6) || input.DPadRight.WasReleased)
		{
			foreach (Door d in roomDoors)
			{
				if (d == null) continue;
				if (d.targetDoor != null && d.direction == TransitionDirection.East)
				{
					TransitionToRoomImmediately(Floor.TransitionDirection.East, d.targetDoor);
				}
			}
		}
	}

	void EndFloor()
	{
		// Disable the whole floor( audio listener from the camera )
		enabled = false;
		floorCamera.SetActive(false);

		// Disable input on all heroes
		foreach (Player player in players)
		{
			//player.Hero.GetComponent<Hero>().HeroController.DisableInput();
			player.Hero.SetActive(false);
		}

		// Show summary screen
		//Instantiate(Resources.Load("Prefabs/FloorSummary"));

        floorInstanceReward.ApplyFloorInstanceRewards();

        Game.Singleton.LoadLevel("Level2", Game.EGameState.Tower);

		// Enable input on summary screen
	}


	public void TransitionToRoom(TransitionDirection direction, Door targetDoor)
	{
		// Set old remove inactive and new one active
		fadePlane.StartFade(0.5f, currentRoom.transform.position);

		StartCoroutine(CoTransitionToRoom());

		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();


		Room prevRoom = currentRoom;
		currentRoom = targetRoom;
		targetRoom = prevRoom;

		currentRoom.gameObject.SetActive(true);

		// Move heroes to the new room
		foreach (Player p in players)
		{
			p.Hero.transform.position = targetDoor.transform.position;
		}

		// Move camera over
		FloorCamera.TransitionToRoom(direction);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();
	}

	public IEnumerator CoTransitionToRoom()
	{
		yield return new WaitForSeconds(1.5f);

		targetRoom.gameObject.SetActive(false);

		//fadePlane.gameObject.SetActive(false);
	}

	public void TransitionToRoomImmediately(TransitionDirection direction, Door targetDoor)
	{
		targetRoom = targetDoor.transform.parent.parent.parent.GetComponent<Room>();

		currentRoom.gameObject.SetActive(false);
		currentRoom = targetRoom;
		currentRoom.gameObject.SetActive(true);

		// Move heroes to the new room
		foreach (Player p in players)
		{
			p.Hero.transform.position = targetDoor.transform.position;
		}

		// Move camera over
        FloorCamera.TransitionToRoom(direction);

		currentRoom.EntryDoor = targetDoor;
		targetDoor.SetAsStartDoor();

		targetRoom.gameObject.SetActive(true);
	}

	#endregion
}
