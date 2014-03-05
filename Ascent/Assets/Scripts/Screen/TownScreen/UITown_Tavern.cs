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
		if (!initialised)
		{
			base.Initialise();
		}

		initialised = true;
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
		case ETavernMode.NoPlayer:
//			townParent.RequestQuit();
			break;
		case ETavernMode.NewOrLoad:
			DeactivatePanel();
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
	
	void NewHero()
	{
		parent.Player.CreateHero(Character.EHeroClass.Warrior);
		parent.Initialise();
	}

	void LoadHero()
	{
		// Get data from selected save file and set
		Hero loadedHero = AscentGameSaver.LoadHero(new HeroSaveData());
		parent.Initialise();
	}

	public void ActivatePanel()
	{
		mode = ETavernMode.NewOrLoad;
		ProcessModeSwitch();
//		parent.Initialise();
	}

	public void DeactivatePanel()
	{
		mode = ETavernMode.NoPlayer;
		ProcessModeSwitch();

		if (parent.Ready)
		{
			parent.DeactivateWindow();
		}
	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		HighlightButton();
	}

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
