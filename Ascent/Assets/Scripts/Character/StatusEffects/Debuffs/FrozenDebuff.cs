using UnityEngine;
using System.Collections;

public class FrozenDebuff : StatusEffect
{
	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		base.ApplyStatusEffect(caster, target, duration);
	}
}
