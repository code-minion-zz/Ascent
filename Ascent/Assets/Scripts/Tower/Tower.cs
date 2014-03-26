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
    public void LoadNextFloor()
    {
        ++currentFloorNumber;
        Application.LoadLevel("P" + numberOfPlayers + "Floor" + currentFloorNumber);
    }

    public void InitialiseTestFloor()
	{
        currentFloor = gameObject.AddComponent<Floor>();
		currentFloor.InitialiseTestFloor();
        MusicManager soundMan = GameObject.Find("SoundManager").GetComponent<MusicManager>();
        soundMan.PlayMusic(MusicManager.MusicSelections.Tower);
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
