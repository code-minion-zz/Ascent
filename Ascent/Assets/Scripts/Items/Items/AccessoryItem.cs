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
		set 
		{
			ItemStats = value;
			accessoryStats = value;
		}
	}

	public int Grade
	{
		get { return stats.Grade; }
		set { stats.Grade = value; }
	}

	public ItemGrade GradeEnum
	{
		get { return (ItemGrade)stats.Grade; }
		set { stats.Grade = (int)value; }
	}

	public int Durability
	{
		get { return durability; }
		set 
		{
			if (value < 0)
			{
				value = 0;
			}
			durability = value; 
		}
	}

	public int DurabilityMax
	{
		get { return durabilityMax; }
		set { durabilityMax = value; }
	}

	public override string ToString()
	{
		return GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + stats.Name + "\n" +
				"Desc: " + stats.Description + "\n" +
				"Dura: " + durability + "\\" + durabilityMax + "\n" +
				"Value: buy-" + 0 + ", sell-" + 0 + "\n" +
				"Stats: POW-" + accessoryStats.Power + ", FIN-" + accessoryStats.Finesse + ", VIT-" +  accessoryStats.Vitality + ", SPR-" + accessoryStats.Spirit + "\n" + 
				"Prop count: " + itemProperties.size + "\n";
				
	}
}
