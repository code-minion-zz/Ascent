using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Additional functions for buying/selling
/// </summary>
public class UITown_Shop : UIPlayerMenuPanel 
{
	#region Variables
	protected List<Item> shopInventory;

	/// <summary>
	/// Stores shop item buttons
	/// </summary>
	[SerializeField]
	protected List<UIItemButton> shopButtons;

	protected enum EMode
	{
		INVALID = -1,
		BUY,
		SELL,
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
	#endregion

	#region Initialization/Setup
	public override void Initialise()
	{
		quantityItems = Random.Range(15,20);
		shopButtons = new List<UIItemButton>(20);
		shopInventory = new List<Item>(20);
		
		buyButtonGrid = transform.Find("Buy").Find("Scroll View").Find("UIGrid").gameObject;
		sellButtonGrid = transform.Find("Sell").Find("Scroll View").Find("UIGrid").gameObject;
		NGUITools.SetActive(sellButtonGrid, false);

		InitShopInventory();

		foreach (UIButton button in shopButtons)
		{
			if (button.gameObject.activeSelf)
			{
				currentHighlightedButton = shopButtons.IndexOf(button as UIItemButton);
				currentSelection = button;
				break;
			}
		}

		// cache the hero stat for easy access to gold and such
		playerHero = (parent as UITownWindow).Player.Hero;

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
			shopButtons.Add(uib);
		}

		UpdateInventory();
	}
	#endregion

	#region On-Call Processes
	/// <summary>
	/// Updates the shop's item buttons
	/// </summary>
	protected virtual void UpdateInventory()
	{
		int buttonCount = 0;
		foreach (Item item in shopInventory)
		{
			NGUITools.SetActive(shopButtons[buttonCount].gameObject, true);
			shopButtons[buttonCount].LinkedItem = item;
			shopButtons[buttonCount].Name.gradientBottom = Color.grey;
			shopButtons[buttonCount].Name.text = item.ItemStats.Name + " [ff9900]" + item.ItemStats.PurchaseValue;
			shopButtons[buttonCount].Type = (item is AccessoryItem) ? UIItemButton.EType.ACCESSORY : UIItemButton.EType.CONSUMABLE;
			++buttonCount;
		}
		
		for (; buttonCount < shopButtons.Count; ++buttonCount)
		{
			shopButtons[buttonCount].LinkedItem = null;
			NGUITools.SetActive(shopButtons[buttonCount].gameObject, false);
		}
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

	protected virtual bool HighlightButton()
	{
		if (currentSelection)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			return true;
		}
		return false;
	}

	public override void OnEnable()
	{
		base.OnEnable();

		updateHighlight = true;
		shopMode = EMode.BUY;
		ChangeTab();
	}

	protected virtual void ChangeTab()
	{
		switch (shopMode)
		{
		case EMode.BUY:
			NGUITools.SetActive(buyButtonGrid, true);
			NGUITools.SetActive(sellButtonGrid, false);
			break;
		case EMode.SELL:
			NGUITools.SetActive(buyButtonGrid, false);
			NGUITools.SetActive(sellButtonGrid, true);
			break;
		case EMode.REPAIR:
			NGUITools.SetActive(buyButtonGrid, false);
			NGUITools.SetActive(sellButtonGrid, false);
			break;
		case EMode.APPRAISE:
			NGUITools.SetActive(buyButtonGrid, false);
			NGUITools.SetActive(sellButtonGrid, false);
			break;
		default:
			Debug.LogError("Invalid Tab");
			return;
			break;
		}
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
	}
	#endregion
		
	#region Input Handling	
	public override void OnMenuUp(InputDevice device)
	{
		++currentHighlightedButton;
		if (currentHighlightedButton >= shopButtons.Count) currentHighlightedButton = 0;

		currentSelection = shopButtons[currentHighlightedButton];
	}
	
	public override void OnMenuDown(InputDevice device)
	{
		--currentHighlightedButton;
		if (currentHighlightedButton < 0) currentHighlightedButton = shopButtons.Count -1;

		currentSelection = shopButtons[currentHighlightedButton];
	}
	
	public override void OnMenuLeft(InputDevice device)
	{
		--shopMode;
		if (shopMode == EMode.INVALID) shopMode = EMode.MAX - 1;

		ChangeTab();
	}
	
	public override void OnMenuRight(InputDevice device)
	{
		++shopMode;
		if (shopMode == EMode.MAX) shopMode = EMode.INVALID + 1;
		
		ChangeTab();
	}
	
	public override void OnMenuOK(InputDevice device)
	{
		switch (shopMode)
		{
		case EMode.BUY:
			if (currentSelection)
			{
//				int value = (currentSelection as UIItemButton).LinkedItem.ItemStats.PurchaseValue;
//				if (playerHero.HeroStats.Gold >= value)
//				{
//					// can afford
//					// TODO: Spawn Confirmation dialog box
//				}
			}
			break;
		case EMode.SELL:
			break;
		case EMode.REPAIR:
			break;
		case EMode.APPRAISE:
			break;
		}
	}
	
	
	public override void OnMenuCancel(InputDevice device)
	{
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
