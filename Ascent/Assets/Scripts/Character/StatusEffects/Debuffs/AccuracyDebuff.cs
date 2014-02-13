using UnityEngine;
using System.Collections;

public class AccuracyDebuff : SecondaryStatModifierEffect
{
	public AccuracyDebuff()
    {
		statType = EStats.Accuracy;
		type = EEffectType.Debuff;
    }
}
