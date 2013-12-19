using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroInventory 
{
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
		items.Add(newItem);
	}
}
