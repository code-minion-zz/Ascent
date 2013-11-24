using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManagerTest : MonoBehaviour 
{

	GUIText textDevices;
	GUIText textPlayers;

	private int nextEmptyPlayerSlot = 0;
	List<Player> players = new List<Player>();
	List<Player> playersToRemove = new List<Player>();

	// Use this for initialization
	void Start () 
	{
		textDevices = GameObject.Find("GUI Text1").guiText;
		textPlayers = GameObject.Find("GUI Text2").guiText;

		InputManager.Initialise();

		InputManager.OnDeviceAttached += OnDeviceAttached;
		InputManager.OnDeviceDetached += OnDeviceDetached;
	}

	void OnDestroy()
	{
		InputManager.OnDeviceAttached -= OnDeviceAttached;
		InputManager.OnDeviceDetached -= OnDeviceDetached;
	}

	void Update()
	{
		InputManager.Update();
		textDevices.text = "Devices\n";

		// We want to keep grabbing this list incase it changes

		List<InputDevice> devices = InputManager.Devices;
		foreach (InputDevice device in devices)
		{
			textDevices.text += "\n" + device.Name;

			if (device.InUse)
			{
				textDevices.text += "(Binded)";
			}
		}

		textPlayers.text = "Player and Binds\n\n";

		if (players.Count < 3)
		{
			// Check if other player's want to play
			foreach (InputDevice device in devices)
			{
				if (!device.InUse)
				{
					if (device.Start.IsPressed)
					{
						device.InUse = true;

						GameObject go  = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
						Player newPlayer = go.GetComponent<Player>() as Player;

						players.Add(newPlayer);

						newPlayer.PlayerID = nextEmptyPlayerSlot;

						++nextEmptyPlayerSlot;


						newPlayer.BindInputDevice(device);

						// Mayb register to events so it can disconnect itself
					}
				}
			}
		}

		foreach (Player p in players)
		{
			if (p.Input.B.IsPressed)
			{
				p.Input.InUse = false;
				p.UnbindInputDevice();

				playersToRemove.Add(p);

				continue;
			}

			textPlayers.text += p.name + "(id: " + p.PlayerID + ") is binded with: " + p.Input.Name + "\n";
		}

		if (playersToRemove.Count > 0)
		{

			foreach (Player p in playersToRemove)
			{
				nextEmptyPlayerSlot = Mathf.Min(nextEmptyPlayerSlot, p.PlayerID);

				players.Remove(p);
				//Destroy(p.gameObject);
			}

			playersToRemove.Clear();
		}
	}

	public void OnDeviceAttached(InputDevice device)
	{

	}

	public void OnDeviceDetached(InputDevice device)
	{
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

			if (playersToRemove.Count > 0)
			{
				foreach (Player p in playersToRemove)
				{
					nextEmptyPlayerSlot = Mathf.Min(nextEmptyPlayerSlot, p.PlayerID);

					players.Remove(p);
					//Destroy(p.gameObject);
				}

				playersToRemove.Clear();
			}
		}
	}
}
