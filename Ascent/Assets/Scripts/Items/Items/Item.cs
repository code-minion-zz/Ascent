using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Item
{
    public enum EGrade
    {
		INVALID_GRADE = -1,
        E,	
        D,	
        C,	
        B,	
        A,	
        S,	
		MAX_GRADE
    }

	// int is a Heuristic item Value used for gold calculations
	public static Dictionary<EGrade, int> gradeHeuristics = new Dictionary<EGrade,int>()
	{
		{ EGrade.E, 2 },
		{ EGrade.D, 4 },
		{ EGrade.C, 5 },
		{ EGrade.B, 6 },
		{ EGrade.A, 7 },
		{ EGrade.S, 9 },
	};

	[System.Xml.Serialization.XmlIgnore]
	protected const int KBaseItemValue = 100;
	[System.Xml.Serialization.XmlIgnore]
	protected const int KMaxItemValue = 10000;

	protected ItemStats stats;
	public ItemStats ItemStats
	{
		get { return stats; }
		set { stats = value; }
	}

    protected bool appraised;

    [System.Xml.Serialization.XmlIgnore()]
    public bool IsAppraised
    {
        get { return appraised; }
        set { appraised = value; }
    }


    [System.Xml.Serialization.XmlIgnore()]
    public int Grade
    {
        get { return stats.Grade; }
        set { stats.Grade = value; }
    }

    [System.Xml.Serialization.XmlIgnore()]
    public EGrade GradeEnum
    {
        get { return (EGrade)stats.Grade; }
        set { stats.Grade = (int)value; }
    }

    public virtual int SellCost
    {
		// To be overriden
		get { return 0; }
    }

	public virtual int BuyCost
	{
		// KIT : Should take into account durability?
		// To be overriden
		get { return 0; }
	}

	public virtual int AppraisalCost
	{
		// To be overriden
		get { return 0; }
	}

	public virtual int RepairCost
	{
		// KIT : Account for isAppraised state?
		// To be overriden
		get { return 0; }
	}

	public virtual int SellCostUnAppraised()
	{
		return Mathf.RoundToInt((float)BuyCost * 0.075f);
	}



    public virtual string ToStringUnidentified()
    {
        return "This needs to be overidden.";

    }
}