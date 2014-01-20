using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour
{
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

    // The event delegate handler we will use to take in the character.
    public delegate void CharacterEventHandler(Character charater);
    public event CharacterEventHandler onDeath;
    public event CharacterEventHandler onSpawn;

    // The event delegate handler we will use for damage taken.
    public delegate void Damage(int amount);
    public event Damage onDamageTaken;
    public event Damage onDamageDealt;
	
	protected List<Action> 			abilities = new List<Action>();
	protected Action 				activeAbility;
	protected GameObject 			weaponPrefab;
    protected bool 					isDead = false;
    protected Color                 originalColour = Color.white;

    protected float 				stunDuration;
	protected float					stunTimeAccum;
    protected float                 invulnerableDuration;
    protected float                 invulnerableTimeAccum;

	protected Transform 			weaponSlot;
	protected Collidable 			chargeBall;
	protected Weapon 				equipedWeapon;
	protected AnimatorController 	characterAnimator;
	protected BaseStats 			baseStatistics;
	protected DerivedStats			derivedStats;
    protected Character             lastDamagedBy;
	protected BetterList<Buff>		buffList = new BetterList<Buff>();
	protected CharacterMotor        motor;

	public CharacterMotor Motor
	{
		get { return motor;  }
	}

    public Transform WeaponSlot
    {
        get { return weaponSlot; }
    }

    public Color OrigionalColor
    {
        get { return originalColour; }
        set { originalColour = value; }
    }

	public Collidable ChargeBall
	{
		get { return chargeBall; }
	}

    public Weapon Weapon
    {
        get { return equipedWeapon; }
    }

    public AnimatorController Animator
    {
        get { return characterAnimator; }
	}
	
	public BaseStats CharacterStats
	{
		get { return baseStatistics; }
	}
	
	public DerivedStats DerivedStats
	{
		get { return derivedStats; }
	}

    public Character LastDamagedBy
    {
        get { return lastDamagedBy; }
        set { lastDamagedBy = value; }
    }

	public BetterList<Buff> BuffList
	{
		get { return buffList; }
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

	public virtual void Initialise()
	{
		Shadow shadow = GetComponentInChildren<Shadow>();
		shadow.Initialise();

		motor = GetComponentInChildren<CharacterMotor>();
		motor.Initialise();

        SetColor(OrigionalColor);
	}

    public virtual void Update()
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
        
		// Process all the buffs
        foreach(Buff b in buffList)
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
        else
        {
            SubUpdate();
        }
    }

    public virtual void SetColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)
        {
            render.material.color = color;
        }
    }

	public virtual void ResetColor()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer render in renderers)
		{
			render.material.color = originalColour;
		}
	}

    public virtual void SubUpdate()
    {
        // To be derived
    }

    private void UpdateActiveAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.UpdateAbility();
        }
    }

    public virtual void UseAbility(int abilityID)
    {
		if (activeAbility == null)
		{
            Action ability = abilities[abilityID];
            // Make sure the cooldown is off otherwise we cannot use the ability
			
            if (ability != null && ability.IsOnCooldown == false && (derivedStats.CurrentSpecial - ability.SpecialCost) >= 0 )
            {
                // TODO: Check if we are not in a state that denies abilities to perform.
                ability.StartAbility();
                activeAbility = ability;

                derivedStats.CurrentSpecial -= ability.SpecialCost;

                motor.StopMotion();
                motor.canMove = false;
            }
		}
    }

	public Action GetAbility(string ability)
	{
		if (activeAbility == null)
		{
			Action action = abilities.Find(a => a.Name == ability); // this is a lambda 
			if (action == null)
			{
				Debug.LogError("Could not find and return ability: " + ability);
			}

			return(action);
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
    public virtual void ApplyDamage(int unmitigatedDamage, EDamageType type, Character owner)
    {
		int finalDamage = unmitigatedDamage;
        lastDamagedBy = owner;

        // Let the owner know of the amount of damage done.
        if (owner != null)
        {
            owner.OnDamageDealt(finalDamage);
        }

        // Obtain the health stat and subtract damage amount to the health.
        derivedStats.CurrentHealth -= finalDamage;

        // Tell this character how much damage it has done.
        OnDamageTaken(finalDamage);

        // If the character is dead
		if (derivedStats.CurrentHealth <= 0 && !isDead)
        {
            // On Death settings
            // Update states to kill character
            OnDeath();
        }
    }

    public virtual void ApplyKnockback(Vector3 direction, float magnitude)
    {
		// Taking damage may or may not interrupt the current ability
		direction = new Vector3(direction.x, 0.0f, direction.z);
		//transform.GetComponent<CharacterController>().Move(direction * magnitude);
		motor.SetKnockback(direction, magnitude);
		//transform.rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public virtual void ApplySpellEffect()
    {
		// Taking damage may or may not interrupt the current ability
    }

    public virtual void ApplyStunEffect(float duration)
    {
        stunDuration = duration;
        SetColor(Color.yellow);
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
    public virtual void Respawn(Vector3 position)
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
        derivedStats.CurrentHealth = derivedStats.MaxHealth;
        derivedStats.CurrentSpecial = derivedStats.MaxSpecial;

        if (isDead)
        {
            Respawn(transform.position);
        }
    }

    public virtual void AddBuff(Buff buff)
    {
        buffList.Add(buff);
    }

    public virtual void RemoveBuff(Buff buff)
    {
        buffList.Remove(buff);
    }

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (activeAbility !=null)
		{
			activeAbility.DebugDraw();
		}
	}
#endif
}
