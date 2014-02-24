//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Panel plus code to handle Radial menu input
/// </summary>
public class UITown_RadialPanel : UITown_Panel
{
	protected Dictionary<float, int> AngleIndex;
	protected bool updatePointer = false;

	public override void Initialise ()
	{
		base.Initialise ();
		
		AngleIndex = new Dictionary<float, int>();

	}

	/// <summary>
	/// Check if arrow is pointing at a button, and highlights it.
	/// Returns false if no button is highlighted.
	/// </summary>
	protected virtual bool HighlightButton ()
	{
		float angle = (parent as UITownWindow).PointerAngle - 90f;

		bool hit = false; // True if at least one match is made

		foreach (KeyValuePair<float,int> p in AngleIndex)
		{
			if (Utilities.CloseTo(angle,p.Key, 20f))
			{
				hit = true;
				if (buttons[p.Value] != currentSelection)
				{
					if (currentSelection)
					{
						UICamera.Notify(currentSelection.gameObject, "OnHover", false);
					}
					currentSelection = buttons[p.Value];
					currentHighlightedButton = p.Value;
					//Debug.Log("Button Highlighted :" + currentSelection.gameObject + " Angle:" + angle + " against:" + p.Key);
				}
				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			}
		}

		// comment out next line for 'sticky selection' behaviour
		if (!hit) // Removes button selection if no longer in range
		{
			if (currentSelection)
			{
				UICamera.Notify(currentSelection.gameObject, "OnHover", false);
			}
			currentHighlightedButton = -1;
			currentSelection = null;

			return false;
		}
		return true;
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (currentSelection != null) 
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", false);
			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
		}
	}



	/// <summary>
	/// Does input satisfy deadzone requirements?
	/// 0up, 1left, 2down, 3right
	/// </summary>
	protected static bool SatisfiesDeadzone(InputDevice device, int direction)
	{		
		bool satisfied = false;
		float checkAngle = 0f;
		InputControl dPadButton = null;
		
		switch (direction)
		{
		case 0:
			dPadButton = device.DPadUp;
			checkAngle = 0f;
			break;
		case 1:
			dPadButton = device.DPadLeft;
			checkAngle = 90f;
			break;
		case 2:
			dPadButton = device.DPadDown;
			checkAngle = 180f;
			break;
		case 3:
			dPadButton = device.DPadRight;
			checkAngle = 270f;
			break;
		default:
			return false;
		}
		
		if (device.LeftStickX.IsPressed || device.LeftStickY.IsPressed)
		{
			// -90 = right, 0 = up, 90 = left, -180 = down
			//bool facingLeft = false; // @KIT commented this field and statement below because it was causing a warning. It is unused anyway? - 22/02/2014
			float angle = Utilities.VectorToAngleInDegrees(device.LeftStickX.Value, device.LeftStickY.Value) -90f;
			//if (angle > 0) facingLeft = true;
			
			satisfied = Utilities.CloseTo(angle, checkAngle, 10f);
			//Debug.Log (angle + " passes Deadzone? :" +satisfied);
			
		}
		else if (dPadButton.IsPressed)
		{
			satisfied = true;
		}
		
		return satisfied;
	}
}


