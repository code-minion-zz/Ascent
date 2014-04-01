using UnityEngine;
using System.Collections;

public class PDefenceDebuff : SecondaryStatModifierEffect
{
	public PDefenceDebuff()
    {
		statType = EStats.PhysicalDefence;
		type = EEffectType.Debuff;
    }
}
