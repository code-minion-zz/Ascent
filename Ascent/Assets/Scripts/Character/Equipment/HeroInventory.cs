using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroInventory 
{
    private const int MAX_INVENTORY_SLOTS = 15;
	
	[System.Xml.Serialization.XmlIgnoreAttribute]
	private List<Item> items;

	public List<Item> Items
	{
		get { return items; }
		set { items = value; }
	}
	
	public HeroInventory()
	{
		items = new List<Item>();
	}

	public void AddItem(Item newItem)
	{
        if (items.Count < MAX_INVENTORY_SLOTS)
        {
            items.Add(newItem);
        }
	}
}
