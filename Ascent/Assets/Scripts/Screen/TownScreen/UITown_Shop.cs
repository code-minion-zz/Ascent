using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Additional functions for buying/selling
/// </summary>
public class UITown_Shop : UITown_RadialPanel 
{
	#region Variables & Properties
	protected List<Item> shopInventory;
	protected List<AccessoryItem> repairList;
	protected List<Item> unappraisedList;

	/// <summary>
	/// Stores shop item buttons
	/// </summary>
	[SerializeField]
	protected List<UIItemButton> itemButtons;

	protected enum EMode
	{
		INVALID = -1,
		BUY,
		REPAIR,
		APPRAISE,
		MAX
	}

	/// <summary>
	/// Must be set before initializing shop inventory
	/// </summary>
	protected UIItemButton.EType shopType;
	
	GameObject itemButtonGrid;

	int quantityItems;
	protected EMode shopMode = EMode.BUY;
	protected bool updateHighlight = false;
	protected Hero playerHero;
	protected Item confirmItem = null;
	bool firstRunHack = true;

	#endregion

	#region Initialization/Setup
	public override void Initialise()
	{
		base.Initialise();

		quantityItems = Random.Range(15,20);
		itemButtons = new List<UIItemButton>(20);
		shopInventory = new List<Item>(20);
		
		itemButtonGrid = transform.Find("ItemList").Find("Scroll View").Find("UIGrid").gameObject;
		//NGUITools.SetActive(sellButtonGrid, false);

		InitShopInventory();

		foreach (UIButton button in itemButtons)
		{
			if (button.gameObject.activeSelf)
			{
				currentHighlightedButton = itemButtons.IndexOf(button as UIItemButton);
				currentSelection = button;
				break;
			}
		}

		// cache the hero stat for easy access to gold and such
		playerHero = (parent as UITownWindow).Player.Hero;

		UnhighlightButton();
		updateHighlight = true;
		initialised = true;
	}

	/// <summary>
	/// Stocks the shop with random items, happens once when Town scene is loaded. shopType must be set first.
	/// </summary>
	protected virtual void InitShopInventory()
	{
		Restock();
		UpdateButtons(shopInventory);
	}
	#endregion
		
	#region Per-Frame Processes
	protected virtual void Update()
	{
		if (updateHighlight)
		{
			if (HighlightButton()) SetInfoLabel();
			updateHighlight = false;
		}

		if (confirmItem != null)
		{
			Buy (1);
		}
	}
	#endregion

	#region As-Needed Functions
	/// <summary>
	/// Changes item buttons to Buy list buttons
	/// </summary>
	protected virtual void UpdateButtons(List<Item> itemList)
	{
		int buttonCount = 0;
		foreach (Item item in itemList)
		{
			NGUITools.SetActive(itemButtons[buttonCount].gameObject, true);
			itemButtons[buttonCount].LinkedItem = item;
			itemButtons[buttonCount].Name.gradientBottom = Color.grey;
			itemButtons[buttonCount].Name.text = item.ItemStats.Name + " [ff9900]" + item.ItemStats.PurchaseValue;
			itemButtons[buttonCount].Type = (item is AccessoryItem) ? UIItemButton.EType.ACCESSORY : UIItemButton.EType.CONSUMABLE;
			++buttonCount;
		}
		
		for (; buttonCount < itemButtons.Count; ++buttonCount)
		{
			itemButtons[buttonCount].LinkedItem = null;
			NGUITools.SetActive(itemButtons[buttonCount].gameObject, false);
		}
		if (firstRunHack)
		{
			NGUITools.FindInParents<UIScrollView>(itemButtonGrid).Scroll(1f);
			firstRunHack = false;
		}
	}

	protected virtual void Restock()
	{
		LootGenerator.ELootType itemType = (shopType == UIItemButton.EType.ACCESSORY) ? LootGenerator.ELootType.Accessory : LootGenerator.ELootType.Consumable;
		
		int count;
		for (count = 0; count < quantityItems; ++count)
		{
			shopInventory.Add(LootGenerator.RandomlyGenerateItem(0, itemType, true));
		}
		
		for (count = 0; count < quantityItems; ++count)
		{		
			GameObject itemPrefab = NGUITools.AddChild(itemButtonGrid, Resources.Load("Prefabs/UI/Town/ItemContainer") as GameObject);
			UIItemButton uib = itemPrefab.GetComponent<UIItemButton>();
			itemButtons.Add(uib);
		}
	}

	protected virtual void RefreshRepairList()
	{
		repairList = playerHero.GetRepairable().ToList();
	}

	protected virtual void RefreshUnappraisedList()
	{
		unappraisedList = playerHero.GetUnappraised().ToList();
	}


	protected virtual void SetInfoLabel()
	{
		if (currentSelection)
		{
			if (currentSelection is UIItemButton)
			{
				UIItemButton itemButton = currentSelection as UIItemButton;
				(parent as UITownWindow).SetInfo(itemButton.LinkedItem.ToString());
			}
		}
	}

