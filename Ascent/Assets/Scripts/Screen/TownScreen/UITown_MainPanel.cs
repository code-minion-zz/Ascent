using UnityEngine;
using System.Collections.Generic;


public class UITown_MainPanel : UITown_Panel
{
	static float TOWER = 0f;
	static float BACKPACK = 270f;
	static float QUIT = 90f;

	Dictionary<float, int> AngleIndex;

	bool justInitialized = false;

	public override void Initialise()
	{
		AngleIndex = new Dictionary<float, int>();
		buttons = new UIButton[3];
		
		buttons[0] = transform.Find("Button Tower").GetComponent<UIButton>();
		AngleIndex.Add(0f, 0);

		buttons[1] = transform.Find("Button Quit").GetComponent<UIButton>();
		AngleIndex.Add(90f, 1);
		
		buttons[2] = transform.Find("Button Backpack").GetComponent<UIButton>();
		AngleIndex.Add(-90f, 2);

		currentSelection = buttons[0];
		currentButton = 0;

		justInitialized = true;
	}


    public override void OnEnable()
	{
		base.OnEnable();

		// TODO : populate item list based on Backpack state
		//if (!initialised) return;
    }

    public void Update()
    {
		if (justInitialized)
		{
			HighlightButton();
			justInitialized = false;
		}


	}
	
	void HighlightButton ()//InputDevice device)
	{
		float angle = (parent as UITownWindow).PointerAngle - 90f;

		foreach (KeyValuePair<float,int> p in AngleIndex)
		{
			Debug.Log("Testing Angle:" + angle + " against:" + p.Key);
			if (CloseTo(angle,p.Key))
			{
				Debug.Log("WIN!! Angle:" + angle + " against:" + p.Key);
				UICamera.Notify(currentSelection.gameObject, "OnHover", false);
				currentSelection = buttons[p.Value];
				currentButton = p.Value;
				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			}
			else
			{
				Debug.Log("FAIL!! Angle:" + angle + " against:" + p.Key);
			}
		}
	}

	public override void OnMenuLeftStickMove(InputDevice device)
	{
		HighlightButton();//device);
	}

	public override void OnMenuOK(InputDevice device)
	{
		// TODO: Change character's equipment

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
}
