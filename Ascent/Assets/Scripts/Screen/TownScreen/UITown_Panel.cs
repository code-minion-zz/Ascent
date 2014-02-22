using System.Collections.Generic;
using UnityEngine;

public class UITown_Panel : UIPlayerMenuPanel
{
	// This class identifies a panel as belonging to the Town scene
	protected bool parentConfirming = false;
	protected bool confirmBoxResult = false;
	protected UITownWindow townParent;

	public override void Initialise() 
	{
		base.Initialise();

		townParent = parent as UITownWindow;

		(parent as UITownWindow).ConfirmBoxClose += OnConfirmBoxClose;
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
}