	protected override bool HighlightButton()
	{
		if (currentSelection)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			return true;
		}
		return false;
	}

	protected virtual bool UnhighlightButton()
	{
		if (!currentSelection) return false;

		UICamera.Notify(currentSelection.gameObject, "OnHover", false);
		return true;
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (initialised)
		{
			updateHighlight = true;
			shopMode = EMode.BUY;
			ChangeTab();
		}
//		if (shopMode == EMode.APPRAISE)
//		{
//			(parent as UITownWindow).SetTitle("Appraise");
//		}
//		else if (shopMode == EMode.REPAIR)
//		{
//			(parent as UITownWindow).SetTitle("Repair");
//		}
	}

	protected virtual void ChangeTab()
	{
		switch (shopMode)
		{
		case EMode.BUY:
			NGUITools.SetActive(itemButtonGrid, true);
			UpdateButtons(shopInventory);
			break;
		case EMode.REPAIR:
			NGUITools.SetActive(itemButtonGrid, false);
			break;
		case EMode.APPRAISE:
			NGUITools.SetActive(itemButtonGrid, false);
			break;
		default:
			Debug.LogError("Invalid Tab");
			return;
		}
		ChangeTitle();
	}

	protected virtual void ChangeTitle()
	{
		// override me
	}

	protected virtual void Buy(int step)
	{
		switch (step)
		{
		case 0:
			if (currentSelection)
			{
				ItemStats itemStat = (currentSelection as UIItemButton).LinkedItem.ItemStats;
				int value = itemStat.PurchaseValue;
				if (playerHero.HeroStats.Gold >= value)
				{
					if (playerHero.HeroInventory.Items.Count >= playerHero.HeroInventory.MAX_INVENTORY_SLOTS)
					{
						Debug.Log("Inventory Full");
						townParent.RequestNoticeBox("Inventory Full");
						break;
					}
					
					confirmItem = CurrentItem;
					// can afford
					if (confirmItem != null)
					{
						OpenConfirmBox("Are you sure you want to purchase " + itemStat.Name + "?\n");
					}
					else 
					{
						Debug.LogError("Could not get Item. currentSelection is not UIItemButton, or does not have LinkedItem");
					}
					
				}
			}
			break;
		case 1:
			if (parentConfirming) return;
			
			if (confirmBoxResult == true)
			{
				playerHero.HeroStats.Gold -= confirmItem.ItemStats.PurchaseValue;
				playerHero.HeroInventory.AddItem(confirmItem);
				Debug.Log("Purchased " + confirmItem.ItemStats.Name + ". Gold remaining:" + playerHero.HeroStats.Gold);
				shopInventory.Remove(confirmItem);
				townParent.RequestNoticeBox("Thanks for buying " + confirmItem.ItemStats.Name + "!");
				confirmItem = null;
				UpdateButtons(shopInventory);
				// TODO: play sounds, fx, etc
			}
			break;
		}
	}
	#endregion
		
	#region Input Handling	
	public override void OnMenuUp(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (!SatisfiesDeadzone(device,0)) return;

		if (shopMode == EMode.BUY)
		{
			if (shopInventory.Count < 2) return;
			UIScrollView sv = NGUITools.FindInParents<UIScrollView>(itemButtonGrid.transform);
			
			--currentHighlightedButton;
			if (currentHighlightedButton < 0) currentHighlightedButton = 0;//shopButtons.Count -1;
			
			UnhighlightButton();
			
			if (!itemButtons[currentHighlightedButton].gameObject.activeSelf)
			{
				List<UIItemButton> activeButtons = itemButtons.Where(button=> button.gameObject.activeSelf == true).ToList();
				
				UIItemButton newHighlight = activeButtons[activeButtons.Count -1];
				
				currentHighlightedButton = itemButtons.IndexOf(newHighlight);
				
				if (currentHighlightedButton == -1) 
				{
					currentSelection = null;
				}
			}

			currentSelection = itemButtons[currentHighlightedButton];

			sv.Scroll(1.08f);

			updateHighlight = true;
		}
	}
	
	public override void OnMenuDown(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (!SatisfiesDeadzone(device,2)) return;

		if (shopMode == EMode.BUY)
		{
			if (shopInventory.Count < 2) return;

			UIScrollView sv = NGUITools.FindInParents<UIScrollView>(itemButtonGrid.transform);			
			++currentHighlightedButton;

			if (currentHighlightedButton >= itemButtons.Count) currentHighlightedButton = itemButtons.Count - 1;//0;

			UnhighlightButton();

			if (!itemButtons[currentHighlightedButton].gameObject.activeSelf)
			{
				List<UIItemButton> activeButtons = itemButtons.Where(button=> button.gameObject.activeSelf == true).ToList();

				UIItemButton newHighlight = activeButtons[activeButtons.Count -1];

				currentHighlightedButton = itemButtons.IndexOf(newHighlight);

				if (currentHighlightedButton == -1) 
				{
					currentSelection = null;
				}
			}

			currentSelection = itemButtons[currentHighlightedButton];

			sv.Scroll(-1.08f);

			updateHighlight = true;
		}
	}
	
	public override void OnMenuLeft(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (!SatisfiesDeadzone(device,1)) return;

		--shopMode;
		if (shopMode == EMode.INVALID) shopMode = EMode.MAX - 1;

		ChangeTab();
		updateHighlight = true;
	}
	
	public override void OnMenuRight(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		if (!SatisfiesDeadzone(device,3)) return;

		++shopMode;
		if (shopMode == EMode.MAX) shopMode = EMode.INVALID + 1;
		
		ChangeTab();
		updateHighlight = true;
	}
	
	public override void OnMenuOK(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		switch (shopMode)
		{
		case EMode.BUY:
			Buy (0);
			break;
		case EMode.REPAIR:
			break;
		case EMode.APPRAISE:
			break;
		}
	}
	
	
	public override void OnMenuCancel(InputDevice device)
	{
		// reject input if these conditions are not met
		if (!IsAcceptingInput()) return;

		// quit to main menu
		(parent as UITownWindow).RequestTransitionToPanel(0);
//		if (activeTab == EMode.INVENTORY)
//		{
//			SwapToBackpack();
//		}
//		else
//		{
//			ReturnToTown();
//		}		
	}

	#endregion 
}
