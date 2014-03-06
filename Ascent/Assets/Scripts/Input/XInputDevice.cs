using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

#pragma warning disable 0162
#pragma warning disable 0429

public class XInputDevice : InputDevice
{
	public enum EVibrationCurve
	{
		None,
		Bell,
		Descending,
		Ascending,
		Constant
	}
	
	public float VibrationDuration;
	public float VibrationElapsed;
	public float VibrationPowerMod;
	public EVibrationCurve VibrationMode;

	public const string xboxName = "XInput ";
	private PlayerIndex gamePadID;

	const int maxAnalogs = 6;
	const int maxButtons = 14;

	GamePadState state;

	public XInputDevice(int id)
	{
		gamePadID = (PlayerIndex)id;
		name = xboxName + id;

		Initialise();
	}

	// Update is called once per frame
	public override void Update()
	{
		VibrationElapsed += Time.deltaTime;
		state = GamePad.GetState(gamePadID);
		if (VibrationMode > EVibrationCurve.None) ProcessVibration();

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
	
	public override void SendDisconnectionEvent()
	{
		base.SendDisconnectionEvent();

		EndVibration();
	}

	public void EndVibration()
	{
		VibrationDuration = 0f;
		VibrationMode = EVibrationCurve.None;
		Vibrate (0f,0f);
	}

	public void StartVibration(EVibrationCurve curve, float power, float duration)
	{
		VibrationMode = curve;
		VibrationPowerMod = power;
		VibrationDuration = duration;
	}

	protected void ProcessVibration()
	{
		float power = 0f;

		switch (VibrationMode)
		{
		case EVibrationCurve.Ascending:
			power = Mathf.Sin(VibrationElapsed/VibrationDuration);
			break;
		case EVibrationCurve.Constant:
			power = 1f;
			break;
		case EVibrationCurve.Descending:
			power = 1 - Mathf.Sin(VibrationElapsed/VibrationDuration);
			break;
		}
//		float left;
//		float right;
//		left = Mathf.Abs(Mathf.Cos(Time.time));
//		right = Mathf.Abs(Mathf.Sin(Time.time));
//		left = 0;
//		right = 0;
		if (VibrationElapsed > VibrationDuration)
		{
			power = 0f;
			VibrationElapsed = 0f;
			VibrationDuration = 0f;
			VibrationMode = EVibrationCurve.None;
		}
		power *= VibrationPowerMod;
		Debug.Log(VibrationElapsed/VibrationDuration + " " + power);
		Vibrate(power,power);
	}

	protected void Vibrate(float left, float right)
	{
		GamePad.SetVibration(gamePadID,left,right);
	}
}
