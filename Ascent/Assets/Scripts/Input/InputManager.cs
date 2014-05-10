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

	private const int KIXBox360Hash = -1383746887; // "Controller (XBOX 360 For Windows)"

	public const bool debugMessages = false;
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

    public static InputDevice KeyBoard
    {
        get { return devices[0]; }
    }

	public static string platform { get; private set; }

	static string prevJoystickHash = "";

	const int maxJoysticks = 4;

	public static void Initialise()
	{
		platform = (SystemInfo.operatingSystem + " " + SystemInfo.deviceModel).ToUpper();

		string[] joystickNames = Input.GetJoystickNames();

		if(InputManager.debugMessages)
		{
			int length = joystickNames.Length;
			Debug.Log(length + " joysticks attached.");

			for (int i = 0; i < joystickNames.Length; ++i)
			{
				Debug.Log(i + ": " + joystickNames[i]);
			}
		}

		OnDeviceAttached = null;
		OnDeviceDetached = null;

		AttachKeyboard();
		RefreshDevicesList();
	}

	public static void Update()
	{
		if (prevJoystickHash != JoystickHash)
		{
#if UNITY_WEBPLAYER
#else
			RefreshDevicesList();
#endif
		}

		if (disableTimer > 0.0f)
		{
			disableTimer -= Time.deltaTime;
		}
		else
		{
			isEnabled = true;
		}

		if (!isEnabled)
		{
			return;
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
	
	public static bool isEnabled;
	private static float disableTimer;
	public static void DisableInputForTime(float time)
	{
		disableTimer = time;
		isEnabled = false;
	}

	public static void UnbindAllDevices()
	{
		foreach (InputDevice d in devices)
		{
			d.InUse = false;
		}
	}

	static void RefreshDevicesList()
	{
#if UNITY_WEBPLAYER
		if (Input.GetJoystickNames().Length > 0)
		{
			int xboxDeviceCount = 0;
			foreach (string deviceName in Input.GetJoystickNames())
			{
				if (deviceName.GetHashCode() == KIXBox360Hash)
				{
					++xboxDeviceCount;
				}
			}

			for (int i = 0; i < xboxDeviceCount; ++i)
			{
				AttachDevice(new XBox360InputDevice(i));
			}
		}
#else
		DetectAndAttachJoysticks();
		DetectAndDetachJoysticks();

		prevJoystickHash = JoystickHash;
#endif
	}

	static void AttachKeyboard()
	{
		AttachDevice(new KeyboardInputDevice());
	}

	static void DetectAndAttachJoysticks()
	{
		for (int i = 0; i < maxJoysticks; ++i)
		{
#if UNITY_WEBPLAYER


#else
			PlayerIndex playerIndex = (PlayerIndex)i;

			XInputDotNetPure.GamePadState state = GamePad.GetState(playerIndex);

			if (state.IsConnected)
			{
				// Check if it is already attached

				bool alreadyExists = false;

				foreach (InputDevice d in devices)
				{
					if (d.Name == XInputDevice.xboxName + i)
					{
						// It already exists
						alreadyExists = true;
						break;
					}
				}

				if (!alreadyExists)
				{
					AttachDevice(new XInputDevice(i));
				}
			}
#endif
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
					if (d.Name == XInputDevice.xboxName + i)
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
		if (i < Devices.Count)
		{
			return Devices[i];
		}
		return null;
	}

    public static InputDevice GetNextUnusedDevice()
    {
        foreach (InputDevice d in devices)
        {
            if (!d.isJoystick)
            {
                continue;
            }

            if (d.InUse)
            {
                continue;
            }
            else
            {
                return d;
            }
        }

        if (!KeyBoard.InUse)
        {
            return KeyBoard;
        }

        return null;
    }

	public static InputDevice GetAnySafeDevice()
	{
		foreach (InputDevice d in devices)
		{
			if (d != null && d.isJoystick)
			{
				return d;
			}
		}

		if (KeyBoard != null)
		{
			return KeyBoard;
		}

		return null;
	}

	static void AttachDevice(InputDevice inputDevice)
	{
		devices.Add(inputDevice);

		if (InputManager.debugMessages)
		{
			Debug.Log("Connected: " + inputDevice.Name);
		}

		if (OnDeviceAttached != null)
		{
			OnDeviceAttached(inputDevice);
		}
	}

	static void DetachDevice(InputDevice inputDevice)
	{
		//devices.Remove(inputDevice);
		Debug.Log("Disconnected: " + inputDevice.Name);

		inputDevice.IsConnected = false;
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