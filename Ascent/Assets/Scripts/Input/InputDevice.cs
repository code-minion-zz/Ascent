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

		Action1, // A
		Action2, // B
		Action3, // X
		Action4, // Y

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

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="InputDevice"/> in use.
	/// In use means 'bound to a player'.
	/// </summary>
	/// <value><c>true</c> if in use; otherwise, <c>false</c>.</value>
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

#region event driven device

	public delegate void InputDeviceEventHandler(InputDevice device);

	// Disconnect
	public event InputDeviceEventHandler OnDisconnected;

	// D-Pad
	public event InputDeviceEventHandler OnDPadUp;
	public event InputDeviceEventHandler OnDPadDown;
	public event InputDeviceEventHandler OnDPadLeft;
	public event InputDeviceEventHandler OnDPadRight;

	public event InputDeviceEventHandler OnDPadUp_up;
	public event InputDeviceEventHandler OnDPadDown_up;
	public event InputDeviceEventHandler OnDPadLeft_up;
	public event InputDeviceEventHandler OnDPadRight_up;

	// L-Stick
	public event InputDeviceEventHandler OnLStickMove;
	public event InputDeviceEventHandler OnLStick;
	public event InputDeviceEventHandler OnLStick_up;

	// R-Stick
	public event InputDeviceEventHandler OnRStickMove;
	public event InputDeviceEventHandler OnRStick;
	public event InputDeviceEventHandler OnRStick_up;

	//// Front face butons
	public event InputDeviceEventHandler OnX;
	public event InputDeviceEventHandler OnY;
	public event InputDeviceEventHandler OnA;
	public event InputDeviceEventHandler OnB;
	public event InputDeviceEventHandler OnStart;
	public event InputDeviceEventHandler OnBack;

	public event InputDeviceEventHandler OnX_up;
	public event InputDeviceEventHandler OnY_up;
	public event InputDeviceEventHandler OnA_up;
	public event InputDeviceEventHandler OnB_up;
	public event InputDeviceEventHandler OnStart_up;
	public event InputDeviceEventHandler OnBack_up;

	// Bumpers and triggers
	public event InputDeviceEventHandler OnLeftTrigger;
	public event InputDeviceEventHandler OnLeftBumper;
	public event InputDeviceEventHandler OnRightTrigger;
	public event InputDeviceEventHandler OnRightBumper;

	public event InputDeviceEventHandler OnLeftTrigger_up;
	public event InputDeviceEventHandler OnLeftBumper_up;
	public event InputDeviceEventHandler OnRightTrigger_up;
	public event InputDeviceEventHandler OnRightBumper_up;

	public void SendDisconnectionEvent()
	{
		if (OnDisconnected != null)
		{
			OnDisconnected.Invoke(this);
		}
	}

	public virtual void UpdateEvents()
	{
		#region DPad
		// DPad down
		if (DPadUp.WasPressed && OnDPadUp != null)
		{
			OnDPadUp.Invoke(this);
		}
		else if (DPadDown.WasPressed && OnDPadDown != null)
		{
			OnDPadDown.Invoke(this);
		}
		if (DPadLeft.WasPressed && OnDPadLeft != null)
		{
			OnDPadLeft.Invoke(this);
		}
		else if (DPadRight.WasPressed && OnDPadRight != null)
		{
			OnDPadRight.Invoke(this);
		}

		// DPad up
		if (DPadUp.WasReleased && OnDPadUp_up != null)
		{
			OnDPadUp_up.Invoke(this);
		}
		else if (DPadDown.WasReleased && OnDPadDown_up != null)
		{
			OnDPadDown_up.Invoke(this);
		}
		if (DPadLeft.WasReleased && OnDPadLeft_up != null)
		{
			OnDPadLeft_up.Invoke(this);
		}
		else if (DPadRight.WasReleased && OnDPadRight_up != null)
		{
			OnDPadRight_up.Invoke(this);
		}

		#endregion

		#region L-Stick

		// L-Stick
		if ((LeftStickX.IsNotNull || LeftStickY.IsNotNull) && OnLStickMove != null)
		{
			OnLStickMove.Invoke(this);
		}

		// L-Stick button
		if (LeftStickButton.WasPressed && OnLStick != null)
		{
			OnLStick.Invoke(this);
		}
		else if (LeftStickButton.WasReleased && OnLStick_up != null)
		{
			OnLStick_up.Invoke(this);
		}

		#endregion

		#region R-Stick

		// R-Stick
		if ((RightStickY.IsNotNull || RightStickY.IsNotNull) && OnRStickMove != null)
		{
			OnRStick.Invoke(this);
		}

		// R-Stick button
		if (RightStickButton.WasPressed && OnRStick != null)
		{
			OnLStick.Invoke(this);
		}
		else if (RightStickButton.WasReleased && OnRStick_up != null)
		{
			OnLStick_up.Invoke(this);
		}

		#endregion

		#region FaceButtons

		// Face Buttons
		// A
		if (Action1.WasPressed && OnA != null)
		{
			Debug.Log("asdasd");
			OnA.Invoke(this);
		}
		else if (Action1.WasReleased && OnA_up != null)
		{
			Debug.Log("asdasd");
			OnA_up.Invoke(this);
		}

		// B
		if (Action2.WasPressed && OnB != null)
		{
			Debug.Log("asdasd");
			OnB.Invoke(this);
		}
		else if (Action1.WasReleased && OnB_up != null)
		{
			Debug.Log("asdasd");
			OnB_up.Invoke(this);
		}

		// X
		if (Action3.WasPressed && OnX != null)
		{
			OnX.Invoke(this);
		}
		else if (Action1.WasReleased && OnX_up != null)
		{
			OnX_up.Invoke(this);
		}

		// Y
		if (Action4.WasPressed && OnY != null)
		{
			OnY.Invoke(this);
		}
		else if (Action1.WasReleased && OnY_up != null)
		{
			OnY_up.Invoke(this);
		}

		// Start
		if (Start.WasPressed && OnStart != null)
		{
			OnStart.Invoke(this);
		}
		else if (Start.WasReleased && OnStart_up != null)
		{
			OnStart_up.Invoke(this);
		}

		// Back
		if (Back.WasPressed && OnBack != null)
		{
			OnBack.Invoke(this);
		}
		else if (Back.WasReleased && OnBack_up != null)
		{
			OnBack_up.Invoke(this);
		}

		#endregion

		#region Triggers

		// Triggers
		if (LeftTrigger.WasPressed && OnLeftTrigger != null)
		{
			OnLeftTrigger.Invoke(this);
		}

		if (LeftTrigger.WasReleased && OnLeftTrigger_up != null)
		{
			OnLeftTrigger_up.Invoke(this);
		}

		if (RightTrigger.WasPressed && OnRightTrigger != null)
		{
			OnRightTrigger.Invoke(this);
		}

		if (RightTrigger.WasReleased && OnRightTrigger_up != null)
		{
			OnRightTrigger_up.Invoke(this);
		}

		#endregion

		#region Bumpers

		// Bumpers
		if (LeftBumper.WasPressed && OnLeftBumper != null)
		{
			OnLeftBumper.Invoke(this);
		}

		if (LeftBumper.WasReleased && OnLeftBumper_up != null)
		{
			OnLeftBumper_up.Invoke(this);
		}

		if (RightBumper.WasPressed && OnRightBumper != null)
		{
			OnRightBumper.Invoke(this);
		}

		if (RightBumper.WasReleased && OnRightBumper_up != null)
		{
			OnRightBumper_up.Invoke(this);
		}

		#endregion
	}

	#endregion

}
