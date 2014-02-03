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

    [System.Xml.Serialization.XmlIgnoreAttribute]
    public Item[] AllItems
    {
        get
        {
            Item[] allItems = new Item[kMaxItems];
            for (BackpackSlot slot = BackpackSlot.ACC1; slot < BackpackSlot.MAX; ++slot)
            {
                allItems[(int)slot] = GetItem(slot);
            }
            return allItems;
        }
    }

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

    public int ItemCount
    {
        get
        {
            return AccessoryCount + ConsumableCount;
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


    public void AddItem(BackpackSlot slot, Item item)
    {
        // TODO: Make sure there is room for the item

        if (slot < BackpackSlot.ACC4 + 1)
        {
            accessoryItems[(int)slot] = (AccessoryItem)item;
        }
        else
        {
            consumableItems[(int)slot - (int)BackpackSlot.ACC4 - 1] = (ConsumableItem)item;
        }
    }

    public Item ReplaceItem(int slot, Item item)
    {
        // TODO: Make sure there is something to replace.
        // TODO: Make sure that there aren't too many accessories or consumables
        Item retval = AllItems[slot];

        AddItem((BackpackSlot)slot, item);
        //AllItems[slot] = item;
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

    public Item GetItem(BackpackSlot slot)
    {
        Item getItem = null;
        if (slot < BackpackSlot.ACC4 + 1)
        {
            getItem = accessoryItems[(int)slot];
        }
        else
        {
            getItem = consumableItems[(int)slot - (int)BackpackSlot.ACC4 - 1];
        }
        return getItem;
    }

    /// <summary>
    /// Update cooldowns on items
    /// </summary>
    public void Process()
    {
        foreach (Item item in AllItems)
        {
            if (item is ConsumableItem)
            {
                ((ConsumableItem)item).Process();
            }
        }
    }
}
