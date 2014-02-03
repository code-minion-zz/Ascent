using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

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
				if (AllItems[i] != null)
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
                if (AllItems[i] != null)
                {
                    if (AllItems[i] is AccessoryItem)
                    {
                        ++count;
                    }
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
                if (AllItems[i] != null)
                {
                    if (AllItems[i] is ConsumableItem)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }
    }
	
	public Item[] AllItems = new Item[kMaxItems];

    public AccessoryItem[] AccessoryItems
    {
        get 
        {
            AccessoryItem[] accessoryItems = new AccessoryItem[kMaxAccessories];
            for (BackpackSlot slot = BackpackSlot.ACC1; slot < BackpackSlot.ACC4; ++slot)
            {
                if (AllItems[(int)slot] != null)
                {
                    accessoryItems[(int)slot] = (AccessoryItem)AllItems[(int)slot];
                }
            }
            return accessoryItems; 
        }
    }

    public ConsumableItem[] ConsumableItems
    {
        get 
        {
            ConsumableItem[] consumableItems = new ConsumableItem[kMaxConsumables];
            for (BackpackSlot slot = BackpackSlot.ITM1; slot < BackpackSlot.ITM3 + 1; ++slot)
            {
                if (AllItems[(int)slot] != null)
                {
                    consumableItems[(int)(slot - BackpackSlot.ACC4 - 1)] = (ConsumableItem)AllItems[(int)slot];
                }
            }
            return consumableItems;
        }
    }

	public void AddItem(BackpackSlot slot, Item item)
    {
        // TODO: Make sure there is room for the item
		AllItems[(int)slot] = item;
    }

    public Item ReplaceItem(int slot, Item item)
    {
        // TODO: Make sure there is something to replace.
        // TODO: Make sure that there aren't too many accessories or consumables
		Item retval = AllItems[slot];
        AllItems[slot] = item;
		return retval;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < kMaxItems; ++i)
        {
            if (AllItems[i] != null)
            {
                if (AllItems[i] == item)
                {
                    AllItems[i] = null;
                }
            }
        }
    }

    /// <summary>
    /// Update cooldowns on items
    /// </summary>
    public void Process()
    {
        foreach(Item item in AllItems)
        {
            if (item is ConsumableItem)
            {
                ((ConsumableItem)item).Process();
            }
        }
    }
}
