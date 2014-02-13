using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : BaseCharacter
{
	#region Enums

	public enum EScope
	{
		Hero,
		Enemy,
		All,
	}

	public enum EHeroClass
	{
		Warrior,
		Rogue,
		Mage
	}

    public enum EDamageType
    {
        Physical,
        Magical,
        Trap
    }

	#endregion


	// The event delegate handler we will use to take in the character.
	public delegate void CharacterEventHandler(Character charater);
	public event CharacterEventHandler onDeath;
	public event CharacterEventHandler onSpawn;

	// The event delegate handler we will use for damage taken.
	public delegate void Damage(int amount);
	public event Damage onDamageTaken;
	public event Damage onDamageDealt; // Not handled by the character.

    protected List<StatusEffect> statusEffects = new List<StatusEffect>();

	protected List<Action> abilities = new List<Action>();

	protected Action activeAbility;
	public bool CanInterruptActiveAbility
	{
		get 
		{  
			if(activeAbility != null)
			{
				return activeAbility.CanBeInterrupted;
			}
			return true;
		}
	}

	protected CharacterStats stats;
	
	protected float stunDuration;
	protected float stunTimeAccum;
	protected float invulnerableDuration;
	protected float invulnerableTimeAccum;

	protected bool canBeStunned = true;
    public bool CanBeStunned
    {
        get { return canBeStunned; }
        set { canBeStunned = value; }
    }

	protected bool canBeKnockedBack = true;
    public bool CanBeKnockedBack
    {
        get { return canBeKnockedBack; }
        set { canBeKnockedBack = value; }
    }

	protected bool canBeDebuffed = true;
    public bool CanBeDebuffed
    {
        get { return CanBeDebuffed; }
        set { CanBeDebuffed = value; }
    }

	protected bool canBeInterrupted = true;
    public bool CanBeInterrupted
    {
        get { return CanBeInterrupted; }
        set { CanBeInterrupted = value; }
    }


    protected bool hitTaken = false;
    public bool HitTaken
    {
        get { return hitTaken; }
        set 
        {
            if (value && hitTaken != value)
            {              
                StartCoroutine("SetHitTaken");
            }
            
        }
    }
    public IEnumerator SetHitTaken()
    {
        hitTaken = true;
        yield return new WaitForSeconds(0.1f);
        hitTaken = false;
    } 

	protected bool isDead = false;

	protected Character lastDamagedBy;

	public CharacterStats Stats
	{
		get { return stats; }
	}

	public Character LastDamagedBy
	{
		get { return lastDamagedBy; }
		set { lastDamagedBy = value; }
	}

	public List<StatusEffect> StatusEffects
	{
		get { return statusEffects; }
	}

	public List<Action> Abilities
	{
		get { return abilities; }
	}

	public bool IsStunned
	{
		get { return stunDuration > 0.0f; }
	}

	public bool IsInvulnerable
	{
		get { return invulnerableDuration > 0.0f; }
	}

	/// <summary>
	/// Returns true if the character is dead. 
	/// </summary>
	public bool IsDead
	{
		get { return isDead; }
	}

	public override void Initialise()
	{
		base.Initialise();
	}

	public override void Update()
	{
		UpdateActiveAbility();

		// Update abilities that require cooldown
		foreach (Action ability in abilities)
		{
			if (ability.IsOnCooldown == true)
			{
				ability.UpdateCooldown();
			}
		}

        // Remove any expired buffs
        for (int i = statusEffects.Count - 1; i > -1; i--)
        {
            if (statusEffects[i].ToBeRemoved)
            {
                statusEffects.RemoveAt(i);
            }
        }

		// Process all the buffs
		foreach (StatusEffect b in statusEffects)
		{
			b.Process();
		}


		if (invulnerableDuration > 0.0f)
		{
			invulnerableDuration -= Time.deltaTime;

			if (invulnerableDuration < 0.0f)
			{
				invulnerableDuration = 0.0f;
				SetColor(originalColour);
			}
		}

		if (stunDuration > 0.0f)
		{
			stunDuration -= Time.deltaTime;

			GetComponentInChildren<CharacterMotor>().StopMotion();

			if (stunDuration < 0.0f)
			{
				stunDuration = 0.0f;
				SetColor(originalColour);
			}
		}
	}

	private void UpdateActiveAbility()
	{
		if (activeAbility != null)
		{
			activeAbility.Update();
		}
	}

    public virtual void UseAbility(Action ability)
    {
        int i = abilities.FindIndex(x => x == ability);

        UseAbility(i);
    }

	public virtual void UseAbility(int abilityID)
	{
        // If there no active ability then we can use a new one
        bool canUse = (activeAbility == null);

        // Or if there is an active one we can use a new one if the old one can be interupted
        bool interupt = false;
        if (!canUse)
        {
            interupt = activeAbility.CanBeInterrupted;
            canUse = interupt;

        }

        if (canUse)
		{
			Action ability = abilities[abilityID];
			// Make sure the cooldown is off otherwise we cannot use the ability
			if (ability != null && ability.IsOnCooldown == false && (stats.CurrentSpecial - ability.SpecialCost) >= 0)
			{
                
                if (interupt)
                {
                    StopAbility();
                }

				// TODO: Check if we are not in a state that denies abilities to perform.
				ability.StartAbility();
				activeAbility = ability;

				stats.CurrentSpecial -= ability.SpecialCost;

				motor.StopMotion();
				motor.canMove = false;
			}
		}
	}

    public bool CanCastAbility(int abilityID)
    {
        // If there no active ability then we can use a new one
        bool canUse = (activeAbility == null);

        // Or if there is an active one we can use a new one if the old one can be interupted
        bool interupt = false;
        if (!canUse)
        {
            interupt = activeAbility.CanBeInterrupted;
            canUse = interupt;
        }

        return canUse;
    }

    public virtual void UseCastAbility(int abilityID)
    {
        Action ability = abilities[abilityID];
        // Make sure the cooldown is off otherwise we cannot use the ability

        if (ability != null && ability.IsOnCooldown == false && (stats.CurrentSpecial - ability.SpecialCost) >= 0)
        {
            if (activeAbility != null)
            {
                if (activeAbility.CanBeInterrupted)
                {
                    StopAbility();
                }
            }

            ability.StartCast();
            //activeAbility = ability;

            motor.StopMotion();
            motor.canMove = false;
        }
    }

	public Action GetAbility(string ability)
	{
		if (activeAbility == null)
		{
			Action action = abilities.Find(a => a.ToString() == ability); // this is a lambda 
			if (action == null)
			{
				Debug.LogError("Could not find and return ability: " + ability);
			}

			return (action);
		}
		return null;
	}

	public virtual void StopAbility()
	{
		if (activeAbility != null)
		{
			activeAbility.EndAbility();
			activeAbility = null;

			motor.canMove = true;
		}
	}

	/// <summary>
	/// Applys damage to this chracter.
	/// </summary>
	/// <param name="unmitigatedDamage">The amount of damage.</param>
	/// <param name="type">The type of damage.</param>
	/// <param name="owner">The character that has dealt the damage to this character.</param>
	public virtual void ApplyDamage(int unmitigatedDamage, EDamageType type, Character damageDealer)
	{
		int damageDealerLevel = damageDealer.Stats.Level;

		int finalDamage = Mathf.Max( Mathf.RoundToInt((float)damageDealerLevel * ((float)(damageDealerLevel * unmitigatedDamage)) / (float)(Stats.Level * stats.PhysicalDefense)), 1);

        HitTaken = true;

		lastDamagedBy = damageDealer;

		// Let the owner know of the amount of damage done.
		if (damageDealer != null)
        {
			damageDealer.OnDamageDealt(finalDamage);
        }

		// Obtain the health stat and subtract damage amount to the health.
		stats.CurrentHealth -= finalDamage;


		if (this is Hero)
		{
			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(this.gameObject, finalDamage, Color.red);
		}
		else
		{
			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(this.gameObject, finalDamage, Color.cyan);
		}

		// Tell this character how much damage it has done.
		OnDamageTaken(finalDamage);

		// If the character is dead
		if (stats.CurrentHealth <= 0 && !isDead)
		{
			// On Death settings
			// Update states to kill character
			OnDeath();
		}
	}

	public virtual void ApplyKnockback(Vector3 direction, float magnitude)
	{
		if (canBeKnockedBack)
		{
			// Taking damage may or may not interrupt the current ability
			direction = new Vector3(direction.x, 0.0f, direction.z);
			//transform.GetComponent<CharacterController>().Move(direction * magnitude);
			motor.SetKnockback(direction, magnitude);
			//transform.rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
		}
	}

	public virtual void ApplySpellEffect()
	{
		if (canBeDebuffed)
		{
			// Taking damage may or may not interrupt the current ability
		}
	}

	public virtual void ApplyStunEffect(float duration)
	{
		if (canBeStunned)
		{
			stunDuration = duration;
			SetColor(Color.yellow);

			FloorHUDManager.Singleton.TextDriver.SpawnDamageText(this.gameObject, "Stunned!", Color.yellow);
		}
	}

	public virtual void ApplyInvulnerabilityEffect(float duration)
	{
		invulnerableDuration = duration;
		SetColor(Color.blue);
	}

	/// <summary>
	/// When the character needs to respawn into the game.
	/// </summary>
	/// <param name="position">The position to spawn the character.</param>
	protected virtual void Respawn(Vector3 position)
	{
		isDead = false;

		// Play this animation.
		transform.position = position;
		OnSpawn();
	}

    protected virtual void OnDeath()
	{
		// We may internally tell this character that they are dead.
		// The reason we do this is when we pool objects we will re-use 
		// this character.
		isDead = true;

		// Notify subscribers of the death.
		if (onDeath != null)
		{
			onDeath(this);
		}
	}

    protected virtual void OnSpawn()
    {
        if (onSpawn != null)
        {
            onSpawn(this);
        }
    }

    /// <summary>
    /// The event called when this character deals damage.
    /// </summary>
    /// <param name="damage">The amount of damage dealt.</param>
    protected virtual void OnDamageDealt(int damage)
    {
        if (onDamageDealt != null)
        {
            onDamageDealt(damage);
        }
    }

    /// <summary>
    /// The event called when this character takes damage. 
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    protected virtual void OnDamageTaken(int damage)
    {
        if (onDamageTaken != null)
        {
            onDamageTaken(damage);
        }
    }
	
	protected void AddSkill(Action skill)
	{
		skill.Initialise(this);
		abilities.Add(skill);
	}

    public virtual void RefreshEverything()
    {	
		if (stats != null)
		{
			stats.CurrentHealth = stats.MaxHealth;
			stats.CurrentSpecial = stats.MaxSpecial;

			if (isDead)
			{
				Debug.Log("a");
				Respawn(transform.position);
			}
		}
    }

	public virtual void AddStatusEffect(StatusEffect effect)
	{
        statusEffects.Add(effect);
	}

    public virtual void RemoveStatusEffect(StatusEffect effect)
	{
        effect.ToBeRemoved = true;
	}

	public virtual int DamageFormulaA(float addFixedDamage, float addMultiplier)
	{
		float damage = 0.0f;
		if (this is Hero)
		{
			damage = (((Hero)this).HeroStats.Attack * addMultiplier) + addFixedDamage;
		}
		else
		{
			damage = (Stats.Attack* addMultiplier) + addFixedDamage;
		}

		return (int)damage;
	}

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (activeAbility != null)
		{
			activeAbility.DebugDraw();
		}
	}
#endif
}
