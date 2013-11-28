using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

#pragma warning disable 0162

public class InputManager : MonoBehaviour
{
	public enum InputHandlingMethod
	{
		Polling,
		EventDriven
	}

	public const bool debugMessages = true;
	public const InputHandlingMethod inputHandlingMethod = InputHandlingMethod.Polling;

	static public bool IsPolling
	{
		get { return inputHandlingMethod == InputHandlingMethod.Polling; }
	}


	public delegate void DeviceEventHandler(InputDevice device);
	public static event DeviceEventHandler OnDeviceAttached;
	public static event DeviceEventHandler OnDeviceDetached;

	static List<InputDevice> devices = new List<InputDevice>();
	public static List<InputDevice> Devices
	{
		get { return devices; }
	}

	public static string platform { get; private set; }

	static string prevJoystickHash = "";

	const int maxJoysticks = 4;

	public static void Initialise()
	{
		platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

		string[] joystickNames = Input.GetJoystickNames();

		int length = joystickNames.Length;
		Debug.Log(length + " joysticks attached.");

		//for (int i = 0; i < length; ++i)
		//{
		//    Debug.Log(i + ": " + joystickNames[i]);
		//}

		OnDeviceAttached = null;
		OnDeviceDetached = null;

		AttachKeyboard();
		RefreshDevices();
	}

	public static void Update()
	{
		if (prevJoystickHash != JoystickHash)
		{
			RefreshDevices();
		}

		if(inputHandlingMethod == InputHandlingMethod.Polling)
		{
			foreach (InputDevice d in devices)
			{
				d.Update();
			}
		}
		else // inputHandlingMethod == InputHandlingMethod.Polling
		{
			foreach (InputDevice d in devices)
			{
				d.UpdateEvents();
			}
		}
	}


	static void RefreshDevices()
	{
		DetectAndAttachJoysticks();
		DetectAndDetachJoysticks();

		prevJoystickHash = JoystickHash;
	}

	static void AttachKeyboard()
	{
		AttachDevice(new KeyboardInputDevice());
	}

	static void DetectAndAttachJoysticks()
	{
		for (int i = 0; i < maxJoysticks; ++i)
		{
			PlayerIndex playerIndex = (PlayerIndex)i;

			GamePadState state = GamePad.GetState(playerIndex);

			if (state.IsConnected)
			{
				// Check if it is already attached

				bool alreadyExists = false;

				foreach (InputDevice d in devices)
				{
					if (d.Name == Xbox360InputDevice.xboxName + i)
					{
						// It already exists
						alreadyExists = true;
						break;
					}
				}

				if (!alreadyExists)
				{
					AttachDevice(new Xbox360InputDevice(i));
				}
			}
		}
	}

	static void DetectAndDetachJoysticks()
	{
		List<InputDevice> detachedDevices = new List<InputDevice>();

		for (int i = 0; i < maxJoysticks; ++i)
		{
			PlayerIndex playerIndex = (PlayerIndex)i;

			GamePadState state = GamePad.GetState(playerIndex);

			if (!state.IsConnected)
			{
				// Check if it was attached
				foreach (InputDevice d in devices)
				{
					if (d.Name == Xbox360InputDevice.xboxName + i)
					{
						detachedDevices.Add(d);
						break;
					}
				}
			}
		}

		foreach (InputDevice d in detachedDevices)
		{
			DetachDevice(d);
		}

		detachedDevices.Clear();
	}

	public static InputDevice GetDevice(int i)
	{
		return Devices[i];
	}

	static void AttachDevice(InputDevice inputDevice)
	{
		devices.Add(inputDevice);
		Debug.Log("Connected: " + inputDevice.Name);

		if (OnDeviceAttached != null)
		{
			OnDeviceAttached(inputDevice);
		}
	}

	static void DetachDevice(InputDevice inputDevice)
	{
		devices.Remove(inputDevice);
		Debug.Log("Disconnected: " + inputDevice.Name);

		inputDevice.SendDisconnectionEvent();

		if (OnDeviceDetached != null)
		{
			OnDeviceDetached(inputDevice);
		}
	}

	static string JoystickHash
	{
		get
		{
			var joystickNames = Input.GetJoystickNames();
			return joystickNames.Length + ": " + String.Join(", ", joystickNames);
		}
	}
}