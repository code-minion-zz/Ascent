using UnityEngine;
using System.Collections.Generic;


public class UITown_MainPanel : UITown_Panel
{
	static float TOWER = 0f;
	static float BACKPACK = 270f;
	static float QUIT = 90f;

	Dictionary<float, int> AngleIndex;

	public override void Initialise()
	{
		AngleIndex = new Dictionary<float, int>();
		buttons = new UIButton[3];
		
		buttons[0] = transform.Find("Button Tower").GetComponent<UIButton>();
		AngleIndex.Add(0f, 0);

		buttons[1] = transform.Find("Button Quit").GetComponent<UIButton>();
		AngleIndex.Add(90f, 1);
		
		buttons[2] = transform.Find("Button Backpack").GetComponent<UIButton>();
		AngleIndex.Add(270f, 2);

		currentSelection = buttons[0];
		UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}

    public override void OnEnable()
    {
		// TODO : populate item list based on Backpack state
		if (!initialised) return;

		//ShowMenu();
		
        base.OnEnable();
    }

    public void Update()
    {

	}
	
	void HighlightButton ()//InputDevice device)
	{
		float angle = (parent as UITownWindow).PointerAngle;

		foreach (KeyValuePair<float,int> p in AngleIndex)
		{
			Debug.Log("Testing Angle:" + angle + " against:" + p.Key);
			if (CloseTo(angle,p.Key))
			{
				Debug.Log("Success! Angle:" + angle + " against:" + p.Key);
				UICamera.Notify(currentSelection.gameObject, "OnHover", false);
				currentSelection = buttons[p.Value];
				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
				
				Debug.Log("Testing Angle:" + angle + " against:" + p.Key);
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

		parent.TransitionToPanel((int)UITownWindow.EBackpackPanels.BACKPACK);
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UITownWindow.EBackpackPanels.BACKPACK);
	}

//	protected void ShowMenu()
//	{
//		UITownWindow.EBackpackTab tab = ((UITownWindow)parent).CurrentTab;
//		switch (tab)
//		{
//			case UITownWindow.EBackpackTab.Accessory:
//			{
//				// TODO: enable accessories tab
//				break;
//			}
//			case UITownWindow.EBackpackTab.Consumable:
//			{
//				// TODO: enable consumables tab
//
//				break;
//			}
//		}
//	}
}
