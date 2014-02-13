using UnityEngine;
using System.Collections;

public class SpeedDebuff : SecondaryStatModifierEffect
{
	public SpeedDebuff()
    {
		statType = EStats.MovementSpeed;
		type = EEffectType.Debuff;
    }
}
