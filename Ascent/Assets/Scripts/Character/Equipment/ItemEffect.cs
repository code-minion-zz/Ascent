//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

public class ItemEffect
{
	enum EffectType
	{
		INVALID = -1,	

		STAT,		// Mainly for equipment, a stat increase that 
					// remains constant as long as the item is equipped

		BUFF,		// Mainly for consumables, applies a buff just once,
					// usually has a duration
		
		ABILITY,	// 

		MAX
	}

	CharacterStatistics stats;

	#region Properties
	public CharacterStatistics Stats
	{
		get{ return stats; }
	}

	#endregion

	public ItemEffect ()
	{
	}
}
