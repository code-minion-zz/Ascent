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

	protected string name;
	protected string description;
	protected int level;
	protected ItemGrade grade;
	protected ItemStats stats;
	protected int baseValue;


	public ItemStats ItemStats
	{
		get { return stats; }
	}


    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

//    public ItemGrade Grade
//    {
//        get { return grade; }
//        set { grade = value; }
//    }

    public int BaseValue
    {
        get { return baseValue; }
        set { baseValue = value; }
    }

    protected virtual int CalculateSellValue()
    {
        // TODO: Find a formula for this. Or retrieve the value from elsewhere.

		// 50% of base value + modifiers?
		float modifier = 1f;

		return (int)(baseValue * 0.5f * modifier);
    }
}