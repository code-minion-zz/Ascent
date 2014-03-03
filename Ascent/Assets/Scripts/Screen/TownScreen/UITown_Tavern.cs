using UnityEngine;
using System.Collections;

public class UITown_Tavern : UITown_RadialPanel {

	enum ETavernMode
	{
		Invalid = -1,
		Empty,
		CreateOrLoad,
		Create,
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

		mode = ETavernMode.Empty;

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
		case ETavernMode.Empty:
			// TODO: Add player and bind panel to it
			break;
		case ETavernMode.CreateOrLoad:
			// TODO: 
			break;
		}
	}

	void ProcessCancel()
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			townParent.RequestQuit();
			break;
		}
	}

	void ProcessModeSwitch()
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			ShowSharedElements(false);
			NGUITools.SetActive(TavernElements[0], true);
			NGUITools.SetActive(TavernElements[1], false);
			break;
		case ETavernMode.CreateOrLoad:
			ShowSharedElements(true);
			NGUITools.SetActive(TavernElements[0], false);
			NGUITools.SetActive(TavernElements[1], true);
			break;
		case ETavernMode.Create:
			break;
		}
	}

	public override void OnEnable()
	{		
		base.OnEnable();
		
		if (initialised) (parent as UITownWindow).SetTitle("Tavern");
	}

	#region Input Handling
	public override void OnMenuLeft(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			break;
		case ETavernMode.CreateOrLoad:
			break;
		case ETavernMode.Create:
			break;
		}
	}
	
	public override void OnMenuRight(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			break;
		case ETavernMode.CreateOrLoad:
			break;
		case ETavernMode.Create:
			break;
		}
	}
	
	public override void OnMenuUp(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			break;
		case ETavernMode.CreateOrLoad:
			break;
		case ETavernMode.Create:
			break;
		}
	}
	
	public override void OnMenuDown(InputDevice device)
	{
		switch (mode)
		{
		case ETavernMode.Empty:
			break;
		case ETavernMode.CreateOrLoad:
			break;
		case ETavernMode.Create:
			break;
		}
	}
	
	public override void OnMenuOK(InputDevice device)
	{
		
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
		
	}
}
