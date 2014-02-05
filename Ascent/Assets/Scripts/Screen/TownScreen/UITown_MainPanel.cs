using UnityEngine;
using System.Collections.Generic;

public class UITown_MainPanel : UITown_Panel
{
	const float TOWER = 0f;
    const float BACKPACK = 270f;
    const float QUIT = 90f;


	bool justInitialized = false;

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

		justInitialized = true;
	}


    public override void OnEnable()
	{
		base.OnEnable();

    }

    public void Update()
    {
		if (justInitialized)
		{
			HighlightButton();
			SetInfoLabel();
			justInitialized = false;
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
		HighlightButton();
		SetInfoLabel();
	}

	public override void OnMenuOK(InputDevice device)
	{
		switch (currentHighlightedButton)
		{
		case 0 : // tower
			//transition
			break;
		case 1 : // quit
			Game.Singleton.LoadLevel("MainMenu",Game.EGameState.MainMenu);
			break;
		case 2 : // backpack
			parent.TransitionToPanel(1);
			break;
		}
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

	public void SetInfoLabel()
	{
		UILabel InfoLabel = (parent as UITownWindow).InfoLabel;
		if (InfoLabel != null)
		{
			//Debug.Log("InfoLabel not null");

			if (currentHighlightedButton == -1)
			{
				Debug.LogError("UITownWindow: Uninitialized Panel Error");
			}

			switch (currentHighlightedButton)
			{
			case 0: // tower
				InfoLabel.text = "Enter the Tower";
				break;
			case 1: // quit
				InfoLabel.text = "Quit to Main Menu";
				break;
			case 2: // backpack
				InfoLabel.text = "Manage your Equipment";
				break;
			}

		}
	}
}
