using UnityEngine;
using System.Collections;

public abstract class Action
{
    protected float animationLength = 0.0f;
    protected float animationSpeed = 1.0f;

    protected float coolDownTime = 0.0f;
    private float cooldownValue = 0.0f;

    protected float currentTime = 0.0f;

    private bool isOnCooldown = false;

    protected string animationTrigger;

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

    protected string name;
    public string Name
    {
        get { return name; }
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
    /// The cooldown period before this ability can be used again.
    /// </summary>
    public float CooldownTime
    {
        get { return coolDownTime; }
    }

    /// <summary>
    /// The remaining countdown until the ability can be used again.
    /// </summary>
    public float RemainingCooldown
    {
        get { return cooldownValue; }
        set { cooldownValue = value; }
    }

    public bool IsOnCooldown
    {
        get { return isOnCooldown; }
    }

    public int SpecialCost
    {
        get { return specialCost; }
    }

    public virtual void Initialise(Character owner)
    {
       this.owner = owner;
       this.name = this.GetType().ToString();
    }

    /// <summary>
    /// Handles resetting values for starting the ability. This includes cooldown times,
    /// by default all abilities have a 0 second cooldown. Changing the cooldowns in specific
    /// abilities should be done in the overloaded Intialise function.
    /// </summary>
    public virtual void StartAbility()
    {
        currentTime = 0.0f;
        cooldownValue = CooldownTime;
        isOnCooldown = true;
		
		if (owner.Animator != null)
		{
			owner.Animator.PlayAnimation(animationTrigger);
		}
    }

    /// <summary>
    /// Stops the ability after it has reached the total duration
    /// 
    /// </summary>
    public virtual void UpdateAbility()
    {
        float timeVal = Time.deltaTime * animationSpeed;
        currentTime += timeVal;

        if (currentTime >= Length)
        {
            owner.StopAbility();
        }
    }

    /// <summary>
    /// The timer that handles updating the cooldowns.
    /// </summary>
    public virtual void UpdateCooldown()
    {
		if (isOnCooldown)
		{
			float timeVal = Time.deltaTime;
			cooldownValue -= timeVal;

			if (cooldownValue <= 0.0f)
			{
				cooldownValue = 0.0f;
				isOnCooldown = false;

				if (OnActionCooled != null)
				{
					OnActionCooled.Invoke();
				}
			}
		}
    }

    public virtual void EndAbility()
    {
		if (owner.Animator != null)
		{
			owner.Animator.StopAnimation(animationTrigger);
		}

        currentTime = 0.0f;

        if (OnActionEnd != null)
        {
            OnActionEnd.Invoke();
        }
    }

    public void RefreshCooldown()
    {
        cooldownValue = 0.0f;
    }

	public virtual void DebugDraw()
	{

	}
}
