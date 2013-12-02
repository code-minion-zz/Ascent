using UnityEngine;
using System;
using System.Collections;

//#pragma warning disable 0162

public class KeyboardInputDevice : InputDevice
{
	public const string keyboardName = "Keyboard";

	const int maxAnalogs = 6;
	const int maxButtons = 14;

	public KeyboardInputDevice()
	{
		this.name = keyboardName;

		Initialise();
	}

	/// <summary>
	/// Converts keyboard directional input to equivalent Horizontal/Vertical input
	/// </summary>
	/// <param name="target">Target Axis</param>
	protected override float GetAnalogValue(Enum target)
	{
		float value = 0.0f;

		InputDevice.InputControlType type = (InputDevice.InputControlType)target;

		switch (type)
		{

			case InputControlType.LeftStickX:
				{
					if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
					{
						value = 0.0f;
					}
					else if (Input.GetKey(KeyCode.LeftArrow))
					{
						value = -1.0f;
					}
					else if (Input.GetKey(KeyCode.RightArrow))
					{
						value = 1.0f;
					}
				}
				break;
			case InputControlType.LeftStickY:
				{
					if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
					{
						value = 0.0f;
                    }
					else if (Input.GetKey(KeyCode.DownArrow))
					{
						value = -1.0f;
					}
					else if (Input.GetKey(KeyCode.UpArrow))
					{
						value = 1.0f;
					}
				}
				break;
			case InputControlType.RightStickX:
				{
					if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.L))
					{
						value = 0.0f;
					}
					else if (Input.GetKey(KeyCode.J))
					{
						value = -1.0f;
					}
					else if (Input.GetKey(KeyCode.L))
					{
						value = 1.0f;
					}
				}
				break;
			case InputControlType.RightStickY:
				{
					if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.K))
					{
						value = 0.0f;
					}
					else if (Input.GetKey(KeyCode.K))
					{
						value = -1.0f;
					}
					else if (Input.GetKey(KeyCode.I))
					{
						value = 1.0f;
					}
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

	// TODO: This belongs in UnityInputDevice
	protected override bool GetButtonState(Enum target)
	{
		bool buttonState = false;

		InputDevice.InputControlType type = (InputDevice.InputControlType)target;

		switch (type)
		{
			case InputControlType.Action1: { buttonState = (Input.GetKey(KeyCode.A) ? true : false); } break;
			case InputControlType.Action2: { buttonState = ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Backspace)) ? true : false); } break;
			case InputControlType.Action3: { buttonState = (Input.GetKey(KeyCode.D) ? true : false); } break;
			case InputControlType.Action4: { buttonState = (Input.GetKey(KeyCode.F) ? true : false); } break;

			case InputControlType.LeftBumper: { buttonState = (Input.GetKey(KeyCode.Q) ? true : false); } break;
			case InputControlType.RightBumper: { buttonState = (Input.GetKey(KeyCode.W) ? true : false); } break;

            case InputControlType.DPadUp: { buttonState = (Input.GetKey(KeyCode.Alpha1) ? true : false); } break;
            case InputControlType.DPadDown: { buttonState = (Input.GetKey(KeyCode.Alpha2) ? true : false); } break;
            case InputControlType.DPadLeft: { buttonState = (Input.GetKey(KeyCode.Alpha3) ? true : false); } break;
            case InputControlType.DPadRight: { buttonState = (Input.GetKey(KeyCode.Alpha4) ? true : false); } break;

			case InputControlType.LeftStickButton: { buttonState = (Input.GetKey(KeyCode.E) ? true : false); } break;
			case InputControlType.RightStickButton: { buttonState = (Input.GetKey(KeyCode.R) ? true : false); } break;

			case InputControlType.Back: { buttonState = (Input.GetKey(KeyCode.RightShift) ? true : false); } break;
			case InputControlType.Start: { buttonState = (Input.GetKey(KeyCode.Return) ? true : false); } break;
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
