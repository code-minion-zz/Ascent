using UnityEngine;
using System.Collections;

public class PrimaryStats  
{
	public float power;
	public float finesse;
	public float vitality;
	public float spirit;

	public float GetStat(EStats stat)
	{
		switch (stat)
		{
			case EStats.Power: return power;
			case EStats.Finesse: return finesse;
			case EStats.Vitality: return vitality;
			case EStats.Spirit: return spirit;
			default: { Debug.LogError("Unhandled case"); } break;
		}

		return 0.0f;
	}

}
