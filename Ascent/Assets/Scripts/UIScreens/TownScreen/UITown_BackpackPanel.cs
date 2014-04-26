using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class UITown_BackpackPanel : UITown_RadialPanel 
{
	enum EMode
	{
		BACKPACK,
		INVENTORY
	}

	#region Properties and Variables
	// Panel-management Variables
	GameObject inventoryGrid;
	GameObject backpackTab;
	GameObject inventoryTab;
	GameObject emptyInventoryLabel;
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
	Item confirmSell;

	// Inventory-Tab Variables
	List<UIItemButton> inventoryItemButtons;
	UIItemButton inventoryHighlightedItemButton;
	int inventoryHighlightedButton = -1;
	int inventoryButtonCount = -1;
	bool updateTooltip = false;

	/// <summary>
	/// Returns item on currently selected item button, Null if not possible
	/// </summary>
	/// <value>The current item.</value>
	protected override Item CurrentItem
	{
		get
		{
			if (activeTab == EMode.BACKPACK) return base.CurrentItem;

			Item retval = null;
			if (inventoryHighlightedItemButton)
			{
				if (inventoryHighlightedItemButton is UIItemButton)
				{
					retval = (inventoryHighlightedItemButton as UIItemButton).LinkedItem; 
				}
			}
			return retval;
		}
	}
	#endregion

	#region Initialization
	public override void Initialise()
	{
		base.Initialise();

		buttons = new UIButton[7];
		inventoryItemButtons = new List<UIItemButton>(15);
		
		Transform backpack = transform.FindChild ("BackpackTab");
		Transform inventory = transform.FindChild ("InventoryTab");
		backpackTab = backpack.gameObject;
		inventoryTab = inventory.gameObject;
		emptyInventoryLabel = inventory.FindChild("Empty Message").gameObject;
		inventoryGrid = inventory.FindChild("Scroll View").FindChild("UIGrid").gameObject;

		// Adding Items to button list
        // TODO : Replace button-adding with button-instantiation and positioning code
		buttons[0] = backpack.FindChild("Accessory 1").GetComponent<UIItemButton>() as UIButton;
		(buttons[0] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(0, -225f);
		buttons[1] = backpack.FindChild("Accessory 2").GetComponent<UIItemButton>() as UIButton;
		(buttons[1] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(1, 90f);
		buttons[2] = backpack.FindChild("Accessory 3").GetComponent<UIItemButton>() as UIButton;
		(buttons[2] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(2, 45f);
		buttons[3] = backpack.FindChild("Accessory 4").GetComponent<UIItemButton>() as UIButton;
		(buttons[3] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;
		AngleIndex.Add(3, 0f);
		buttons[4] = backpack.FindChild("Consumable 1").GetComponent<UIItemButton>() as UIButton;
		(buttons[4] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(4, -45f);
		buttons[5] = backpack.FindChild("Consumable 2").GetComponent<UIItemButton>() as UIButton;
		(buttons[5] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(5, -90f);
		buttons[6] = backpack.FindChild("Consumable 3").GetComponent<UIItemButton>() as UIButton;
		(buttons[6] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
		AngleIndex.Add(6, -135f);


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
	#endregion

	#region Per-frame Processes
	protected void Update()
	{
		if (updatePointer)
		{
			if (activeTab == EMode.BACKPACK)
			{
				HighlightButton();
			}
			updatePointer = false;
		}

		if (confirmSell != null)
		{
			Sell (1);
		}

		if (updateTooltip)
		{
			SetInfoLabel();
			updateTooltip = false;
		}
	}
	#endregion

	#region As-Needed Functions
	public void UpdateBackpack()
	{
		// Change Button Icons in accordance to backpack data
		heroBackpack = parent.Player.Hero.Backpack;
		
		Item[] arrayItems = heroBackpack.AllItems;

		for (int i = 0; i < Backpack.kMaxItems; ++i)
		{
			if (arrayItems[i] != null)
			{
				UISprite sprite = buttons[i].transform.FindChild("Item").GetComponent<UISprite>();
				//sprite.color = temp;

				NGUITools.SetActive(buttons[i].transform.FindChild("Item").gameObject, true);
				(buttons[i] as UIItemButton).LinkedItem = arrayItems[i];
				if (i < 4)
				{
					(buttons[i] as UIItemButton).Type = UIItemButton.EType.ACCESSORY;

					// Cast to UIItemButton then to AccessoryItem
					AccessoryItem accessoryItem = ((AccessoryItem)((UIItemButton)buttons[i]).LinkedItem);
					
					string accessoryType = accessoryItem.Type.ToString().ToLower();
					sprite.spriteName = "accessory_" + accessoryType + "_" + accessoryItem.GradeEnum.ToString().ToLower();
				}
				else
				{
					(buttons[i] as UIItemButton).Type = UIItemButton.EType.CONSUMABLE;
				}

			}
			else
			{
				(buttons[i] as UIItemButton).LinkedItem = null;;
				NGUITools.SetActive(buttons[i].transform.FindChild("Item").gameObject, false);
			}
		}
		updateTooltip = true;
	}
	
	public override void OnEnable()
	{
		base.OnEnable();
		
		if (initialised) 
		{
			townParent.SetTitle("Backpack");
			townParent.SetInstructions("Press (A) to Select\nPress (B) to Cancel\nPress (Y) to Sell");
		}
		
		if (currentSelection) UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}
	
	public override void OnDisable()
	{
		//if (currentSelection != null) UICamera.Notify(currentSelection.gameObject, "OnHover", false);
		
		base.OnDisable();
	}

	public void UpdateInventory()
	{
		int itemIndex;
		int buttonIndex = 0;
		List<Item> items = heroInvent.Items;
		inventoryButtonCount = 0;

		for (itemIndex = 0; itemIndex < items.Count; itemIndex++) 
		{
			bool theDroidsYouAreLookingFor = false;
			Item item = items [itemIndex];
//			Debug.Log (item);
			if (item is AccessoryItem) 
			{
				if (inventType != UIItemButton.EType.ACCESSORY) 
				{
					theDroidsYouAreLookingFor = false;
				}
				else
				{
					// TODO : Replace this with grabbing icon data from Item
					//AccessoryItem accessoryItem = ((AccessoryItem)((UIItemButton)inventoryItemButtons[buttonIndex]).LinkedItem);
					AccessoryItem accessoryItem = (AccessoryItem)item;

					string accessoryType = accessoryItem.Type.ToString().ToLower();
					string accessorySpriteName = "accessory_" + accessoryType + "_" + accessoryItem.GradeEnum.ToString().ToLower();

					inventoryItemButtons[buttonIndex].Icon.spriteName = accessorySpriteName;
					theDroidsYouAreLookingFor = true;
				}
			}
			else if (item is ConsumableItem) 
			{
				if (inventType != UIItemButton.EType.CONSUMABLE) 
				{
					theDroidsYouAreLookingFor = false;
				}
				else
				{
					// TODO : Replace this with grabbing icon data from Item	
					//ConsumableItem consumableItem = ((ConsumableItem)((UIItemButton)inventoryItemButtons[buttonIndex]).LinkedItem);
					ConsumableItem consumableItem = (ConsumableItem)item;

					string consumableType = consumableItem.Type.ToString().ToLower();
					string consumableSpriteName = "consumable_" + consumableType;

					inventoryItemButtons[buttonIndex].Icon.spriteName = consumableSpriteName;
					theDroidsYouAreLookingFor = true;
				}
			}

			if (theDroidsYouAreLookingFor)
			{
				string newName;
				if (item.ItemStats.Name != null)
				{
                    if (item.IsAppraised)
                    {
                        newName = item.ItemStats.Name;
                    }
                    else
                    {
                        newName = "??????";
                    }
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
				++inventoryButtonCount;
				++buttonIndex;
			}
		}
		
		if (buttonIndex == 0)
		{
			NGUITools.SetActive(emptyInventoryLabel, true);
		}
		else 
		{
			NGUITools.SetActive(emptyInventoryLabel, false);
		}

		for (; buttonIndex < inventoryItemButtons.Count; ++buttonIndex)
		{
			inventoryItemButtons [buttonIndex].Reset();
		}
		updateTooltip = true;
	}

	void ReturnToTown()
	{
		(parent as UITownWindow).RequestTransitionToPanel(0);
	}

	void SwapToBackpack()
	{
		activeTab = EMode.BACKPACK;


		NGUITools.SetActive(inventoryTab, false);
		NGUITools.SetActive(backpackTab, true);

		inventoryHighlightedItemButton = null;
		inventoryHighlightedButton = -1;
		UpdateBackpack();
		townParent.ShowArrow(true);
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
		townParent.ShowArrow(false);
		townParent.SetInfo("Press (A) to select an item");
		if (inventoryButtonCount > 0)
		{
			inventoryHighlightedButton = 0;
			HighlightInventoryButton();
		}
		else
		{
			inventoryHighlightedButton = -1;
		}

		// tell UIGrid to rearrange next Update()
		inventoryGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	void SetInfoLabel()
	{
        string newString = "";
		if (activeTab == EMode.INVENTORY)
		{
			if (inventoryHighlightedButton != -1)
			{
				if (inventoryHighlightedItemButton.LinkedItem != null)
				{
                    if (inventoryHighlightedItemButton.LinkedItem.IsAppraised)
                    {
                        newString = inventoryHighlightedItemButton.LinkedItem.ToString();
                    }
                    else
                    {
                        newString = inventoryHighlightedItemButton.LinkedItem.ToStringUnidentified();
                    }
				}
			}
		}
		else
		{
			if (currentHighlightedButton >= 0 && currentHighlightedButton <= 3)
			{
				newString = "ACCESSORY\n";
			}
			else if (currentHighlightedButton > 3)
			{
				newString = "CONSUMABLE\n";
			}
			if (currentSelection != null)
			{
				if (CurrentItem != null)
				{
					newString += (CurrentItem.IsAppraised)?(currentSelection as UIItemButton).LinkedItem.ToString() : (currentSelection as UIItemButton).LinkedItem.ToStringUnidentified();
				}
			}
		}

		townParent.SetInfo(newString);

	}

	void HighlightInventoryButton()
	{
		if (inventoryHighlightedItemButton != null)
		{
			UICamera.Notify(inventoryHighlightedItemButton.gameObject, "OnHover", false);
		}
		inventoryHighlightedItemButton = inventoryItemButtons[inventoryHighlightedButton];
		UICamera.Notify(inventoryHighlightedItemButton.gameObject, "OnHover", true);
		SetInfoLabel();
	}

	protected override bool HighlightButton()
	{
		bool hit = base.HighlightButton();

		if (hit)
		{			
			SetInfoLabel();
		}
		return hit;
	}

	
	protected virtual void Sell(int step)
	{
		Hero playerHero = townParent.Player.Hero;
		switch (step)
		{
		case 0:
			if (currentSelection)
			{
				if (CurrentItem == null)
				{
					// play error sound
					return;
				}
				ItemStats itemStat = (currentSelection as UIItemButton).LinkedItem.ItemStats;
				int value = itemStat.PurchaseValue;
				if (playerHero.HeroStats.Gold >= value)
				{					
					confirmSell = CurrentItem;

					// can afford
					if (confirmSell != null)
					{
						OpenConfirmBox("Are you sure you want to sell " + itemStat.Name + " for " + confirmSell.ItemStats.SellValue + "?\n");
					}
					else 
					{
						Debug.Log("Could not get Item. currentSelection is not UIItemButton, or does not have LinkedItem");
					}					
				}
			}
			else if (inventoryHighlightedItemButton)
			{

			}
			break;
		case 1:
			if (parentConfirming) return;
			
			if (confirmBoxResult == true)
			{
				playerHero.HeroStats.Gold += confirmSell.ItemStats.SellValue;

				// TODO: remove item from respective list here
				if (activeTab == EMode.BACKPACK)
				{
					playerHero.Backpack.RemoveItem(confirmSell);
					UpdateBackpack();
				}
				else
				{
					playerHero.HeroInventory.Items.Remove(confirmSell);
					UpdateInventory();
				}
				
				Debug.Log("Sold " + confirmSell.ItemStats.Name + ". Current gold:" + playerHero.HeroStats.Gold);

				townParent.RequestNoticeBox("Sold " + confirmSell.ItemStats.Name + " for " + confirmSell.ItemStats.SellValue);
				confirmSell = null;
				// TODO: Update Backpack or Inventory here
				// TODO: play sounds, fx, etc
			}
			break;
		}
	}
	#endregion

	#region Input Handling
	public override void OnMenuLeftStickMove(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;
		
		//if (activeTab == EMode.BACKPACK) updatePointer = true;
		updatePointer = true;
	}

	public override void OnMenuUp(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (activeTab != EMode.INVENTORY) return;

		if (inventoryHighlightedButton == -1) return;

		if (inventoryHighlightedButton == 0) 
		{
			inventoryHighlightedButton = inventoryButtonCount;
		}
		--inventoryHighlightedButton;

		HighlightInventoryButton();
	}

	public override void OnMenuDown(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (activeTab != EMode.INVENTORY) return;
		
		if (inventoryHighlightedButton == -1) return;
		
		if (inventoryHighlightedButton == inventoryButtonCount-1) 
		{
			inventoryHighlightedButton = -1;
		}
		++inventoryHighlightedButton;
		
		HighlightInventoryButton();
	}
	
	public override void OnMenuLeft(InputDevice device)
	{
	}

	public override void OnMenuRight(InputDevice device)
	{		
	}

	public override void OnMenuOK(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (activeTab == EMode.BACKPACK)
		{
			if (currentHighlightedButton != -1)
			{
				SwapToInventory();
			}
		}		
		else if (activeTab == EMode.INVENTORY)
		{
			if (inventoryHighlightedButton != -1)
			{
				// Replace Selected Backpack Item with Selected Inventory Item
                if (inventoryHighlightedItemButton.LinkedItem.IsAppraised)
                {
                    parent.Player.Hero.Equip(currentHighlightedButton, inventoryHighlightedItemButton.LinkedItem);
                    //heroBackpack.ReplaceItem(currentHighlightedButton, inventoryHighlightedItemButton.LinkedItem);

                    SwapToBackpack();
                }
			}
		}
	}

	public override void OnMenuCancel(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (activeTab == EMode.INVENTORY)
		{
			SwapToBackpack();
		}
		else
		{
			ReturnToTown();
		}

	}
	
	public override void OnMenuSpecial(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		UIItemButton button = null;
		if (activeTab == EMode.INVENTORY)
		{
			if (inventoryHighlightedItemButton)
			{
				button = inventoryHighlightedItemButton;
			}
		}
		else 
		{
			if (currentSelection)
			{
				button = currentSelection as UIItemButton;
			}
		}
		if (button)
		{
			Sell (0);
//			string name = null;
//			confirmSell = button.LinkedItem;
//			if (!confirmSell.IsAppraised)
//			{
//				name = "?????";
//			}
//			else
//			{
//				name = confirmSell.ItemStats.Name;
//			}
//			Debug.Log (confirmSell.ItemStats.Name + confirmSell.IsAppraised);
//			townParent.RequestConfirmBox("Are you sure you want to sell " + name + " for " + button.LinkedItem.ItemStats.SellValue + " gold?");
		}
	}

	#endregion 
}
