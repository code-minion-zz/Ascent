using UnityEngine;
using System.Collections;

public class HealthRegenBuff : TicksOverTimeEffect 
{
	protected override void Tick()
	{
		target.Stats.CurrentHealth += (int)Mathf.Max((totalAmount / (float)ticks), 1.0f);
	}
}
