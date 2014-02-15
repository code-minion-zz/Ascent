using UnityEngine;
using System.Collections;

public class SpecialDebuff : SecondaryStatModifierEffect
{
	public SpecialDebuff()
    {
		statType = EStats.SpecialPerStrike;
		type = EEffectType.Debuff;
    }
}
