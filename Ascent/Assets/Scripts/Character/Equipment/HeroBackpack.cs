using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroBackpack 
{
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
                if (allItems[i] != null)
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

    public List<Item> allItemsa = new List<Item>();

    protected Item[] allItems = new Item[kMaxItems];
    public Item[] AllItems
    {
        get { return allItems; }
        set { allItems = value; }
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

    public void AddItem(int slot, Item item)
    {
        // TODO: Make sure there is room for the item
        allItems[slot] = item;
    }

    public void ReplaceItem(int slot, Item item)
    {
        // TODO: Make sure there is something to replace.
        // TODO: Make sure that there aren't too many accessories or consumables
        allItems[slot] = item;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < kMaxItems; ++i)
        {
            if (allItems[i] != null)
            {
                if (allItems[i] == item)
                {
                    allItems[i] = null;
                }
            }
        }
    }

    public void UpdateSubItemLists()
    {

    }
}
