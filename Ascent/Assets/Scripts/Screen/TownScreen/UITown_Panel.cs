using System.Collections.Generic;
using UnityEngine;

public class UITown_Panel : UIPlayerMenuPanel
{
	// This class identifies a panel as belonging to the Town scene
	protected bool parentConfirming = false;
	protected bool confirmBoxResult = false;
	protected UITownWindow townParent;
	
	/// <summary>
	/// Returns item on currently selected item button, Null if not possible
	/// </summary>
	/// <value>The current item.</value>
	protected virtual Item CurrentItem
	{
		get
		{
			Item retval = null;
			if (currentSelection)
			{
				if (currentSelection is UIItemButton)
				{
					retval = (currentSelection as UIItemButton).LinkedItem; 
				}
			}
			return retval;
		}
	}

	public override void Initialise() 
	{
		base.Initialise();

		townParent = parent as UITownWindow;

		(parent as UITownWindow).PopupClose += OnConfirmBoxClose;
	}
	
	protected virtual void OpenConfirmBox(string str)
	{
		parentConfirming = true;
		townParent.RequestConfirmBox(str);
	}

	protected virtual void OnConfirmBoxClose(bool result)
	{
		parentConfirming = false;
		confirmBoxResult = result;
		Debug.Log("OnConfirmBoxClose:"+confirmBoxResult);
	}
		
	public override void OnMenuCancel(InputDevice device)
	{
		townParent.RequestTransitionToPanel(0);
	}

	protected virtual bool IsAcceptingInput()
	{
		bool retval = true;
		if (parentConfirming) retval = false;
		
		if ((parent as UITownWindow).PopupActive) retval = false;
		
		if (!gameObject.activeInHierarchy) retval = false;
		
		return retval;
	}

}