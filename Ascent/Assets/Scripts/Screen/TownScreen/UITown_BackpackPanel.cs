using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITown_BackpackPanel : UITown_Panel 
{
	enum EMode
	{
		BACKPACK,
		INVENTORY
	}

	EMode activeTab = EMode.BACKPACK;

	int lastActiveButton = 0;
	List<UIButton> inventoryButtons;

	public override void Initialise()
	{

		base.Initialise();

		buttons = new UIButton[7];
		inventoryButtons = new List<UIButton>();
	
		Transform backpack = transform.FindChild ("BackpackTab");
		Transform inventory = transform.FindChild ("InventoryTab");
		buttons[0] = backpack.FindChild("Accessory 1").GetComponent<UIButton>();
		AngleIndex.Add(-225f, 0);
		buttons[1] = backpack.FindChild("Accessory 2").GetComponent<UIButton>();
		AngleIndex.Add(90f, 1);
		buttons[2] = backpack.FindChild("Accessory 3").GetComponent<UIButton>();
		AngleIndex.Add(45f, 2);
		buttons[3] = backpack.FindChild("Accessory 4").GetComponent<UIButton>();
		AngleIndex.Add(0f, 3);
		buttons[4] = backpack.FindChild("Consumable 1").GetComponent<UIButton>();
		AngleIndex.Add(-45f, 4);
		buttons[5] = backpack.FindChild("Consumable 2").GetComponent<UIButton>();
		AngleIndex.Add(-90f, 5);
		buttons[6] = backpack.FindChild("Consumable 3").GetComponent<UIButton>();
		AngleIndex.Add(-135f, 6);



		currentHighlightedButton = 0;
		currentSelection = buttons[0];
		lastActiveButton = currentHighlightedButton;
		buttonMax = 7;

		initialised = true;
		updatePointer = true;

		UpdateItems();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (initialised) (parent as UITownWindow).SetTitle("Backpack");
	}

	public override void OnDisable()
	{
//		if (initialised)
//		{
//			lastActiveButton = currentHighlightedButton;
//			currentSelection = buttons[lastActiveButton];
//		}

		base.OnDisable();
	}

	protected void Update()
	{
		if (updatePointer)
		{
			switch (activeTab)
			{
			case EMode.BACKPACK:
				HighlightButton();
				break;
			case EMode.INVENTORY:
				break;
			}
			updatePointer = false;
		}
	}

	public void UpdateItems()
	{
		//Debug.Log(parent);
		// Change Button Icons in accordance to backpack data
		Backpack bp = parent.Player.Hero.Backpack;
		
		Item[] arrayItems = bp.AllItems;
		for (int i = 0; i < 7; ++i)
		{
			//Debug.Log("UpdateItems:"+i+" "+arrayItems[i]);
			if (arrayItems[i] != null)
			{
				Color temp = new Color();
				switch ((Item.ItemGrade)bp.AllItems[i].ItemStats.Grade)
				{
				case Item.ItemGrade.E:
					temp = Color.red;
					break;
				case Item.ItemGrade.D:
					temp = Color.magenta;
					break;
				case Item.ItemGrade.C:
					temp = Color.blue;
					break;
				case Item.ItemGrade.B:
					temp = Color.yellow;
					break;
				case Item.ItemGrade.A:
					temp = Color.cyan;
					break;
				case Item.ItemGrade.S:
					temp = Color.green;
					break;
				}
				buttons[i].transform.FindChild("Item").GetComponent<UISprite>().color = temp;
				NGUITools.SetActive(buttons[i].transform.FindChild("Item").gameObject, true);
			}
			else
			{
				
				NGUITools.SetActive(buttons[i].transform.FindChild("Item").gameObject, false);
			}
		}
	}

	void ReturnToTown()
	{
		(parent as UITownWindow).RequestTransitionToPanel(0);
	}

	void SwapToInventory()
	{

	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		updatePointer = true;
		//HighlightButton();
	}

	public override void OnMenuUp(InputDevice device)
	{
		if (activeTab == EMode.INVENTORY)
		{

		}
//		UICamera.Notify(currentSelection.gameObject, "OnHover", false);
//
//		currentSelection = PrevButton();
//
//		UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}

	public override void OnMenuDown(InputDevice device)
	{
//		UICamera.Notify(currentSelection.gameObject, "OnHover", false);
//
//		currentSelection = NextButton();
//
//		UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}
	
	public override void OnMenuLeft(InputDevice device)
	{
	}

	public override void OnMenuRight(InputDevice device)
	{		
	}

	public override void OnMenuOK(InputDevice device)
	{
	}


	public override void OnMenuCancel(InputDevice device)
	{
		// TODO: Link back to the main town screen

		ReturnToTown();
	}
	
	public override void OnMenuHax(InputDevice device)
	{
		//((UITownScreen)parent.ParentScreen).StartGame();
//		Game.Singleton.LoadLevel("Sewer_Levels", Game.EGameState.TowerRandom);
	}
	#endregion 
}
