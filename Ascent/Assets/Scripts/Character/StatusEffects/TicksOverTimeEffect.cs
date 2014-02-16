using UnityEngine;
using System.Collections;

public class TicksOverTimeEffect : StatusEffect
{
	protected int ticks;
	protected float totalAmount;

	protected float timeAccum;
	protected float timePerTick;
	protected EApplyMethod applyMethod;

	public virtual void ApplyStatusEffect(Character caster, Character target, EApplyMethod applyMethod, int ticks, float totalAmount, float duration)
	{
		this.ticks = ticks;
		this.totalAmount = totalAmount;
		this.applyMethod = applyMethod;

		base.ApplyStatusEffect(caster, target, duration);

		timePerTick = duration / ticks;

		target.ApplyStatusEffect(this);
	}

	protected override void ProcessEffect()
	{
		timeAccum += Time.deltaTime;
		if (timeAccum >= timePerTick)
		{
			timeAccum -= timePerTick;
			Tick();
		}
	}

	protected virtual void Tick()
	{
		// Derive!
	}
}
