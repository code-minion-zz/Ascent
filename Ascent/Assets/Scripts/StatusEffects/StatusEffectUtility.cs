using UnityEngine;
using System.Collections;

[System.Flags]
public enum EStatus
{
	None			= 0x00000,
	Stun			= 0x00001,
	Knock			= 0x00002,
	Interrupt		= 0x00004,
	Frozen			= 0x00008,
	Shock			= 0x00010,
	Silence			= 0x00020,
	Sleep			= 0x00040,
	Poison			= 0x00080,
	Invulnerability = 0x00100,

	All = 0xFFFFFFF,
}

[System.Flags]
public enum EStatusColour
{
	White	= 0x00000,
	Yellow	= 0x00001,
	Red		= 0x00002,
	Blue	= 0x00004,
	Green	= 0x00008,
	Pink	= 0x00010,

	Black = 0xFFFFFFF,
} 

public static class StatusEffectUtility
{
	public static Color GetColour(EStatusColour colour)
	{
		switch (colour)
		{
			case EStatusColour.White:	return Color.white;
			case EStatusColour.Yellow:	return Color.yellow;
			case EStatusColour.Red:		return Color.red;
			case EStatusColour.Blue:	return Color.blue;
			case EStatusColour.Green:	return Color.green;
			case EStatusColour.Pink:	return Color.magenta;
			case EStatusColour.Black:	return Color.black;
			default: break;
		}
		return Color.white;
	}
}