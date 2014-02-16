using UnityEngine;
using System.Collections;

public class BlessingOfSpirit : Blessing
{
	protected override void ApplyStatusEffect(Character caster, Character target, float duration)
	{
		((Hero)target).Lives += 1;

		base.ApplyStatusEffect(caster, target, duration);
	}
}
