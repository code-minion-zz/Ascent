using UnityEngine;
using System.Collections;

public class UICameraWrap : UICamera 
{
	InputDevice owner;	
	
	/// <summary>
	/// Init Menu with the specified device. This needs to be called to set the owner.
	/// Will not work otherwise.
	/// </summary>
	/// <param name="device">Device.</param>
	public void Init(InputDevice device)
	{
		owner = device;
		if (owner.isJoystick)
		{
			useKeyboard = false;
			useController = true;
		}
	}

	void ProcessOthers()
	{
		currentTouchID = -100;
		currentTouch = mController;

		// If this is an input field, ignore WASD and Space key presses
		inputHasFocus = (mCurrentSelection != null && mCurrentSelection.GetComponent<UIInput>() != null);
		
		bool submitKeyDown = false;
		bool submitKeyUp = false;

		// Fetch the Submit Button
		InputControl submitButton;
		// submitButton = owner.GetSubmit()
		submitButton = owner.A;

		// Check Button State for Submit Key		
		if (submitButton != InputControl.Null && submitButton.WasPressed)
		{
			currentKey = submitKey0;
			submitKeyDown = true;
		}		
		if (submitButton != InputControl.Null && submitButton.WasReleased)
		{
			currentKey = submitKey0;
			submitKeyUp = true;
		}
		
		if (submitKeyDown || submitKeyUp)
		{
			currentTouch.current = mCurrentSelection;
			ProcessTouch(submitKeyDown, submitKeyUp);
			currentTouch.current = null;
		}
		
		int vertical = 0;
		int horizontal = 0;
		
		if (useKeyboard)
		{
			if (inputHasFocus)
			{
				// TODO : find a way to make keyboard ignore character keys like WSAD
				vertical += (int)owner.LeftStickY.Value;
				horizontal += (int)owner.LeftStickX.Value;
			}
			else
			{
				vertical += (int)owner.LeftStickY.Value;
				horizontal += (int)owner.LeftStickX.Value;
			}
		}
		
		if (useController)
		{
			// Y Axis Leftstick / UpDown
			if (owner.LeftStickY.IsNotNull) vertical += (int)owner.LeftStickY.Value;
			if (owner.DPadUp.WasPressed) vertical += 1;
			if (owner.DPadDown.WasPressed) vertical -= 1;
			// X Axis Leftstick / LeftRight
			if (owner.LeftStickX.IsNotNull) horizontal += (int)owner.LeftStickX.Value;
			if (owner.DPadLeft.WasPressed) horizontal -= 1;
			if (owner.DPadRight.WasPressed) horizontal += 1;
		}
		
		// Send out key notifications
		if (vertical != 0) Notify(mCurrentSelection, "OnKey", vertical > 0 ? KeyCode.UpArrow : KeyCode.DownArrow);
		if (horizontal != 0) Notify(mCurrentSelection, "OnKey", horizontal > 0 ? KeyCode.RightArrow : KeyCode.LeftArrow);
		
		if (useKeyboard && Input.GetKeyDown(KeyCode.Tab))
		{
			currentKey = KeyCode.Tab;
			Notify(mCurrentSelection, "OnKey", KeyCode.Tab);
		}

		// Fetch the Cancel Button
		InputControl cancelButton;
		// cancelButton = owner.GetCancel()
		cancelButton = owner.B;

		// On Release : Send out the cancel key notification
		if (cancelButton != InputControl.Null && cancelButton.WasReleased)
		{
			currentKey = cancelKey0;
			Notify(mCurrentSelection, "OnKey", KeyCode.Escape);
		}
		
		currentTouch = null;
		currentKey = KeyCode.None;
	}
}
