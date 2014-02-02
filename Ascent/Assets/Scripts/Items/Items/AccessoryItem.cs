using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
	protected BetterList<ItemProperty> itemProperties = new BetterList<ItemProperty>();
	protected AccessoryStats accessoryStats;

    protected int durability;
    protected int durabilityMax;

	public bool IsBroken
    {
        get { return Durability <= 0; }
        private set { }
    }
	
	public BetterList<ItemProperty> ItemProperties
	{
		get { return itemProperties; }
		protected set { itemProperties = value; }
	}

	public AccessoryStats AcessoryStats
	{
		get { return accessoryStats; }
		set { accessoryStats = value; }
	}

	public int Grade
	{
		get { return (int)grade; }
		set { grade = (ItemGrade)value; }
	}

	public ItemGrade GradeEnum
	{
		get { return grade; }
		set { grade = value; }
	}

	public int Durability
	{
		get { return durability; }
		set { durability = value; }
	}

	public int DurabilityMax
	{
		get { return durabilityMax; }
		set { durabilityMax = value; }
	}

	public override string ToString()
	{
		return GradeEnum.ToString() + " Lv" + level + ", Name: " + name + "\n" +
				"Desc: " + description + "\n" +
				"Dura: " + durability + "\\" + durabilityMax + "\n" +
				"Value: buy-" + 0 + ", sell-" + 0 + "\n" +
				"Prop count: " + itemProperties.size + "\n";
				
	}
}
