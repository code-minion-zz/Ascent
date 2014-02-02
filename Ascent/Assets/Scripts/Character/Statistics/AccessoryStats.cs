using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryStats  : ItemStats
{
	protected PrimaryStats stats = new PrimaryStats();
	protected List<ItemProperty> properties = new List<ItemProperty>();

	public float Power
	{
		get { return stats.power; }
		set { stats.power = value; }
	}

	public float Finesse
	{
		get { return stats.finesse; }
		set { stats.finesse = value; }
	}

	public float Vitality
	{
		get { return stats.vitality; }
		set { stats.vitality = value; }
	}

	public float Spirit
	{
		get { return stats.spirit; }
		set { stats.spirit = value; }
	}

	public PrimaryStats PrimaryStats
	{
		get { return stats; }
	}

	public List<ItemProperty> Properties
	{
		get { return properties; }
	}
}
