using UnityEngine;
using System.Collections.Generic;

public class UITown_MainPanel : UITown_Panel
{
	const float TOWER = 0f;
    const float BACKPACK = 270f;
    const float QUIT = 90f;

	public override void Initialise()
	{
		base.Initialise();

		buttons = new UIButton[3];
		
		buttons[0] = transform.Find("Button Tower").GetComponent<UIButton>();
		AngleIndex.Add(0f, 0);

		buttons[1] = transform.Find("Button Quit").GetComponent<UIButton>();
		AngleIndex.Add(90f, 1);
		
		buttons[2] = transform.Find("Button Backpack").GetComponent<UIButton>();
		AngleIndex.Add(-90f, 2);

		currentSelection = buttons[0];
		currentHighlightedButton = 0;

		//justInitialized = true;
		initialised = true;
		updatePointer = true;
	}


    public override void OnEnable()
	{
		base.OnEnable();

		if (initialised) 
		{
			(parent as UITownWindow).SetTitle("Town");
		}
    }

    public void Update()
    {
		if (updatePointer)
		{
			if (HighlightButton())
			{
				SetInfoLabel();
			}
			updatePointer = false;
		}

		if (initialised)
		{
			SetInfoLabel();
		}
	}
	
//	void HighlightButton ()
//	{
//		float angle = (parent as UITownWindow).PointerAngle - 90f;
//
//		foreach (KeyValuePair<float,int> p in AngleIndex)
//		{
//			//Debug.Log("Testing Angle:" + angle + " against:" + p.Key);
//			if (CloseTo(angle,p.Key))
//			{
//				//Debug.Log("WIN!! Angle:" + angle + " against:" + p.Key);
//				UICamera.Notify(currentSelection.gameObject, "OnHover", false);
//				currentSelection = buttons[p.Value];
//				currentHighlightedButton = p.Value;
//				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
//				SetInfoLabel();
//			}
//			else
//			{
//				//Debug.Log("FAIL!! Angle:" + angle + " against:" + p.Key);
//			}
//		}
//	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		if (!gameObject.activeInHierarchy) return;
		//HighlightButton();
		updatePointer = true;
	}

	public override void OnMenuOK(InputDevice device)
	{
		if (currentSelection.gameObject.activeInHierarchy)	ButtonAction();
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
	}
	
	public override void OnMenuHax(InputDevice device)
	{
		//((UITownScreen)parent.ParentScreen).StartGame();
		Game.Singleton.LoadLevel("Sewer_Levels", Game.EGameState.TowerRandom);
	}
	
	public override void OnMenuUp(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuDown(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuLeft(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	public override void OnMenuRight(InputDevice device)
	{
		// TODO: Change character's equipment
		
	}
	#endregion 

	void ButtonAction()
	{
		switch (currentHighlightedButton)
		{
		case 0 : // tower
			(parent as UITownWindow).RequestTransitionToPanel(2);
			break;
		case 1 : // quit
			Game.Singleton.LoadLevel("MainMenu",Game.EGameState.MainMenu);
			break;
		case 2 : // backpack
			(parent as UITownWindow).RequestTransitionToPanel(1);
			break;
		default : // nothing meaningful is highlighted
			break;
		}
	}

	void SetInfoLabel()
	{
		UITownWindow townWindow = (parent as UITownWindow);

		switch (currentHighlightedButton)
		{
		case 0: // tower
			townWindow.SetInfo("Enter the Tower");
			break;
		case 1: // quit
			townWindow.SetInfo("Quit to Main Menu");
			break;
		case 2: // backpack
		townWindow.SetInfo("Manage your Equipment");
			break;
		default:
			townWindow.SetInfo("");
			break;
		}
	}

}
