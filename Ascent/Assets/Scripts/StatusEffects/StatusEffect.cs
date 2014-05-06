// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffect 
{
    public enum EApplyMethod
    {
        Percentange = 0,
        Fixed
    }

	public enum EEffectType
	{
		Buff,
		Debuff
	}

	protected EEffectType type;
	public EEffectType Type
	{
		get { return type; }
		set { type = value; }
	}

    protected bool timed = false;
    public bool Timed
    {
        get { return timed; }
        set { timed = value; }
    }

    protected float timeElapsed = 0.0f;
    public float TimeElapsed
    {
        get { return timeElapsed; }
        set 
		{
			timeElapsed = value;

			if (timeElapsed < 0.0f)
			{
				timeElapsed = 0.0f;
			}
		}
    }

    protected float duration = 0.0f;
    public float FullDuration
    {
        get { return duration; }
        set { duration = value; }
    }

	protected bool overridePrevious;
	public bool OverridePrevious
	{
		get { return overridePrevious; }
		set { overridePrevious = value; }
	}

    // The source of the buff
    protected Character caster;
    public Character Caster
    {
        get { return caster; }
        set { caster = value; }
    }

    // The target of the buff
    protected Character target;
    public Character Target
    {
        get { return target; }
        set { target = value; }
    }

    protected bool toBeRemoved;
    public bool ToBeRemoved
    {
        get { return toBeRemoved; }
        set { toBeRemoved = value; }
    }

    public void Process()
    {
        if (timed)
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= duration)
                {
                    timeElapsed = duration;
                }

                ProcessEffect();

                if (timeElapsed >= duration)
                {
                    EndEffect();
                    RemoveEffect();
                }
            }
        }
    }

	
	protected void ProcessImmuneEffect(Character target)
	{
        this.target = target;
		FloorHUDManager.Singleton.TextDriver.SpawnDamageText(target.gameObject, "Immune", Color.white);
		RemoveEffect();
	}

    public virtual void ApplyStatusEffect(Character caster, Character target)
    {
		timed = duration > 0.0f;
		this.caster = caster;
		this.target = target;

		// First check if there is a status effect of the time type

		List<StatusEffect> statusEffects = target.StatusEffects;

		if (overridePrevious)
		{
			bool overrideSuccesful = false;

			// Check if the effect already exists
			for (int i = 0; i < statusEffects.Count; ++i)
			{
				// Override the existing effect if the new one is more powerful
				// TODO: write comparison function in base class and have derived classes override it.
				if (statusEffects[i].GetType() == this.GetType())
				{
					// If the duration of the new effect is longer, replace the old one.
					// If the duration of the new effect is shorter, extend the old one.
					bool isDurationLonger = (statusEffects[i].FullDuration - statusEffects[i].TimeElapsed) > this.FullDuration;
					if (isDurationLonger)
					{
						statusEffects[i].FullDuration = this.FullDuration;
						statusEffects[i].TimeElapsed = 0.0f;
					}
					else
					{
						// Extend the life of the existing buff
						statusEffects[i].TimeElapsed -= this.FullDuration;
					}

					if (statusEffects[i].toBeRemoved)
					{
						statusEffects[i].toBeRemoved = false;
					}

					overrideSuccesful = true;
					return;
				}
			}

			if (!overrideSuccesful)
			{
				statusEffects.Add(this);
			}
		}
		else
		{
			statusEffects.Add(this);
		}
    }

    protected virtual void ProcessEffect()
    {
        // To be derived if it is needed.
    }

    protected virtual void EndEffect()
    {
        // To be derived if needed
    }

    protected virtual void RemoveEffect()
    {
        target.RemoveStatusEffect(this);
    }

	public void EndEarly()
	{
		EndEffect();
	}
}
