using UnityEngine;
using System.Collections;

public class PoisonedDebuff : TicksOverTimeEffect
{
	public PoisonedDebuff()
    {
		type = EEffectType.Debuff;
    }

	protected override void Tick()
	{
		target.Stats.CurrentHealth -= (int)Mathf.Max((totalAmount / (float)ticks), 1.0f);
	}
}
