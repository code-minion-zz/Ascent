using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

#pragma warning disable 0162
#pragma warning disable 0429

public class Xbox360InputDevice : InputDevice
{
	public const string xboxName = "XBox360 ";
	private PlayerIndex gamePadID;

	const int maxAnalogs = 6;
	const int maxButtons = 14;

	GamePadState state;

	public Xbox360InputDevice(int id)
	{
		gamePadID = (PlayerIndex)id;
		name = xboxName + id;

		Initialise();
	}

	// Update is called once per frame
	public override void Update()
	{
		state = GamePad.GetState(gamePadID);

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
					value = state.ThumbSticks.Left.X;
				}
				break;
			case InputControlType.LeftStickY:
				{
					value = state.ThumbSticks.Left.Y;
				}
				break;
			case InputControlType.RightStickX:
				{
					value = state.ThumbSticks.Right.X;
				}
				break;
			case InputControlType.RightStickY:
				{
					value = state.ThumbSticks.Right.Y;
				}
				break;
			case InputControlType.LeftTrigger:
				{
					value = state.Triggers.Left;
				}
				break;
			case InputControlType.RightTrigger:
				{
					value = state.Triggers.Right;
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
			case InputControlType.Action1: { buttonState = (state.Buttons.A == ButtonState.Pressed ? true : false); } break;
			case InputControlType.Action2: { buttonState = (state.Buttons.B == ButtonState.Pressed ? true : false); } break;
			case InputControlType.Action3: { buttonState = (state.Buttons.X == ButtonState.Pressed ? true : false); } break;
			case InputControlType.Action4: { buttonState = (state.Buttons.Y == ButtonState.Pressed ? true : false); } break;

			case InputControlType.LeftBumper: { buttonState = (state.Buttons.LeftShoulder == ButtonState.Pressed ? true : false); } break;
			case InputControlType.RightBumper: { buttonState = (state.Buttons.RightShoulder == ButtonState.Pressed ? true : false); } break;

			case InputControlType.DPadUp: { buttonState = (state.DPad.Up == ButtonState.Pressed ? true : false); } break;
			case InputControlType.DPadDown: { buttonState = (state.DPad.Down == ButtonState.Pressed ? true : false); } break;
			case InputControlType.DPadLeft: { buttonState = (state.DPad.Left == ButtonState.Pressed ? true : false); } break;
			case InputControlType.DPadRight: { buttonState = (state.DPad.Right == ButtonState.Pressed ? true : false); } break;

			case InputControlType.LeftStickButton: { buttonState = (state.Buttons.LeftStick == ButtonState.Pressed ? true : false); } break;
			case InputControlType.RightStickButton: { buttonState = (state.Buttons.RightStick == ButtonState.Pressed ? true : false); } break;

			case InputControlType.Back: { buttonState = (state.Buttons.Back == ButtonState.Pressed ? true : false); } break;
			case InputControlType.Start: { buttonState = (state.Buttons.Start == ButtonState.Pressed ? true : false); } break;
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
