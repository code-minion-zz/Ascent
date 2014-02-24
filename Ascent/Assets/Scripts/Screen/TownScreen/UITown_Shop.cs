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

	/// <summary>
	/// Stores shop item buttons
	/// </summary>
	[SerializeField]
	protected List<UIItemButton> buyButtons;

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
	
	GameObject buyButtonGrid;
	GameObject sellButtonGrid;

	int quantityItems;
	protected EMode shopMode = EMode.BUY;
	protected bool updateHighlight = false;
	protected Hero playerHero;
	protected Item confirmItem = null;

	/// <summary>
	/// Returns item on currently selected item button, Null if not possible
	/// </summary>
	/// <value>The current item.</value>
	Item CurrentItem
	{
		get
		{
			Item retval = null;
			if (currentSelection)
			{
				if (currentSelection is UIItemButton)
				{
					retval = (currentSelection as UIItemButton).LinkedItem; 
				}
			}
			return retval;
		}
	}
	#endregion

	#region Initialization/Setup
	public override void Initialise()
	{
		base.Initialise();

		quantityItems = Random.Range(15,20);
		buyButtons = new List<UIItemButton>(20);
		shopInventory = new List<Item>(20);
		
		buyButtonGrid = transform.Find("Buy").Find("Scroll View").Find("UIGrid").gameObject;
		//NGUITools.SetActive(sellButtonGrid, false);

		InitShopInventory();

		foreach (UIButton button in buyButtons)
		{
			if (button.gameObject.activeSelf)
			{
				currentHighlightedButton = buyButtons.IndexOf(button as UIItemButton);
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
		LootGenerator.ELootType itemType = (shopType == UIItemButton.EType.ACCESSORY) ? LootGenerator.ELootType.Accessory : LootGenerator.ELootType.Consumable;

		int count;
		for (count = 0; count < quantityItems; ++count)
		{
			shopInventory.Add(LootGenerator.RandomlyGenerateItem(0, itemType, true));
		}

		for (count = 0; count < quantityItems; ++count)
		{		
			GameObject itemPrefab = NGUITools.AddChild(buyButtonGrid, Resources.Load("Prefabs/UI/Town/ItemContainer") as GameObject);
			UIItemButton uib = itemPrefab.GetComponent<UIItemButton>();
			buyButtons.Add(uib);
		}

		UpdateItemButtons();
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
			if (parentConfirming) return;

			if (confirmBoxResult == true)
			{
				playerHero.HeroStats.Gold -= confirmItem.ItemStats.SellValue;
				playerHero.HeroInventory.AddItem(confirmItem);
				Debug.Log("Purchased " + confirmItem.ItemStats.Name + ". Gold remaining:" + playerHero.HeroStats.Gold);
				confirmItem = null;
				// TODO: play sounds, fx, etc
			}
		}
	}
	#endregion

	#region On-Call Processes
	/// <summary>
	/// Updates the shop's item buttons
	/// </summary>
	protected virtual void UpdateItemButtons()
	{
		int buttonCount = 0;
		foreach (Item item in shopInventory)
		{
			NGUITools.SetActive(buyButtons[buttonCount].gameObject, true);
			buyButtons[buttonCount].LinkedItem = item;
			buyButtons[buttonCount].Name.gradientBottom = Color.grey;
			buyButtons[buttonCount].Name.text = item.ItemStats.Name + " [ff9900]" + item.ItemStats.PurchaseValue;
			buyButtons[buttonCount].Type = (item is AccessoryItem) ? UIItemButton.EType.ACCESSORY : UIItemButton.EType.CONSUMABLE;
			++buttonCount;
		}
		
		for (; buttonCount < buyButtons.Count; ++buttonCount)
		{
			buyButtons[buttonCount].LinkedItem = null;
			NGUITools.SetActive(buyButtons[buttonCount].gameObject, false);
		}
		
		NGUITools.FindInParents<UIScrollView>(buyButtonGrid).Scroll(1f);
		//NGUITools.FindInParents<UIScrollView>(buyButtonGrid).enabled = true;
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
			NGUITools.SetActive(buyButtonGrid, true);
			UpdateItemButtons();
			break;
		case EMode.REPAIR:
			NGUITools.SetActive(buyButtonGrid, false);
			break;
		case EMode.APPRAISE:
			NGUITools.SetActive(buyButtonGrid, false);
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
			UIScrollView sv = NGUITools.FindInParents<UIScrollView>(buyButtonGrid.transform);
			
			--currentHighlightedButton;
			if (currentHighlightedButton < 0) currentHighlightedButton = 0;//shopButtons.Count -1;
			
			UnhighlightButton();
			currentSelection = buyButtons[currentHighlightedButton];

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

			UIScrollView sv = NGUITools.FindInParents<UIScrollView>(buyButtonGrid.transform);			
			++currentHighlightedButton;

			if (currentHighlightedButton >= buyButtons.Count) currentHighlightedButton = buyButtons.Count - 1;//0;

			UnhighlightButton();
			currentSelection = buyButtons[currentHighlightedButton];

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
			if (currentSelection)
			{
				ItemStats itemStat = (currentSelection as UIItemButton).LinkedItem.ItemStats;
				int value = itemStat.PurchaseValue;
				if (playerHero.HeroStats.Gold >= value)
				{
					if (playerHero.HeroInventory.Items.Count >= playerHero.HeroInventory.MAX_INVENTORY_SLOTS)
					{
						Debug.Log("Inventory Full");
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

	bool IsAcceptingInput()
	{
		bool retval = true;
		if (parentConfirming) retval = false;
		
		if ((parent as UITownWindow).Confirming) retval = false;

		return retval;
	}
	#endregion 
}
