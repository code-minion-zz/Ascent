using UnityEngine;
using System.Collections;


[System.Xml.Serialization.XmlInclude(typeof(WarriorStrike))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorHeavyStrike))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorCharge))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorWarCry))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorWarStomp))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorFireball))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorLightning))]
[System.Xml.Serialization.XmlInclude(typeof(WarriorFreezeField))]
public abstract class Ability
{
    public float animationLength = 0.0f;
    protected float animationSpeed = 1.0f;

    protected float cooldownFullDuration = 0.0f;
    private float coolingDownTimeElapsed = 0.0f;

    protected float timeElapsedSinceStarting = 0.0f;

    protected bool canBeInterrupted = false;

    protected bool isInstantCast = true;
    public bool IsInstanctCast
    {
        get { return isInstantCast; }
    }

    protected string animationTrigger;
	public string AnimationTrigger
	{
		get { return animationTrigger; }
	}

    protected int specialCost;

    public delegate void ActionEnd();
    public event ActionEnd OnActionEnd;

	public delegate void ActionCooled();
	public event ActionCooled OnActionCooled;

    protected Character owner;
    public Character Owner
    {
        get { return owner; }
    }

    /// <summary>
    /// The total duration of the ability.
    /// </summary>
    public float Length
    {
        get { return animationLength; }
        set { animationLength = value; }
    }

    /// <summary>
    /// The remaining countdown until the ability can be used again.
    /// </summary>
    public float RemainingCooldown
    {
        get { return coolingDownTimeElapsed; }
        set { coolingDownTimeElapsed = value; }
    }

	public float CooldownFullDuration
	{
		get { return cooldownFullDuration; }
		set { cooldownFullDuration = value; }
	}

    public bool IsOnCooldown
    {
        get { return coolingDownTimeElapsed > 0.0f; }
    }

    public int SpecialCost
    {
        get { return specialCost; }
    }

    public bool CanBeInterrupted
    {
        get { return canBeInterrupted; }
        set { canBeInterrupted = value; }
    }

    public virtual void Initialise(Character owner)
    {
       this.owner = owner;
       //this.name = this.GetType().ToString();
    }


    /// <summary>
    /// Handles resetting values for starting the ability. This includes cooldown times,
    /// by default all abilities have a 0 second cooldown. Changing the cooldowns in specific
    /// abilities should be done in the overloaded Intialise function.
    /// </summary>
    public virtual void StartAbility()
    {
        timeElapsedSinceStarting = 0.0f;
        coolingDownTimeElapsed = cooldownFullDuration;
    }

    public virtual void StartCast()
    {
        // To be derived
    }

	public virtual void UpdateCast()
	{
		// To be derived
	}

	/// <summary>
	/// Updates time, updates action then checks for time expiration
	/// 
	/// </summary>
	public virtual void Update()
	{
		timeElapsedSinceStarting += Time.deltaTime * animationSpeed;
        if (timeElapsedSinceStarting > animationLength)
		{
            timeElapsedSinceStarting = animationLength;
		}

		UpdateAbility();

        if (timeElapsedSinceStarting >= animationLength)
		{
			owner.Loadout.StopAbility();
		}
	}

    /// <summary>
    /// Must be overridden else update action won't do anything.
    /// 
    /// </summary>
	public virtual void UpdateAbility()
    {
		// Override
    }

    /// <summary>
    /// The timer that handles updating the cooldowns.
    /// </summary>
    public virtual void UpdateCooldown()
    {
        if (coolingDownTimeElapsed > 0.0f)
        {
            float timeVal = Time.deltaTime;
            coolingDownTimeElapsed -= timeVal;

            if (coolingDownTimeElapsed <= 0.0f)
            {
                coolingDownTimeElapsed = 0.0f;

                if (OnActionCooled != null)
                {
                    OnActionCooled.Invoke();
                }

                
            }
        }
    }

    public virtual void EndAbility()
    {
        timeElapsedSinceStarting = 0.0f;

        if (OnActionEnd != null)
        {
            OnActionEnd.Invoke();
        }
    }

    public void RefreshCooldown()
    {
        coolingDownTimeElapsed = 0.0f;
    }

	public virtual void DebugDraw()
	{

	}
}
