using UnityEngine;
using System.Collections;

[System.Xml.Serialization.XmlInclude(typeof(HealthPotionItem))]
[System.Xml.Serialization.XmlInclude(typeof(BombItem))]
[System.Xml.Serialization.XmlInclude(typeof(KeyItem))]
[System.Xml.Serialization.XmlInclude(typeof(SpecialPotionItem))]
public class ConsumableItem : Item
{
	public enum EConsumableType
	{
		INVALID = -1,

		HealthPotion,
		SpecialPotion,

		Key,
		Bomb,

		MAX
	}

	[System.Xml.Serialization.XmlIgnore]
	protected EConsumableType consumableType;

	public EConsumableType Type
	{
		get { return consumableType; }
		set { consumableType = value; }
	}

	protected int charges;
	public int Charges
	{
		get{ return charges; }
		set { charges = value; }
	}

    [System.Xml.Serialization.XmlIgnore()]
    protected float cooldown = 0.0f;

    [System.Xml.Serialization.XmlIgnore()]
    public float Cooldown
    {
        get { return cooldown; }
	}

    [System.Xml.Serialization.XmlIgnore()]
	protected float cooldownMax = 2.0f;

    [System.Xml.Serialization.XmlIgnore()]
	public float CooldownMax
	{
		get { return cooldownMax; }
		set { cooldownMax = value; }
	}

    [System.Xml.Serialization.XmlIgnore()]
	public bool perishable = true;

    [System.Xml.Serialization.XmlIgnore()]
	protected bool HasCooldownAndCharges
	{
		get { return charges > 0 && cooldown == 0.0f; }
	}

	public override int SellCost
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

	public override int BuyCost
	{
		// https://docs.google.com/spreadsheet/ccc?key=0ApF1sRIB-wxQdHpVaEE0OGdRd0FYTlQwWngtTFpkeHc&usp=drive_web#gid=0

		get
		{
			float buyCost = (float)KBaseItemValue + ((float)KMaxItemValue - (float)KBaseItemValue) * Mathf.Pow((((float)stats.Level + (float)gradeHeuristics[GradeEnum]) / (float)StatGrowth.KMaxLevel), 2.0f);

			return Mathf.RoundToInt(buyCost);
		}
	}

	public override int AppraisalCost
	{
		get
		{
			float appraisalCost = (float)SellCostUnAppraised() * (1.0f - (1.0f / (float)gradeHeuristics[GradeEnum])) * 0.5f;
			return Mathf.RoundToInt(appraisalCost);
		}
	}


	public void Process()
	{
		if (cooldown > 0.0f)
		{
			cooldown -= Time.deltaTime;

			if (cooldown < 0.0f)
			{
				cooldown = 0.0f;
			}
		}
	}

	public void UseItem(Hero user)
    {
        if (HasCooldownAndCharges && CanUse(user))
		{
			Consume(user);
			cooldown = cooldownMax;

			if (perishable)
			{
				charges -= 1;
			}
		}
    }

    protected virtual bool CanUse(Hero user) { return false; }
    protected virtual void Consume(Hero user) { }
	
	public override string ToString()
	{
		return "Grade: " + GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + stats.Name + "\n" +
			"Desc: " + stats.Description + "\n" + "Value: buy-" + BuyCost + ", sell-" + SellCost + "\n";
		
	}

    public override string ToStringUnidentified()
    {
        return "Grade: " + GradeEnum.ToString() + " Lv" + stats.Level + ", Name: " + "?" + "\n" +
			"Desc: " + "?" + "\n" + "Value: buy-" + BuyCost + ", sell-" + SellCost + "\n";
    }
}
