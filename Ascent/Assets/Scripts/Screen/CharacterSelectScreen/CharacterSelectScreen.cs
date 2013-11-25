using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectScreen : MonoBehaviour 
{
	public const string levelToLoad = "Level";

	private const int maxPlayers = 3;

	private List<Player> players = new List<Player>();
	List<Player> playersToRemove = new List<Player>();

	private List<Transform> charSelectPanel = new List<Transform>(3);
	private List<Transform> readyLabels = new List<Transform>(4);

    private List<UIPlayerPanel> playerPanel = new List<UIPlayerPanel>(3);

	private int nextEmptyPlayerSlot = 0;

	bool allReady = false;

	List<InputDevice> devices;

	// Use this for initialization
	void Start()
	{
		Transform xform = transform.FindChild("UI Root (2D)");
		xform = xform.FindChild("Camera");
		xform = xform.FindChild("Anchor");
		xform = xform.FindChild("Panel");
		xform = xform.FindChild("Background");

		charSelectPanel.Add(xform.FindChild("Player 1 Panel"));
        //playerPanel.Add(charSelectPanel[0].GetComponent<UIPlayerPanel>());
		charSelectPanel[0].gameObject.SetActive(false);
		charSelectPanel.Add(xform.FindChild("Player 2 Panel"));
        //playerPanel.Add(charSelectPanel[1].GetComponent<UIPlayerPanel>());
		charSelectPanel[1].gameObject.SetActive(false);
		charSelectPanel.Add(xform.FindChild("Player 3 Panel"));
        //playerPanel.Add(charSelectPanel[2].GetComponent<UIPlayerPanel>());
		charSelectPanel[2].gameObject.SetActive(false);

		readyLabels.Add(xform.FindChild("Ready 1"));
		readyLabels[0].gameObject.SetActive(false);
		readyLabels.Add(xform.FindChild("Ready 2"));
		readyLabels[1].gameObject.SetActive(false);
		readyLabels.Add(xform.FindChild("Ready 3"));
		readyLabels[2].gameObject.SetActive(false);
		readyLabels.Add(xform.FindChild("Ready 4"));
		readyLabels[3].gameObject.SetActive(false);

		InputManager.OnDeviceAttached += OnDeviceAttached;
		InputManager.OnDeviceDetached += OnDeviceDetached;

		devices = InputManager.Devices;
	}

	void OnDestroy()
	{
		InputManager.OnDeviceAttached -= OnDeviceAttached;
		InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

	// Update is called once per frame
	void Update()
	{
		// Remove players that wish to leave
		if (playersToRemove.Count > 0)
		{
			foreach (Player p in playersToRemove)
			{
				RemovePlayer(p);
			}

			playersToRemove.Clear();

			if (players.Count == 0)
			{
				readyLabels[3].gameObject.SetActive(false);
			}
		}

		// Check if ready to move onto next screeen
		if (allReady)
		{
			if (players[0].Input.Start.WasPressed || players[0].Input.A.WasPressed)
			{
				ToNextScreen();
			}
		}

		// Check if players are ready to start
		foreach (Player p in players)
		{
			if (readyLabels[p.PlayerID].GetComponent<UILabel>().color == Color.red &&
				(p.Input.Start.WasPressed || p.Input.A.WasPressed))
			{
				// Player wants to ready up
				readyLabels[p.PlayerID].GetComponent<UILabel>().color = Color.green;
				readyLabels[p.PlayerID].GetComponent<UILabel>().text = "Ready";

				allReady = CheckAllPlayersReady();
			}
			else if (p.Input.B.WasPressed)
			{
				if (readyLabels[p.PlayerID].GetComponent<UILabel>().color == Color.green)
				{
					// Players wants to unready
					readyLabels[p.PlayerID].GetComponent<UILabel>().color = Color.red;
					readyLabels[p.PlayerID].GetComponent<UILabel>().text = "Not Ready";

					allReady = CheckAllPlayersReady();
				}
				else if (readyLabels[p.PlayerID].GetComponent<UILabel>().color == Color.red)
				{
					// Player wants to leave
					p.Input.InUse = false;
					p.UnbindInputDevice();
					playersToRemove.Add(p);

					continue;
				}
			}
		}

		// Check if any players want to enter the game
		if (players.Count < maxPlayers)
		{
			foreach (InputDevice device in devices)
			{
				if (!device.InUse)
				{
					if (device.Start.WasPressed || device.A.WasPressed)
					{
						device.InUse = true;

						GameObject go = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
						go.transform.parent = Game.Singleton.transform;
						Player newPlayer = go.GetComponent<Player>() as Player;

						players.Add(newPlayer);

						newPlayer.PlayerID = nextEmptyPlayerSlot;
						newPlayer.name = newPlayer.name + nextEmptyPlayerSlot;

						charSelectPanel[nextEmptyPlayerSlot].gameObject.SetActive(true);

						readyLabels[nextEmptyPlayerSlot].gameObject.SetActive(true);
						readyLabels[nextEmptyPlayerSlot].GetComponent<UILabel>().color = Color.red;
						readyLabels[nextEmptyPlayerSlot].GetComponent<UILabel>().text = "Not Ready";

                        newPlayer.BindInputDevice(device);

                        newPlayer.activePlayerPanel = charSelectPanel[nextEmptyPlayerSlot].FindChild("LoadNew Panel").GetComponent<UIPlayerPanel>();
                        newPlayer.activePlayerPanel.Initialise(newPlayer);

						++nextEmptyPlayerSlot;

						

						readyLabels[3].gameObject.SetActive(true);
						readyLabels[3].GetComponent<UILabel>().color = Color.red;
						readyLabels[3].GetComponent<UILabel>().text = "All Not Ready";

						allReady = CheckAllPlayersReady();
					}
				}
			}
		}
	}

	public bool CheckAllPlayersReady()
	{
		bool ready = false;

		foreach (Player p in players)
		{
			ready = readyLabels[p.PlayerID].GetComponent<UILabel>().color == Color.green;
			
			if (!ready)
			{
				readyLabels[3].GetComponent<UILabel>().color = Color.red;
				readyLabels[3].GetComponent<UILabel>().text = "All Not Ready";

				return ready;
			}
		}

		readyLabels[3].GetComponent<UILabel>().color = Color.green;
		readyLabels[3].GetComponent<UILabel>().text = "All Ready";

		return ready;
	}

	public void ToNextScreen()
	{
		// NormalisePlayerList: Adjust Ids so that they are ordered 0 - 2. 
		for (int i = 0; i < players.Count; ++i)
		{
			players[i].PlayerID = i;

			players[i].CreateHero(Character.EHeroClass.Warrior);
		}

		// Send these over to the Game for storage.
		Game.Singleton.SetPlayers(players);

		// On to the next screen.
		Game.Singleton.LoadLevel(levelToLoad);
	}

	public void OnDeviceAttached(InputDevice device)
	{
		// Repoll all the devices
		devices = InputManager.Devices;
	}

	public void OnDeviceDetached(InputDevice device)
	{
		Debug.Log("Deta");
		// Remove player from game if their device was in use
		if (device.InUse)
		{
			foreach (Player p in players)
			{
				if (p.Input == device)
				{
					p.Input.InUse = false;
					p.UnbindInputDevice();

					playersToRemove.Add(p);

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

		charSelectPanel[p.PlayerID].gameObject.SetActive(false);
		readyLabels[p.PlayerID].gameObject.SetActive(false);

		players.Remove(p);
		Destroy(p.gameObject);
	}
}
