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

    protected int power;
    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    protected int finesse;
    public int Finesse
    {
        get { return finesse; }
        set { finesse = value; }
    }

    protected int vitality;
    public int Vitality
    {
        get { return vitality; }
        set { vitality = value; }
    }

    protected int spirit;
    public int Spirit
    {
        get { return spirit; }
        protected set { spirit = value; }
    }
}
