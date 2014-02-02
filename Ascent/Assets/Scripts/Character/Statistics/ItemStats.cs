using UnityEngine;
using System.Collections;

public class ItemStats 
{
	protected int level;
	protected int grade;

	protected string name;
	protected string description;

	public int Level
	{
		get { return level; }
		set { level = value; }
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

	public int Grade
	{
		get { return grade; }
		set { grade = value; }
	}

	public int PurchaseValue
	{
		get { return 0; }
		set {  }
	}

	public int SellValue
	{
		get { return 0; }
		set {  }
	}
}
