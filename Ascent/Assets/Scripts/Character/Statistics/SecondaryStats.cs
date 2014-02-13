using UnityEngine;
using System.Collections;

public class SecondaryStats 
{
	public float health;
	public float special;

	public float attack;

	public float physicalDefense;
	public float magicalDefense;

	public float criticalHitChance;
	public float criticalHitMultiplier;

    public float specialPerStrike;

	public float dodgeChance;

    public float GetStat(EStats stat)
    {
        switch (stat)
        {
            case EStats.Health: return health;
            case EStats.Special: return special;
            case EStats.Attack: return attack;
            case EStats.PhysicalDefence: return physicalDefense;
            case EStats.MagicalDefence: return magicalDefense;
            case EStats.DodgeChance: return dodgeChance;
            case EStats.CriticalHitChance: return criticalHitChance;
            case EStats.CriticalHitMutliplier: return criticalHitMultiplier;
            case EStats.SpecialPerStrike: return specialPerStrike;
            default: { Debug.LogError("Unhandled case"); } break;
        }
        return 0.0f;
    }
}
