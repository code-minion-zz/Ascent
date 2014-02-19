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
    protected AbilityLoadout loadout;
	protected CharacterStats stats;
	protected bool hitTaken;
	protected bool isDead;
	protected Character lastDamagedBy;
	protected EStatus vulnerabilities = EStatus.All;
	protected EStatus status;
	protected EStatusColour statusColour = EStatusColour.White;
	protected bool colourHasChanged = false;

    public AbilityLoadout Loadout
    {
        get { return loadout; }
    }

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

	public EStatus Vulnerabilities
	{
		get { return vulnerabilities; }
		set { vulnerabilities = value; }
	}

	public EStatus Status
	{
		get { return status; }
		set { status = value; }
	}

	public EStatusColour StatusColour
	{
		get { return statusColour; }
		set 
		{
			// If this is a new colour flag it for updating.
			if ((statusColour & value) != value)
			{
				colourHasChanged = true;
			}

			statusColour = value; 
		}
	}	 

	public bool CanMove
	{
		get { return !IsInState(EStatus.Stun); }
	}

	public bool CanAct
	{
		get { return !IsInState(EStatus.Stun); }
	}

	public bool CanAttack
	{
		get { return !IsInState(EStatus.Stun); }
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
        loadout.Process();

		if (colourHasChanged)
		{
			SetColor(StatusEffectUtility.GetColour(StatusColour));
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
	}

	/// <summary>
	/// Applys damage to this chracter.
	/// </summary>
	/// <param name="unmitigatedDamage">The amount of damage.</param>
	/// <param name="type">The type of damage.</param>
	/// <param name="owner">The character that has dealt the damage to this character.</param>
	public virtual void ApplyDamage(int unmitigatedDamage, bool crit, EDamageType type, Character damageDealer)
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

	public IEnumerator SetHitTaken()
	{
		hitTaken = true;
		yield return new WaitForSeconds(0.1f);
		hitTaken = false;
	} 


	public virtual void ApplyKnockback(Vector3 direction, float magnitude)
	{
		if (IsVulnerableTo(EStatus.Knock))
		{
			// Taking damage may or may not interrupt the current ability
			direction = new Vector3(direction.x, 0.0f, direction.z);
			//transform.GetComponent<CharacterController>().Move(direction * magnitude);
			motor.SetKnockback(direction, magnitude);
			//transform.rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
		}
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

    public virtual void RefreshEverything()
    {	
		if (stats != null)
		{
			stats.CurrentHealth = stats.MaxHealth;
			stats.CurrentSpecial = stats.MaxSpecial;

			if (isDead)
			{
				Respawn(transform.position);
			}
		}
    }

	public bool IsVulnerableTo(EStatus statusEffect)
	{
		return (vulnerabilities & statusEffect) == statusEffect;
	}

	public bool IsInState(EStatus statusEffect)
	{
		return (status & statusEffect) == statusEffect;
	}

	public virtual void ApplyStatusEffect(StatusEffect effect)
	{
		bool overridePrevious = effect.OverridePrevious;
		bool overrideSuccesful = false;
		
		if (overridePrevious)
		{
			// Check if the effect already exists
			for (int i = 0; i < statusEffects.Count; ++i)
			{
				// Override the existing effect if the new one is more powerful
				// TODO: write comparison function in base class and have derived classes override it.
				if (statusEffects[i].Type == effect.Type)
				{
					bool isDurationLonger = (statusEffects[i].FullDuration - statusEffects[i].TimeElapsed) > effect.FullDuration;
					if (isDurationLonger)
					{
						statusEffects[i] = effect;
					}
					else
					{
						// Extend the life of the existing buff
						statusEffects[i].TimeElapsed -= effect.FullDuration;
					}

					overrideSuccesful = true;
				}
			}
		}

		if (!overrideSuccesful)
		{
			statusEffects.Add(effect);
		}
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
        loadout.DebugDraw();
	}
#endif
}
