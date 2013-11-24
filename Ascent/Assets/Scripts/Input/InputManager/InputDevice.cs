using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDevice
{
	public enum InputControlType
	{
		LeftStickX,
		LeftStickY,
		LeftStickButton,

		RightStickX,
		RightStickY,
		RightStickButton,

		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,

		Action1,
		Action2,
		Action3,
		Action4,

		LeftTrigger,
		RightTrigger,

		LeftBumper,
		RightBumper,

		Back,
		Start,

		Null,
	}

	public static readonly InputDevice Null = new InputDevice("NullInputDevice");

	protected string name;
	public string Name
	{
		get { return name; }
	}

	public bool isJoystick;

	bool isInUse = false;

	protected Dictionary<int, InputControl> controlTable = new Dictionary<int, InputControl>();

	public Dictionary<int, InputControl> analogs = new Dictionary<int, InputControl>();
	public Dictionary<int, InputControl> Analogs { get { return analogs; } protected set { analogs = value; } }

	public Dictionary<int, InputControl> buttons = new Dictionary<int, InputControl>();
	public Dictionary<int, InputControl> Buttons { get { return buttons; } protected set { buttons = value; } }

	int filledAnalogCount = 0;
	int filledButtonCount = 0;

	public bool InUse
	{
		get { return isInUse; }
		set { isInUse = value; }
	}

	public InputDevice(string name)
	{
		this.name = name;
	}

	public InputDevice()
	{
	}

	public InputDevice(bool isJoystick)
	{
		this.isJoystick = isJoystick;
	}

	public bool IsConnected()
	{
		return (false);
	}

	protected void Initialise()
	{
		AddAnalogControl(InputControlType.LeftStickX);
		AddAnalogControl(InputControlType.LeftStickY);
		AddAnalogControl(InputControlType.RightStickX);
		AddAnalogControl(InputControlType.RightStickY);
		AddAnalogControl(InputControlType.LeftTrigger);
		AddAnalogControl(InputControlType.RightTrigger);

		AddButtonControl(InputControlType.Action1);
		AddButtonControl(InputControlType.Action2);
		AddButtonControl(InputControlType.Action3);
		AddButtonControl(InputControlType.Action4);

		AddButtonControl(InputControlType.DPadUp);
		AddButtonControl(InputControlType.DPadDown);
		AddButtonControl(InputControlType.DPadLeft);
		AddButtonControl(InputControlType.DPadRight);

		AddButtonControl(InputControlType.LeftBumper);
		AddButtonControl(InputControlType.RightBumper);

		AddButtonControl(InputControlType.Back);
		AddButtonControl(InputControlType.Start);

		AddButtonControl(InputControlType.LeftStickButton);
		AddButtonControl(InputControlType.RightStickButton);
	}

	public virtual void Update()
	{
		foreach (KeyValuePair<int, InputControl> control in Analogs)
		{
			control.Value.UpdateWithValue(GetAnalogValue(control.Value.type));
		}

		foreach (KeyValuePair<int, InputControl> control in Buttons)
		{
			control.Value.UpdateWithState(GetButtonState(control.Value.type));
		}
	}

	public InputControl GetControl(Enum inputControlType)
	{
		int controlIndex = Convert.ToInt32(inputControlType);
		var control = controlTable[controlIndex];
		return control == null ? InputControl.Null : control;
	}


	public void AddAnalogControl(Enum target)
	{
		SetAnalogControl(filledAnalogCount++, target);
	}


	public void SetAnalogControl(int i, Enum target)
	{
		Analogs.Add(i, new InputControl(target));
		var controlIndex = Convert.ToInt32(target);
		controlTable.Add(controlIndex, Analogs[i]);
	}


	public void AddButtonControl(Enum target)
	{
		SetButtonControl(filledButtonCount++, target);
	}

	// TODO: This belongs in UnityInputDevice
	protected virtual float GetAnalogValue(Enum target)
	{
		return 0.0f;
	}


	// TODO: This belongs in UnityInputDevice
	protected virtual bool GetButtonState(Enum target)
	{
		return false;
	}


	public void SetButtonControl(int i, Enum target)
	{
		Buttons.Add(i, new InputControl(target));
		var controlIndex = Convert.ToInt32(target);
		controlTable[controlIndex] = Buttons[i];
	}

	public InputControl LeftStickX { get { return GetControl(InputControlType.LeftStickX); } }
	public InputControl LeftStickY { get { return GetControl(InputControlType.LeftStickY); } }
	public InputControl LeftStickButton { get { return GetControl(InputControlType.LeftStickButton); } }

	public InputControl RightStickX { get { return GetControl(InputControlType.RightStickX); } }
	public InputControl RightStickY { get { return GetControl(InputControlType.RightStickY); } }
	public InputControl RightStickButton { get { return GetControl(InputControlType.RightStickButton); } }

	public InputControl DPadUp { get { return GetControl(InputControlType.DPadUp); } }
	public InputControl DPadDown { get { return GetControl(InputControlType.DPadDown); } }
	public InputControl DPadLeft { get { return GetControl(InputControlType.DPadLeft); } }
	public InputControl DPadRight { get { return GetControl(InputControlType.DPadRight); } }

	public InputControl Action1 { get { return GetControl(InputControlType.Action1); } }
	public InputControl Action2 { get { return GetControl(InputControlType.Action2); } }
	public InputControl Action3 { get { return GetControl(InputControlType.Action3); } }
	public InputControl Action4 { get { return GetControl(InputControlType.Action4); } }

	public InputControl A { get { return GetControl(InputControlType.Action1); } }
	public InputControl B { get { return GetControl(InputControlType.Action2); } }
	public InputControl X { get { return GetControl(InputControlType.Action3); } }
	public InputControl Y { get { return GetControl(InputControlType.Action4); } }

	public InputControl Start { get { return GetControl(InputControlType.Start); } }
	public InputControl Back { get { return GetControl(InputControlType.Back); } }

	public InputControl LeftTrigger { get { return GetControl(InputControlType.LeftTrigger); } }
	public InputControl RightTrigger { get { return GetControl(InputControlType.RightTrigger); } }

	public InputControl LeftBumper { get { return GetControl(InputControlType.LeftBumper); } }
	public InputControl RightBumper { get { return GetControl(InputControlType.RightBumper); } }
}
