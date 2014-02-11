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

	// Inventory-Tab Variables
	List<UIButton> inventoryButtons;
	UIButton inventoryHighlightedButton;

	Backpack heroBackpack = null;
	HeroInventory heroInvent = null;

	public override void Initialise()
	{

		base.Initialise();

		buttons = new UIButton[7];
		inventoryButtons = new List<UIButton>();
	
		Transform backpack = transform.FindChild ("BackpackTab");
		GameObject inventoryGrid = transform.FindChild ("InventoryTab").FindChild("Scroll View").FindChild("UIGrid").gameObject;

		// Adding Items to button list
		// TODO : Replace button-adding with button-instantiation and positioning code
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

		heroInvent = parent.Player.Hero.HeroInventory;

		int i;
		for (i = 0; i < heroInvent.Items.Count; ++i)
		{
			GameObject itemPrefab = NGUITools.AddChild(inventoryGrid, Resources.Load("Prefabs/UI/Town/ItemContainer") as GameObject);
			inventoryButtons.Add(itemPrefab.GetComponent<UIButton>());
		}
		inventoryGrid.GetComponent<UIGrid>().repositionNow = true;

		currentHighlightedButton = 0;
		currentSelection = buttons[0];
		lastActiveButton = currentHighlightedButton;
		buttonMax = 7;

		initialised = true;
		updatePointer = true;

		UpdateBackpack();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (initialised) (parent as UITownWindow).SetTitle("Backpack");
	}

	public override void OnDisable()
	{
		if (currentSelection != null) UICamera.Notify(currentSelection.gameObject, "OnHover", false);

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

	public void UpdateBackpack()
	{
		// Change Button Icons in accordance to backpack data
		heroBackpack = parent.Player.Hero.Backpack;
		
		Item[] arrayItems = heroBackpack.AllItems;
		for (int i = 0; i < heroBackpack.ItemCount; ++i)
		{
			if (arrayItems[i] != null)
			{
				Color temp = new Color();
				switch ((Item.ItemGrade)heroBackpack.AllItems[i].ItemStats.Grade)
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

	public void UpdateInventory()
	{
	}

	void ReturnToTown()
	{
		(parent as UITownWindow).RequestTransitionToPanel(0);
	}

	void SwapToInventory()
	{
		(parent as UITownWindow).ShowArrow(false);
		if (inventoryButtons.Count > 0)
		{
			inventoryHighlightedButton = inventoryButtons[0];
		}
	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		updatePointer = true;
		//HighlightButton();
	}

	public override void OnMenuUp(InputDevice device)
	{
		if (activeTab != EMode.INVENTORY) return;


	}

	public override void OnMenuDown(InputDevice device)
	{
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
	}
	#endregion 
}
