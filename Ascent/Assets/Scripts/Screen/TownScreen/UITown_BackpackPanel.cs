using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class UITown_BackpackPanel : UITown_Panel 
{
	enum EMode
	{
		BACKPACK,
		INVENTORY
	}

	// Panel-management Variables
	GameObject inventoryGrid;
	GameObject backpackTab;
	GameObject inventoryTab;
	/// <summary>
	/// Are we in the backpack or inventory view?
	/// </summary>
	EMode activeTab = EMode.BACKPACK;
	/// <summary>
	/// What type of item should be displayed in the inventory?
	/// </summary>
	UIItemButton.EType inventType = UIItemButton.EType.ACCESSORY;
	Backpack heroBackpack = null;
	HeroInventory heroInvent = null;

	// Inventory-Tab Variables
	List<UIItemButton> inventoryItemButtons;
	UIItemButton inventoryHighlightedItemButton;


	public override void Initialise()
	{
		base.Initialise();

		buttons = new UIButton[7];
		inventoryItemButtons = new List<UIItemButton>(15);
		
		Transform backpack = transform.FindChild ("BackpackTab");
		Transform inventory = transform.FindChild ("InventoryTab");
		backpackTab = backpack.gameObject;
		inventoryTab = inventory.gameObject;
		inventoryGrid = inventory.FindChild("Scroll View").FindChild("UIGrid").gameObject;

		// Adding Items to button list
        // TODO : Replace button-adding with button-instantiation and positioning code
		buttons[0] = backpack.FindChild("Accessory 1").GetComponent<UIItemButton>() as UIButton;
		(buttons[0] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(-225f, 0);
		buttons[1] = backpack.FindChild("Accessory 2").GetComponent<UIItemButton>() as UIButton;
		(buttons[1] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(90f, 1);
		buttons[2] = backpack.FindChild("Accessory 3").GetComponent<UIItemButton>() as UIButton;
		(buttons[2] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(45f, 2);
		buttons[3] = backpack.FindChild("Accessory 4").GetComponent<UIItemButton>() as UIButton;
		(buttons[3] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(0f, 3);
		buttons[4] = backpack.FindChild("Consumable 1").GetComponent<UIItemButton>() as UIButton;
		(buttons[4] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(-45f, 4);
		buttons[5] = backpack.FindChild("Consumable 2").GetComponent<UIItemButton>() as UIButton;
		(buttons[5] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(-90f, 5);
		buttons[6] = backpack.FindChild("Consumable 3").GetComponent<UIItemButton>() as UIButton;
		(buttons[6] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(-135f, 6);


		// Pool Inventory buttons
		heroInvent = parent.Player.Hero.HeroInventory;
		int i;
		for (i = 0; i < heroInvent.MAX_INVENTORY_SLOTS; ++i)
		{
			GameObject itemPrefab = NGUITools.AddChild(inventoryGrid, Resources.Load("Prefabs/UI/Town/ItemContainer") as GameObject);
			UIItemButton uib = itemPrefab.GetComponent<UIItemButton>();
			inventoryItemButtons.Add(uib);
		}

		currentSelection = buttons[0];
		buttonMax = 7;
		currentHighlightedButton = 0;

		updatePointer = true;

		UpdateBackpack();
		//UpdateInventory();
		initialised = true;
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
				switch ((Item.ItemGrade)arrayItems[i].ItemStats.Grade)
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
				(buttons[i] as UIItemButton).LinkedItem = arrayItems[i];

			}
			else
			{
				NGUITools.SetActive(buttons[i].transform.FindChild("Item").gameObject, false);
			}
		}
	}

	public void UpdateInventory()
	{
		int itemIndex;
		int buttonIndex = 0;
		List<Item> items = heroInvent.Items;

		for (itemIndex = 0; itemIndex < items.Count; itemIndex++) 
		{
			bool theDroidsYouAreLookingFor = false;
			Item item = items [itemIndex];
			Debug.Log (item);
			if (item is AccessoryItem) 
			{
				if (inventType == UIItemButton.EType.ACCESSORY) 
				{
					theDroidsYouAreLookingFor = true;
				}
			}
			else if (item is ConsumableItem) 
			{
				if (inventType == UIItemButton.EType.CONSUMABLE) 
				{
					theDroidsYouAreLookingFor = true;
				}
			}

			if (theDroidsYouAreLookingFor)
			{
				string newName;
				if (item.ItemStats.Name != null)
				{
					newName = item.ItemStats.Name;
				}
				else
				{
					newName = "NullString";
				}
				inventoryItemButtons[buttonIndex].Name.text = newName;
				inventoryItemButtons[buttonIndex].Name.MarkAsChanged();
				inventoryItemButtons [buttonIndex].Type = inventType;
				inventoryItemButtons [buttonIndex].LinkedItem = item;
				NGUITools.SetActive (inventoryItemButtons [buttonIndex].gameObject, true);
				++buttonIndex;
			}
		}

		for (; buttonIndex < inventoryItemButtons.Count; ++buttonIndex)
		{
			inventoryItemButtons [buttonIndex].Reset();
		}
	}



	void ReturnToTown()
	{
		(parent as UITownWindow).RequestTransitionToPanel(0);
	}

	void SwapToBackpack()
	{
		UpdateBackpack();

		NGUITools.SetActive(inventoryTab, false);
		NGUITools.SetActive(backpackTab, true);
		activeTab = EMode.BACKPACK;

		(parent as UITownWindow).ShowArrow(true);
	}

	void SwapToInventory()
	{
		NGUITools.SetActive(backpackTab, false);
		NGUITools.SetActive(inventoryTab, true);
		activeTab = EMode.INVENTORY;

		// set the inventory type to the type of the currently selected backpack item
		inventType = (currentSelection as UIItemButton).Type;

		UpdateInventory();

		// set currently highlighted button to the first element
		(parent as UITownWindow).ShowArrow(false);
		if (inventoryItemButtons.Count > 0)
		{
			inventoryHighlightedItemButton = inventoryItemButtons[0];
		}

		// tell UIGrid to rearrange next Update()
		inventoryGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	void SetInfoLabel()
	{
		UIItemButton uib;
		if (activeTab == EMode.INVENTORY)
		{
			uib = inventoryHighlightedItemButton;
		}
		else
		{
			uib = currentSelection as UIItemButton;
		}
		(parent as UITownWindow).SetInfo(uib.LinkedItem.ToString());
	}

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		updatePointer = true;
	}

	public override void OnMenuUp(InputDevice device)
	{
		if (activeTab != EMode.INVENTORY) return;


	}

	public override void OnMenuDown(InputDevice device)
	{
		if (activeTab != EMode.INVENTORY) return;


	}
	
	public override void OnMenuLeft(InputDevice device)
	{
	}

	public override void OnMenuRight(InputDevice device)
	{		
	}

	public override void OnMenuOK(InputDevice device)
	{
		if (activeTab == EMode.BACKPACK)
		{
			SwapToInventory();
		}
	}


	public override void OnMenuCancel(InputDevice device)
	{
		if (activeTab == EMode.INVENTORY)
		{
			SwapToBackpack();
		}
		else
		{
			ReturnToTown();
		}

	}
	
	public override void OnMenuHax(InputDevice device)
	{
	}
	#endregion 
}
