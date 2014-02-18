using UnityEngine;
using System.Collections.Generic;

public class UITown_Shop : UIPlayerMenuPanel 
{
	#region Variables
	protected List<Item> shopInventory;

	/// <summary>
	/// Stores shop item buttons
	/// </summary>
	protected List<UIItemButton> shopButtons;

	/// <summary>
	/// Must be set before initializing shop inventory
	/// </summary>
	protected UIItemButton.EType shopType;
	
	GameObject buyButtonGrid;
	GameObject sellButtonGrid;

	int quantityItems;
	#endregion

	#region Initialization/Setup
	public override void Initialise()
	{
		quantityItems = Random.Range(15,20);
		shopButtons = new List<UIItemButton>(20);
		shopInventory = new List<Item>(20);
		
		buyButtonGrid = transform.Find("Buy").Find("Scroll View").Find("UIGrid").gameObject;
		sellButtonGrid = transform.Find("Sell").Find("Scroll View").Find("UIGrid").gameObject;
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

	}
	#endregion

	#region Per-Frame Processes
	protected virtual void Update()
	{

	}
	#endregion
		
	#region Input Handling	
	public override void OnMenuUp(InputDevice device)
	{
//		if (activeTab != EMode.INVENTORY) return;
//		
//		if (inventoryHighlightedButton == -1) return;
//		
//		if (inventoryHighlightedButton == 0) 
//		{
//			inventoryHighlightedButton = inventoryButtonCount;
//		}
//		--inventoryHighlightedButton;
//		
//		HighlightInventoryButton();
	}
	
	public override void OnMenuDown(InputDevice device)
	{
//		if (activeTab != EMode.INVENTORY) return;
//		
//		if (inventoryHighlightedButton == -1) return;
//		
//		if (inventoryHighlightedButton == inventoryButtonCount-1) 
//		{
//			inventoryHighlightedButton = -1;
//		}
//		++inventoryHighlightedButton;
//		
//		HighlightInventoryButton();
	}
	
	public override void OnMenuLeft(InputDevice device)
	{
	}
	
	public override void OnMenuRight(InputDevice device)
	{		
	}
	
	public override void OnMenuOK(InputDevice device)
	{
//		if (activeTab == EMode.BACKPACK)
//		{
//			if (currentHighlightedButton != -1)
//			{
//				SwapToInventory();
//			}
//		}		
//		else if (activeTab == EMode.INVENTORY)
//		{
//			if (inventoryHighlightedButton != -1)
//			{
//				// Replace Selected Backpack Item with Selected Inventory Item
//				parent.Player.Hero.Equip(currentHighlightedButton, inventoryHighlightedItemButton.LinkedItem);
//				
//				SwapToBackpack();
//			}
//		}
	}
	
	
	public override void OnMenuCancel(InputDevice device)
	{
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
