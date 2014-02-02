using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryStats  : ItemStats
{
	protected PrimaryStats stats;
	protected List<ItemProperty> properties;

	public float Power
	{
		get { return stats.power; }
	}

	public float Finesse
	{
		get { return stats.finesse; }
	}

	public float Vitality
	{
		get { return stats.finesse; }
	}

	public float Spirit
	{
		get { return stats.spirit; }
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
