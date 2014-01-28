using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Backpack 
{
	public enum BackpackSlot
	{
		INVALID = -1,
		ACC1,
		ACC2,
		ACC3,
		ACC4,
		ITM1,
		ITM2,
		ITM3,
		MAX
	}

    public const int kMaxItems = 7;
    public const int kMaxAccessories = 4;
    public const int kMaxConsumables = 3;

    public int ItemCount
    {
        get 
        {
            int count = 0;

            for (int i = 0; i < kMaxItems; ++i)
            {
				if (AllItems[(BackpackSlot)i] != null)
                {
                    ++count;
                }
            }

            return count;
        }
    }
    
    public int AccessoryCount
    {
        get
        {
            int count = 0;

            for (int i = 0; i < kMaxConsumables; ++i)
            {
                if (accessoryItems[i] != null)
                {
                    ++count;
                }
            }

            return count;
        }
    }

    public int ConsumableCount
    {
        get
        {
            int count = 0;

            for (int i = 0; i < kMaxConsumables; ++i)
            {
                if (consumableItems[i] != null)
                {
                    ++count;
                }
            }

            return count;
        }
    }

    public Dictionary<BackpackSlot, Item> AllItems = new Dictionary<BackpackSlot, Item>();

//    protected Item[] allItems = new Item[kMaxItems];
//    public Item[] AllItems
//    {
//        get { return allItems; }
//        set { allItems = value; }
//    }

    protected AccessoryItem[] accessoryItems = new AccessoryItem[kMaxAccessories];
    public AccessoryItem[] AccessoryItems
    {
        get { return accessoryItems; }
        protected set { accessoryItems = value; }
    }

    protected ConsumableItem[] consumableItems = new ConsumableItem[kMaxConsumables];
    public ConsumableItem[] ConsumableItems
    {
        get { return consumableItems; }
        protected set { consumableItems = value; }
    }

	public void AddItem(BackpackSlot slot, Item item)
    {
        // TODO: Make sure there is room for the item
		AllItems[slot] = item;
    }

    public Item ReplaceItem(int slot, Item item)
    {
        // TODO: Make sure there is something to replace.
        // TODO: Make sure that there aren't too many accessories or consumables
		Item retval = AllItems[(BackpackSlot)slot];
        AllItems[(BackpackSlot)slot] = item;
		return retval;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < kMaxItems; ++i)
        {
			BackpackSlot bs = (BackpackSlot)i;
            if (AllItems[bs] != null)
            {
                if (AllItems[bs] == item)
                {
                    AllItems[bs] = null;
                }
            }
        }
    }

	void GetAllOf(System.Type t)
	{
		//AllItems.SelectMany()
	}

    public void UpdateSubItemLists()
    {

    }
}
