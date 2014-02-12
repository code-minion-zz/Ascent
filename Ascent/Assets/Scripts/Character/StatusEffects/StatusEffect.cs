// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class StatusEffect 
{
    protected bool timed = false;
    public bool Timed
    {
        get { return timed; }
        set { timed = value; }
    }

    protected float duration = 0.0f;
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    protected string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    protected StatusEffectIcon icon;
    public StatusEffectIcon Icon
    {
        get { return icon; }
        set { icon = value; }
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

    public void Process()
    {
        if (timed)
        {
            if (duration > 0.0f)
            {
                duration -= Time.deltaTime;

                ProcessEffect();

                if (duration <= 0.0f)
                {
                    EndEffect();
                    RemoveEffect();
                }
            }
        }
    }
    
    public virtual void ApplyStatusEffect(Character caster, Character target, float duration)
    {
        if (duration > 0.0f)
        {
            timed = true;
            this.duration = duration;
        }

        this.caster = caster;
        this.target = target;

        name = this.ToString();
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
}
