using System.Collections.Generic;
using UnityEngine;

public class UITown_Panel : UIPlayerMenuPanel
{
	// This class identifies a panel as belonging to the Town scene
	protected bool parentConfirming = false;
	protected bool confirmBoxResult = false;

	public override void Initialise() 
	{
		base.Initialise();

		(parent as UITownWindow).ConfirmBoxClose += OnConfirmBoxClose;
	}
	
	protected virtual void OpenConfirmBox(string str)
	{
		parentConfirming = true;
		(parent as UITownWindow).RequestConfirmBox(str);
	}

	protected virtual void OnConfirmBoxClose(bool result)
	{
		parentConfirming = false;
		confirmBoxResult = result;
		Debug.Log("OnConfirmBoxClose:"+confirmBoxResult);
	}
}