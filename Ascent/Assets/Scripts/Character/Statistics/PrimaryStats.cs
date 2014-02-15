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

    public float GetRootStat(EStats stat)
    {
        switch (stat)
        {
            case EStats.Health: return vitality;
            case EStats.Special: return spirit;
            case EStats.Attack: return power;
            case EStats.PhysicalDefence: return vitality;
            case EStats.MagicalDefence: return spirit;
            case EStats.DodgeChance: return finesse;
            case EStats.CriticalHitChance: return finesse;
            case EStats.CriticalHitMutliplier: return finesse;
            case EStats.SpecialPerStrike: return spirit;
            default: { Debug.LogError("Unhandled case"); } break;
        }
        return 0.0f;
    }

}
