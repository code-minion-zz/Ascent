using UnityEngine;
using System.Collections;

public class CriticalBuff : SecondaryStatModifierEffect
{
    public CriticalBuff()
    {
        statType = EStats.CriticalHitChance;
    }
}
