using UnityEngine;
using System.Collections;

public class AttackDebuff : SecondaryStatModifierEffect 
{
	public AttackDebuff()
    {
		statType = EStats.Attack;
		type = EEffectType.Debuff;
    }
}
