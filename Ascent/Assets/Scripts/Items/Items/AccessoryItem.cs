using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
    [System.Xml.Serialization.XmlIgnore()]
    protected List<ItemProperty> itemProperties = new List<ItemProperty>();
	protected AccessoryStats accessoryStats;

    protected int durability;
    protected int durabilityMax;

    [System.Xml.Serialization.XmlIgnore()]
	public bool IsBroken
    {
        get { return Durability <= 0; }
        private set { }
    }


	public List<ItemProperty> ItemProperties
	{
		get { return itemProperties; }
		protected set { itemProperties = value; }
	}

    [System.Xml.Serialization.XmlIgnore()]
	public AccessoryStats AcessoryStats
	{
		get { return accessoryStats; }
		set 
		{
			ItemStats = value;
			accessoryStats = value;
		}
	}

    [System.Xml.Serialization.XmlIgnore()]
	public int Grade
	{
		get { return stats.Grade; }
		set { stats.Grade = value; }
	}

    [System.Xml.Serialization.XmlIgnore()]
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
				"Prop count: " + itemProperties.Count + "\n";
				
	}
}
