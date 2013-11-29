using System;

public abstract class Item
{
    public enum ItemGrade
    {
        E = 0,
        D,
        C,
        B,
        A,
        S
    }

	protected string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    protected string description;
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    protected int level;
    public int Level
    {
        get { return level; }
        protected set { level = value; }
    }


    protected ItemGrade grade;
    public ItemGrade Grade
    {
        get { return grade; }
        set { grade = value; }
    }

    protected int sellValue;
    public int SellValue
    {
        get { return sellValue; }
        protected set { sellValue = value; }
    }

    protected virtual void CalculateSellValue()
    {
        // TODO: Find a formula for this. Or retrieve the value from elsewhere.
    }
}