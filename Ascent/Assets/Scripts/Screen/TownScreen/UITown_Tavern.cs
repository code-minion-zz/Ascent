using UnityEngine;
using System.Collections;

public class UITown_Tavern : UITown_RadialPanel {

	enum ETavernMode
	{
		Invalid = -1,
		NoPlayer,
		NewOrLoad,
		New,
		Load,
		CharacterSheet,
		Max
	}

	[SerializeField]
	public GameObject[] TavernElements;

	ETavernMode mode = ETavernMode.Invalid;

	public override void Initialise()
	{
		base.Initialise();

		mode = ETavernMode.NoPlayer;

		initialised = true;

		ProcessModeSwitch();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!initialised) return;

		UpdateElements();
	}

	/// <summary>
	/// If the panel has any dynamic elements or effects, update them here
	/// </summary>
	void UpdateElements()
	{

	}

	/// <summary>
	/// Hides everything if there is no active player
	/// </summary>
	void ShowSharedElements(bool yayOrNay)
	{
		townParent.ShowSharedElements(yayOrNay);
	}

	void ProcessOK()
	{
		if (currentSelection == null) return;
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			// TODO: Add player and bind panel to it
			break;
		case ETavernMode.NewOrLoad:
			// TODO: 
			break;
		}
	}

	void ProcessCancel()
	{
		switch (mode)
		{
		case ETavernMode.NewOrLoad:
			townParent.RequestQuit();
			break;
		}
	}

	void ProcessModeSwitch()
	{
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			ShowSharedElements(false);
			NGUITools.SetActive(TavernElements[0], true);
			NGUITools.SetActive(TavernElements[1], false);
			break;
		case ETavernMode.NewOrLoad:
			ShowSharedElements(true);
			NGUITools.SetActive(TavernElements[0], false);
			NGUITools.SetActive(TavernElements[1], true);
			break;
		case ETavernMode.New:
			break;
		}
	}

	public override void OnEnable()
	{		
		base.OnEnable();
		
		if (initialised)
		{
			(parent as UITownWindow).SetTitle("Tavern");
			ProcessModeSwitch();
		}
	}

	public void ActivatePanel()
	{
		mode = ETavernMode.NewOrLoad;
		ProcessModeSwitch();
		parent.Initialise();
	}

	public void DeactivatePanel()
	{
		mode = ETavernMode.NoPlayer;
		ProcessModeSwitch();
	}

	#region Input Handling
	public override void OnMenuLeft(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			break;
		case ETavernMode.NewOrLoad:
			break;
		case ETavernMode.New:
			break;
		case ETavernMode.Load:
			break;
		case ETavernMode.CharacterSheet:
			break;
		}
	}
	
	public override void OnMenuRight(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			break;
		case ETavernMode.NewOrLoad:
			break;
		case ETavernMode.New:
			break;
		case ETavernMode.Load:
			break;
		case ETavernMode.CharacterSheet:
			break;
		}
	}
	
	public override void OnMenuUp(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			break;
		case ETavernMode.NewOrLoad:
			break;
		case ETavernMode.New:
			break;
		case ETavernMode.Load:
			break;
		case ETavernMode.CharacterSheet:
			break;
		}
	}
	
	public override void OnMenuDown(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.NoPlayer:
			break;
		case ETavernMode.NewOrLoad:
			break;
		case ETavernMode.New:
			break;
		case ETavernMode.Load:
			break;
		case ETavernMode.CharacterSheet:
			break;
		}
	}
	
	public override void OnMenuOK(InputDevice device)
	{
		ProcessOK();
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
		ProcessCancel();	
	}
	#endregion
}
