// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Buff 
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

    public virtual void ApplyBuff(Character caster, Character target, float duration)
    {
        if(duration > 0.0f)
        {
            timed = true;
            this.duration = duration;
        }

        this.caster = caster;
        this.target = target;

        name = this.ToString();
    }

    public void Process()
    {
        if (timed)
        {
            if(duration > 0.0f)
            {
                duration -= Time.deltaTime;

                ProcessBuff();

                if (duration <= 0.0f)
                {
                    EndBuff();
                    RemoveBuff();
                }
            }
        }
    }

    protected virtual void ProcessBuff()
    {
        // To be derived if it is needed.
    }

    protected virtual void EndBuff()
    {
        // To be derived if needed
    }

    protected virtual void RemoveBuff()
    {
        target.RemoveBuff(this);
    }
}

public class BaseStatBuff : Buff
{
	public EStats type;
	protected PrimaryStats stats;

	public float Power
	{
		get { return stats.power; }
	}

	public float Finesse
	{
		get { return stats.finesse; }
	}

	public float Vitality
	{
		get { return stats.finesse; }
	}

	public float Spirit
	{
		get { return stats.spirit; }
	}

	public PrimaryStats PrimaryStats
	{
		get { return stats; }
	}

	public void AddBuff(float statValue)
	{
		statValue += stats.GetStat(type);
	}
}