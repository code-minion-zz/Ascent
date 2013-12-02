// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Buff 
{
}

public class BaseStatBuff : Buff
{
	BaseStats stats;

	BaseStatBuff()
	{
		stats = new BaseStats();
	}

	public BaseStats Stats
	{
		get { return stats; }
	}
}