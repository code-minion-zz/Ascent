using UnityEngine;
using System.Collections;

public class DodgeDebuff : SecondaryStatModifierEffect
{
	public DodgeDebuff()
    {
		statType = EStats.DodgeChance;
		type = EEffectType.Debuff;
    }
}
