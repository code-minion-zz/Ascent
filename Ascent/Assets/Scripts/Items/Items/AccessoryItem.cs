using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
	[System.Xml.Serialization.XmlIgnore]
	protected PrimaryStats primaryStats = new PrimaryStats();

    [System.Xml.Serialization.XmlIgnore()]
    protected List<ItemProperty> itemProperties = new List<ItemProperty>();

    protected int durability;
    protected int durabilityMax;

    [System.Xml.Serialization.XmlIgnore()]
	public bool IsBroken
    {
        get { return Durability <= 0; }
        //private set { }
    }

		[System.Xml.Serialization.XmlIgnore]
	public float Power
	{
		get { return primaryStats.power; }
		set { primaryStats.power = value; }
	}

		[System.Xml.Serialization.XmlIgnore]
	public float Finesse
	{
		get { return primaryStats.finesse; }
		set { primaryStats.finesse = value; }
	}

		[System.Xml.Serialization.XmlIgnore]
	public float Vitality
	{
		get { return primaryStats.vitality; }
		set { primaryStats.vitality = value; }
	}

		[System.Xml.Serialization.XmlIgnore]
	public float Spirit
	{
		get { return primaryStats.spirit; }
		set { primaryStats.spirit = value; }
	}


	public PrimaryStats PrimaryStats
	{
		get { return primaryStats; }
		set {  primaryStats = value; }
	}

	public List<ItemProperty> ItemProperties
	{
		get { return itemProperties; }
		protected set { itemProperties = value; }
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

    public float ExperienceGainBonus
    {
        get 
        {
            float experienceBonus = 0.0f;
            foreach (ItemProperty property in itemProperties)
            {
                if (property is ExperienceItemProperty)
                {
					experienceBonus = ((ExperienceItemProperty)property).ExperienceGainBonus;
                }
            }
            return experienceBonus; 
        }
    }

    public float GoldGainBonus
    {
        get
        {
            float experienceBonus = 0.0f;
            foreach (ItemProperty property in itemProperties)
            {
                if (property is GoldItemProperty)
                {
					experienceBonus = ((GoldItemProperty)property).GoldGainBonus;
                }
            }
            return experienceBonus;
        }
    }

	public override string ToString()
	{
		string retVal = "Grade: " +GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + stats.Name + "\n" +
			"Desc: " + stats.Description + "\n" +
			"Dura: " + durability + "\\" + durabilityMax + "\n" +
			"Value: buy-" + 0 + ", sell-" + 0 + "\n" +
			"Stats: POW-" + Power + ", FIN-" + Finesse + ", VIT-" + Vitality + ", SPR-" + Spirit + "\n";
			//"Prop count: " + itemProperties.Count + "\n";

		foreach (ItemProperty ip in itemProperties)
		{
			retVal += ip.ToString() + "\n";
		}

		return retVal;
	}
}
