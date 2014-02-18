using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

#pragma warning disable 0162
#pragma warning disable 0429
#pragma warning disable 0414

public class XBox360InputDevice : InputDevice
{
	public const string xboxName = "XBox360 ";
	private PlayerIndex gamePadID;

	const int maxAnalogs = 6;
	const int maxButtons = 14;

	public XBox360InputDevice(int id)
	{
		gamePadID = (PlayerIndex)id;
		name = xboxName + id;

		Initialise();
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

	public override void UpdateEvents()
	{
		Update();
		base.UpdateEvents();
	}

	protected override float GetAnalogValue(Enum target)
	{
		float value = 0.0f;

		InputDevice.InputControlType type = (InputDevice.InputControlType)target;

		switch (type)
		{
			case InputControlType.LeftStickX:
				{
					value = Input.GetAxisRaw("P1 Left Axis X");
					//Debug.Log(value);
				}
				break;
			case InputControlType.LeftStickY:
				{
					value = Input.GetAxisRaw("P1 Left Axis Y");
				}
				break;
			//case InputControlType.RightStickX:
			//    {
			//        value = Input.GetAxisRaw("P1 Right Axis X");
			//    }
			//    break;
			//case InputControlType.RightStickY:
			//    {
			//        value = Input.GetAxisRaw("P1 Right Axis Y");
			//    }
			//    break;
			case InputControlType.LeftTrigger:
				{
					value = Input.GetAxisRaw("P1 Left Trigger");
				}
				break;
			case InputControlType.RightTrigger:
				{
					value = Input.GetAxisRaw("P1 Right Trigger");
				}
				break;

		}

		if (InputManager.debugMessages)
		{
			if (value != 0.0f)
			{
				Debug.Log(name + " " + type + ": " + value);
			}
		}

		return value;
	}

	protected override bool GetButtonState(Enum target)
	{
		bool buttonState = false;

		InputDevice.InputControlType type = (InputDevice.InputControlType)target;
		switch (type)
		{
			case InputControlType.Action1: { buttonState = (Input.GetButton("P1 A") ? true : false); } break;
			case InputControlType.Action2: { buttonState = (Input.GetButton("P1 B") ? true : false); } break;
			case InputControlType.Action3: { buttonState = (Input.GetButton("P1 X") ? true : false); } break;
			case InputControlType.Action4: { buttonState = (Input.GetButton("P1 Y") ? true : false); } break;

			case InputControlType.LeftBumper: { buttonState = (Input.GetButton("P1 Left Bumper") ? true : false); } break;
			case InputControlType.RightBumper: { buttonState = (Input.GetButton("P1 Right Bumper") ? true : false); } break;

			case InputControlType.DPadUp: { buttonState = (Input.GetAxis("P1 DPad Axis Y") > 0.0f ? true : false); } break;
			case InputControlType.DPadDown: { buttonState = (Input.GetAxis("P1 DPad Axis Y") < 0.0f ? true : false); } break;
			case InputControlType.DPadLeft: { buttonState = (Input.GetAxis("P1 DPad Axis X") > 0.0f ? true : false); } break;
			case InputControlType.DPadRight: { buttonState = (Input.GetAxis("P1 DPad Axis X") < 0.0f ? true : false); } break;

			case InputControlType.LeftStickButton: { buttonState = (Input.GetButton("P1 Left Stick") ? true : false); } break;
			case InputControlType.RightStickButton: { buttonState = (Input.GetButton("P1 Right Stick") ? true : false); } break;

			case InputControlType.Back: { buttonState = (Input.GetButton("P1 Back") ? true : false); } break;
			case InputControlType.Start: { buttonState = (Input.GetButton("P1 Start") ? true : false); } break;
		}

		if (InputManager.debugMessages)
		{
			if (buttonState)
			{
				Debug.Log(name + " " + type + ": " + buttonState);
			}
		}

		return buttonState;
	}
}
