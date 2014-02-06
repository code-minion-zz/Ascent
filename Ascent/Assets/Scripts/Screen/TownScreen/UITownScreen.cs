using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITownScreen : UIPlayerMenuScreen 
{
	public GameObject WindowPrefab;
	public GameObject PlayerGrid;
	public delegate void ReadyHandler();

	private const int maxPlayers = 3;
	private List<Player> players = new List<Player>();
	private int nextEmptyPlayerSlot = 0;
#pragma warning disable 0649
    private	uint readyPlayers;

#pragma warning disable 0414 // UITownScreen.devices assigned but never used.
	List<InputDevice> devices;

	void OnDestroy()
	{
		InputManager.OnDeviceAttached -= OnDeviceAttached;
		InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

	public override void Start()
	{
		base.Start();

		InputManager.OnDeviceAttached += OnDeviceAttached;
		InputManager.OnDeviceDetached += OnDeviceDetached;

		devices = InputManager.Devices;

		players = Game.Singleton.Players;


		// Activate windows for number of players
		for (int i = 0; i < players.Count; ++i)
		{
			windows[i].gameObject.SetActive(true);
			windows[i].SetPlayer(players[i]);
			windows[i].Initialise();
			windows[i].OnEnable();
		}
	}

	public void Update () 
	{
	}


	public void OnDeviceAttached(InputDevice device)
	{
		// Repoll all the devices
		devices = InputManager.Devices;
	}

	public void OnDeviceDetached(InputDevice device)
	{
		// Remove player from game if their device was in use
		if (device.InUse)
		{
			foreach (Player p in players)
			{
				if (p.Input == device)
				{
                    foreach (UIPlayerMenuWindow win in windows)
                    {
                        if(win.Player == p)
                        {
                            win.CloseWindow();
                        }
                    }
					continue;
				}
			}
		}

		// Repoll all the devices
		devices = InputManager.Devices;
	}

	public void RemovePlayer(Player p)
	{
		nextEmptyPlayerSlot = Mathf.Min(nextEmptyPlayerSlot, p.PlayerID);

		players.Remove(p);
		Destroy(p.gameObject);
	}

	void ReadyTracker()
	{

		if (readyPlayers == Game.Singleton.NumberOfPlayers)
		{
			// load next level
			Game.Singleton.LoadLevel("Sewer_level", Game.EGameState.TowerRandom);
		}
	}
}
