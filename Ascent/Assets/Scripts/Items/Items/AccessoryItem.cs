using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
    public enum EAccessoryType
    {
        None,

        Ring,
        Necklace,
        Earring,
        Bracelet,
        Medal,

        Max,

    }

    [System.Xml.Serialization.XmlIgnore]
    protected int hitsTaken;

    [System.Xml.Serialization.XmlIgnore]
    public int HitsTaken
    {
        get { return hitsTaken; }
        set { hitsTaken = value; }
    }

    [System.Xml.Serialization.XmlIgnore]
    protected EAccessoryType accessoryType;

    public EAccessoryType Type
    {
        get { return accessoryType; }
        set { accessoryType = value; }
    }

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

	protected override int SellCost
	{
		get 
		{
			float sellCost = 0.0f;

			if (appraised)
			{
				sellCost = BuyCost * 0.075f;
			}
			else
			{
				sellCost = SellCostUnAppraised();
			}

			return Mathf.RoundToInt(sellCost); 
		}
	}

	protected override int BuyCost
	{
		// https://docs.google.com/spreadsheet/ccc?key=0ApF1sRIB-wxQdHpVaEE0OGdRd0FYTlQwWngtTFpkeHc&usp=drive_web#gid=0

		get 
		{
			float buyCost = (float)KBaseItemValue + ((float)KMaxItemValue - (float)KBaseItemValue) * Mathf.Pow((((float)stats.Level + (float)gradeHeuristics[GradeEnum]) / (float)StatGrowth.KMaxLevel), 2.0f);

			return Mathf.RoundToInt(buyCost); 
		}
	}

	protected override int AppraisalCost
	{
		get
		{
			float appraisalCost = (float)SellCostUnAppraised() * (1.0f - (1.0f / (float)gradeHeuristics[GradeEnum])) * 0.5f;
			return Mathf.RoundToInt(appraisalCost);
		}
	}

	protected override int RepairCost
	{
		get
		{
            float repairCost = ((float)durabilityMax - (float)durability) * (AppraisalCost * 0.1f);
			return Mathf.RoundToInt(repairCost);
		}
	}

    public void ApplyDurabilityDamage(int unmitigatedDamage, bool crit, Character owner, Character source)
    {
        // https://docs.google.com/spreadsheet/ccc?key=0ApF1sRIB-wxQdHpVaEE0OGdRd0FYTlQwWngtTFpkeHc&usp=drive_web#gid=4

        hitsTaken++;

        float divisor = (crit) ? 100.0f : 50.0f;
        float expo = (crit) ? 1.50f : 1.25f;

        float durabilityLossChance = ((Mathf.Pow((float)source.Stats.Level + (float)hitsTaken, expo)) / owner.Stats.Level) / divisor;
        
        float critBonus = 30.0f;
        durabilityLossChance += (crit) ? critBonus : 0.0f;

        float maxRandChance = 100.0f;

        if (durabilityLossChance > maxRandChance)
        {
            durabilityLossChance = maxRandChance;
        }

        float rand = Random.Range(0.0f, maxRandChance);

        if (rand <= durabilityLossChance)
        {
            hitsTaken = 0;
            durability -= 1;
            Debug.Log(stats.Name + ": " + durability + " / " + durabilityMax);
        }
    }

	public override string ToString()
	{
		string retVal = "Grade: " + GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + stats.Name + "\n" +
			"Desc: " + stats.Description + "\n" +
			"Durability: " + durability + " \\ " + durabilityMax + "\n" +
			"Value: buy-" + BuyCost + ", sell-" + SellCost + "\n" +
			"Stats: POW-" + Power + ", FIN-" + Finesse + ", VIT-" + Vitality + ", SPR-" + Spirit + "\n";
			//"Prop count: " + itemProperties.Count + "\n";

		foreach (ItemProperty ip in itemProperties)
		{
			retVal += ip.ToString() + "\n";
		}

		return retVal;
	}

    public override string ToStringUnidentified()
    {
        string retVal = "Grade: " + GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + "?????" + "\n" +
            "Desc: " + "?????" + "\n" +
            "Dura: " + "??" + " \\ " + "??" + "\n" +
            "Value: buy-" + BuyCost + ", sell-" + SellCost + "\n" +
            "Stats: POW-" + "??" + ", FIN-" + "??" + ", VIT-" + "??" + ", SPR-" + "??" + "\n";
        //"Prop count: " + itemProperties.Count + "\n";

        foreach (ItemProperty ip in itemProperties)
        {
            retVal += "?????" + "\n";
        }

        return retVal;
    }
}
