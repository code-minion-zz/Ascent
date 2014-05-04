using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour 
{
    private List<Player> players;

	private Floor currentFloor;
	public Floor CurrentFloor
	{
		get { return currentFloor;  }
		set { currentFloor = value; }
	}

	public int currentFloorNumber;
    public int numberOfPlayers;
    public int keys;
    public int lives;

    public bool initialised;

    public void InitialiseTower()
    {
        if (!initialised)
        {
            if( Game.Singleton.Players == null || Game.Singleton.Players.Count == 0)
            {
                players = new List<Player>();
                for (int i = 0; i < numberOfPlayers; ++i)
                {
                    GameObject go = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
                    go.transform.parent = Game.Singleton.transform;
                    Player newPlayer = go.GetComponent<Player>() as Player;

                    newPlayer.PlayerID = i;
                    newPlayer.name = "Player" + i;

                    InputDevice device = InputManager.GetNextUnusedDevice();
                    newPlayer.BindInputDevice(device);
                    device.InUse = true;

                    players.Add(newPlayer);

                    newPlayer.CreateHero(Character.EHeroClass.Warrior);
                    newPlayer.Hero.gameObject.SetActive(true);
                }
              
                Game.Singleton.SetPlayers(players);
            }



            initialised = true;
        }

        InitialiseTestFloor();
    }

	[ContextMenu("NextFloor")]
	public void GoUp()
	{
		FloorHUDManager.Singleton.LevelCompleteScreen();
		Game.Singleton.Tower.CurrentFloor.gameOver = true;
	}

    public void LoadNextFloor()
    {
        ++currentFloorNumber;
        Destroy(currentFloor);

        Game.Singleton.gameStateToLoad = Game.EGameState.TowerPlayer1;

        foreach(Player p in Game.Singleton.Players)
        {
            p.Hero.Loadout.StopAbility();
			p.Hero.Motor.StopMovingAlongGrid();
            p.Hero.Motor.StopMotion();
			p.Hero.HeroAnimator.PlayMovement(HeroAnimator.EMoveAnimation.IdleLook);
            p.Hero.RefreshEverything();
        }

        if (currentFloorNumber > Game.Singleton.maxFloor)
        {
            Destroy(currentFloor);
            Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
        }
        else
        {
            Application.LoadLevel("P" + 1 + "Floor" + currentFloorNumber);
        }
    }

    [ContextMenu("GameOver")]
    public void GameOver()
    {
        initialised = false;

        foreach (Player p in players)
        {
            Destroy(p.gameObject);
            Destroy(p);
        }

        Game.Singleton.Players = null;
        currentFloorNumber = 0;
        Destroy(currentFloor);
        Game.Singleton.LoadLevel(Game.EGameState.MainMenu);
    }

    public void InitialiseTestFloor()
    {
        currentFloor = gameObject.AddComponent<Floor>();
		currentFloor.InitialiseTestFloor();
        MusicManager musicMan = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		musicMan.PlayMusic(MusicManager.MusicSelections.Tower);
    }

	public void InitialiseFloor()
	{
        currentFloorNumber = 1;
        currentFloor = gameObject.AddComponent<Floor>();
        currentFloor.InitialiseRandomFloor();
	}

    protected float experienceGainBonus;
    public float ExperienceGainBonus
    {
        get { return experienceGainBonus; }
    }

    protected float goldGainBonus;
    public float GoldGainBonus
    {
        get { return goldGainBonus; }
    }
    
}
