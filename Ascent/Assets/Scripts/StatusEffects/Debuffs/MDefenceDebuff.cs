using UnityEngine;
using System.Collections;

public class MDefenceDebuff : SecondaryStatModifierEffect
{
	public MDefenceDebuff()
    {
		statType = EStats.MagicalDefence;
		type = EEffectType.Debuff;
    }
}
