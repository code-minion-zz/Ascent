using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryItem : Item
{
   // protected List<ItemProperty> itemProperties;
    protected int durability;
    protected int durabilityMax;
	protected ItemGrade grade;
	public bool IsBroken
    {
        get { return Durability > 0; }
        private set { }
    }

	BaseStats stats;
	
	#region Properties
//	public BetterList<ItemProperty> ItemProperties
//	{
//		get { return itemProperties; }
//		protected set { itemProperties = value; }
//	}
	public int Grade
	{
		get { return (int)grade;}
		set { grade = (ItemGrade)value; }
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
	public BaseStats Stats
	{
		get{ return stats; }
	}
	#endregion

	public AccessoryItem()
	{
		stats = new BaseStats();
	}
}
