using System;

public abstract class Item
{
    public enum ItemGrade
    {
		INVALID_GRADE = -1,
        E,	// Cursed
        D,	// Shitty
        C,	// Not bad but not good either
        B,	// Kinda good
        A,	// The bee's knees
        S,	// Bloody legend
		MAX_GRADE
    }

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
    public ItemGrade GradeEnum
    {
        get { return (ItemGrade)stats.Grade; }
        set { stats.Grade = (int)value; }
    }

    protected virtual int CalculateSellValue()
    {
        // TODO: Find a formula for this. Or retrieve the value from elsewhere.

		// 50% of base value + modifiers?
		float modifier = 1f;

		return (int)(0 * 0.5f * modifier);
    }

    public virtual string ToStringUnidentified()
    {
        return "This needs to be overidden.";

    }
}