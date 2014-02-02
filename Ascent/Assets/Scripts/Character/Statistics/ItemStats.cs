using UnityEngine;
using System.Collections;

public class ItemStats 
{
	public int level;
	public int grade;

	protected string name;
	protected string description;

	public int Level
	{
		get { return level; }
	}

	public string Name
	{
		get { return name; }
	}

	public string Description
	{
		get { return description; }
	}

	public int Grade
	{
		get { return grade; }
	}

	public int PurchaseValue
	{
		get { return 0; }
	}

	public int SellValue
	{
		get { return 0; }
	}
}
