// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class AscentInput
{
	private InControl.InputDevice device;
	public InControl.InputDevice Device
	{
		get { return device; }
	}

	public delegate void AscentInputEventHandler(ref InControl.InputDevice device);

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
	public event AscentInputEventHandler OnLStickMove;
    public event AscentInputEventHandler OnLStick;
	public event AscentInputEventHandler OnLStick_up;

	// R-Stick
    public event AscentInputEventHandler OnRStickMove;
    public event AscentInputEventHandler OnRStick;
    public event AscentInputEventHandler OnRStick_up;

	//// Front face butons
	public event AscentInputEventHandler OnX;
	public event AscentInputEventHandler OnY;
	public event AscentInputEventHandler OnA;
	public event AscentInputEventHandler OnB;
    public event AscentInputEventHandler OnStart;
    public event AscentInputEventHandler OnBack;

    public event AscentInputEventHandler OnX_up;
    public event AscentInputEventHandler OnY_up;
    public event AscentInputEventHandler OnA_up;
    public event AscentInputEventHandler OnB_up;
    public event AscentInputEventHandler OnStart_up;
    public event AscentInputEventHandler OnBack_up;

    // Bumpers and triggers
    public event AscentInputEventHandler OnLeftTrigger;
    public event AscentInputEventHandler OnLeftBumper;
    public event AscentInputEventHandler OnRightTrigger;
    public event AscentInputEventHandler OnRightBumper;

    public event AscentInputEventHandler OnLeftTrigger_up;
    public event AscentInputEventHandler OnLeftBumper_up;
    public event AscentInputEventHandler OnRightTrigger_up;
    public event AscentInputEventHandler OnRightBumper_up;


	public void Initialise(InControl.InputDevice device)
	{
		this.device = device;
	}

	public void Update()
	{
        // TODO: Handle device detachment
        if (device == null)
        {
            Debug.Log("Put something in here to handle the disconnection");
            return;
        }

        #region DPad
        // DPad down
        if (device.DPadUp.WasPressed && OnDPadUp != null)
        {
            OnDPadUp.Invoke(ref device);
        }
        else if (device.DPadDown.WasPressed && OnDPadDown != null)
        {
            OnDPadDown.Invoke(ref device);
        }
        if (device.DPadLeft.WasPressed && OnDPadLeft != null)
        {
            OnDPadLeft.Invoke(ref device);
        }
        else if (device.DPadRight.WasPressed && OnDPadRight != null)
        {
            OnDPadRight.Invoke(ref device);
        }

        // DPad up
        if (device.DPadUp.WasReleased && OnDPadUp_up != null)
        {
            OnDPadUp_up.Invoke(ref device);
        }
        else if (device.DPadDown.WasReleased && OnDPadDown_up != null)
        {
            OnDPadDown_up.Invoke(ref device);
        }
        if (device.DPadLeft.WasReleased && OnDPadLeft_up != null)
        {
            OnDPadLeft_up.Invoke(ref device);
        }
        else if (device.DPadRight.WasReleased && OnDPadRight_up != null)
        {
            OnDPadRight_up.Invoke(ref device);
        }

        #endregion

        #region L-Stick

        // L-Stick
        if ((device.LeftStickX.IsNotNull || device.LeftStickY.IsNotNull) && OnLStickMove != null)
        {
            OnLStickMove.Invoke(ref device);
        }

        // L-Stick button
        if (device.LeftStickButton.WasPressed && OnLStick != null)
        {
            OnLStick.Invoke(ref device);
        }
        else if (device.LeftStickButton.WasReleased && OnLStick_up != null)
        {
            OnLStick_up.Invoke(ref device);
        }

        #endregion

        #region R-Stick

        // R-Stick
        if ((device.RightStickY.IsNotNull || device.RightStickY.IsNotNull) && OnRStickMove != null)
        {
            OnRStick.Invoke(ref device);
        }

        // R-Stick button
        if (device.RightStickButton.WasPressed && OnRStick != null)
        {
            OnLStick.Invoke(ref device);
        }
        else if (device.RightStickButton.WasReleased && OnRStick_up != null)
        {
            OnLStick_up.Invoke(ref device);
        }

        #endregion

        #region FaceButtons

        // Face Buttons
        // A
        if (device.Action1.WasPressed && OnA != null)
        {

            OnA.Invoke(ref device);
        }
        else if (device.Action1.WasReleased && OnA_up != null)
        {
            OnA_up.Invoke(ref device);
        }

        // B
        if (device.Action2.WasPressed && OnB != null)
        {
            OnB.Invoke(ref device);
        }
        else if (device.Action1.WasReleased && OnB_up != null)
        {
            OnB_up.Invoke(ref device);
        }

        // X
        if (device.Action3.WasPressed && OnX != null)
        {
            OnX.Invoke(ref device);
        }
        else if (device.Action1.WasReleased && OnX_up != null)
        {
            OnX_up.Invoke(ref device);
        }

        // Y
        if (device.Action4.WasPressed && OnY != null)
        {
            OnY.Invoke(ref device);
        }
        else if (device.Action1.WasReleased && OnY_up != null)
        {
            OnY_up.Invoke(ref device);
        }

        // Start
        if (device.Buttons[7].WasPressed && OnStart != null)
        {
            OnStart.Invoke(ref device);
        }
        else if (device.Buttons[7].WasReleased && OnStart_up != null)
        {
            OnStart_up.Invoke(ref device);
        }

        // Back
        if (device.Buttons[6].WasPressed && OnBack != null)
        {
            OnBack.Invoke(ref device);
        }
        else if (device.Buttons[6].WasReleased && OnBack_up != null)
        {
            OnBack_up.Invoke(ref device);
        }

        #endregion

        #region Triggers

        // Triggers
        if (device.LeftTrigger.WasPressed && OnLeftTrigger != null)
        {
            OnLeftTrigger.Invoke(ref device);
        }

        if (device.LeftTrigger.WasReleased && OnLeftTrigger_up != null)
        {
            OnLeftTrigger_up.Invoke(ref device);
        }

        if (device.RightTrigger.WasPressed && OnRightTrigger != null)
        {
            OnRightTrigger.Invoke(ref device);
        }

        if (device.RightTrigger.WasReleased && OnRightTrigger_up != null)
        {
            OnRightTrigger_up.Invoke(ref device);
        }

        #endregion

        #region Bumpers

        // Bumpers
        if (device.LeftBumper.WasPressed && OnLeftBumper != null)
        {
            OnLeftBumper.Invoke(ref device);
        }

        if (device.LeftBumper.WasReleased && OnLeftBumper_up != null)
        {
            OnLeftBumper_up.Invoke(ref device);
        }

        if (device.RightBumper.WasPressed && OnRightBumper != null)
        {
            OnRightBumper.Invoke(ref device);
        }

        if (device.RightBumper.WasReleased && OnRightBumper_up != null)
        {
            OnRightBumper_up.Invoke(ref device);
        }

        #endregion

    }

}
