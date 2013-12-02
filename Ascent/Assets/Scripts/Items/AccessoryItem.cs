using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
    protected List<ItemProperty> itemProperties;
    public List<ItemProperty> ItemProperties
    {
        get { return itemProperties; }
        protected set { itemProperties = value; }
    }

    protected int durability;
    public int Durability
    {
        get { return durability; }
        set { durability = value; }
    }

    protected int durabilityMax;
    public int DurabilityMax
    {
        get { return durabilityMax; }
        set { durabilityMax = value; }
    }

    public bool IsBroken
    {
        get { return Durability > 0; }
        private set { }
    }

	BaseStats stats;
	
	#region Properties
	public BaseStats Stats
	{
		get{ return stats; }
	}
	#endregion
}
