// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class AscentInput
{
	private InControl.InputDevice device;

	public delegate void AscentInputEventHandler(InControl.InputDevice device);

	// D-Pad
	public event AscentInputEventHandler OnDPadUp;
	public event AscentInputEventHandler OnDPadDown;
	public event AscentInputEventHandler OnDPadLeft;
	public event AscentInputEventHandler OnDPadRight;

	public event AscentInputEventHandler OnDPadUp_up;
	public event AscentInputEventHandler OnDPadDown_up;
	public event AscentInputEventHandler OnDPadLeft_up;
	public event AscentInputEventHandler OnDPadRight_up;

	// L-Stick
	public event AscentInputEventHandler OnLStickUp;
	public event AscentInputEventHandler OnLStickDown;
	public event AscentInputEventHandler OnLStickLeft;
	public event AscentInputEventHandler OnLStickRight;

	//// R-Stick
	//public event AscentInputEventHandler OnRStickUp;
	//public event AscentInputEventHandler OnRStickDown;
	//public event AscentInputEventHandler OnRStickLeft;
	//public event AscentInputEventHandler OnRStickRight;

	//// Front face butons
	//public event AscentInputEventHandler OnX;
	//public event AscentInputEventHandler OnY;
	//public event AscentInputEventHandler OnA;
	//public event AscentInputEventHandler OnB;
	//public event AscentInputEventHandler OnStart;
	//public event AscentInputEventHandler OnBack;

	//public event AscentInputEventHandler OnX_up;
	//public event AscentInputEventHandler OnY_up;
	//public event AscentInputEventHandler OnA_up;
	//public event AscentInputEventHandler OnB_up;
	//public event AscentInputEventHandler OnStart_up;
	//public event AscentInputEventHandler OnBack_up;

	//// Bumpers and triggers
	//public event AscentInputEventHandler OnLeftTrigger;
	//public event AscentInputEventHandler OnLeftBumper;
	//public event AscentInputEventHandler OnRightTrigger;
	//public event AscentInputEventHandler OnRightBumper;

	//public event AscentInputEventHandler OnLeftTrigger_up;
	//public event AscentInputEventHandler OnLeftBumper_up;
	//public event AscentInputEventHandler OnRightTrigger_up;
	//public event AscentInputEventHandler OnRightBumper_up;


	public void Initialise(InControl.InputDevice device)
	{
		this.device = device;
	}

	public void Update()
	{
		if (device == null)
		{
			Debug.Log("Put something in here to handle the disconnection");
			return;
		}

		// DPad down
		if (device.DPadUp.WasPressed && OnDPadUp != null)
		{
			OnDPadUp.Invoke(device);
		}
		else if (device.DPadDown.WasPressed && OnDPadDown != null)
		{
			OnDPadDown.Invoke(device);
		}
		if (device.DPadLeft.WasPressed && OnDPadLeft != null)
		{
			OnDPadLeft.Invoke(device);
		}
		else if (device.DPadRight.WasPressed && OnDPadRight != null)
		{
			OnDPadRight.Invoke(device);
		}

		// DPad up
		if (device.DPadUp.WasReleased && OnDPadUp_up != null)
		{
			OnDPadUp_up.Invoke(device);
		}
		else if (device.DPadDown.WasReleased && OnDPadDown_up != null)
		{
			OnDPadDown_up.Invoke(device);
		}
		if (device.DPadLeft.WasReleased && OnDPadLeft_up != null)
		{
			OnDPadLeft_up.Invoke(device);
		}
		else if (device.DPadRight.WasReleased && OnDPadRight_up != null)
		{
			OnDPadRight_up.Invoke(device);
		}

		// L-Stick
		if (device.LeftStickY.IsNotNull && OnLStickUp != null)
		{
			OnLStickUp.Invoke(device);
		}
		else if (device.LeftStickY.IsNotNull && OnLStickDown != null)
		{
			OnLStickDown.Invoke(device);
		}
		if (device.LeftStickY.IsNotNull && OnLStickLeft != null)
		{
			OnLStickLeft.Invoke(device);
		}
		else if (device.LeftStickY.IsNotNull && OnLStickRight != null)
		{
			OnLStickRight.Invoke(device);
		}
	}

}
