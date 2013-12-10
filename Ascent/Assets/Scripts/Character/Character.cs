using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour
{
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
    }
	
	protected List<Action> 			abilities = new List<Action>();
	protected IAction 				activeAbility;
	protected GameObject 			weaponPrefab;
    protected bool 					isDead = false;
    protected Color 				originalColour;

    protected float 				stunDuration;
	
	protected Transform 			weaponSlot;
	protected Collidable 			chargeBall;
	protected Weapon 				equipedWeapon;
	protected AnimatorController 	characterAnimator;
	protected BaseStats 			baseStatistics;
	protected DerivedStats			derivedStats;
	protected List<Object> 			lastObjectsDamagedBy = new List<Object>();
	protected BetterList<Buff>		buffList = new BetterList<Buff>();

    public Transform WeaponSlot
    {
        get { return weaponSlot; }
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

    public List<Object> LastObjectsDamagedBy
    {
        get { return lastObjectsDamagedBy; }
    }

	public BetterList<Buff> BuffList
	{
		get { return buffList; }
	}

    /// <summary>
    /// Returns true if the character is dead. 
    /// </summary>
    public bool IsDead
    {
        get { return isDead; }
    }

    public virtual void Awake()
    {
        // To be derived
    }

    public virtual void Start()
    {
        // To be derived
    }

    public virtual void Update()
    {
        UpdateActiveAbility();
    }

    private void UpdateActiveAbility()
    {
        if (activeAbility != null)
        {
            activeAbility.UpdateAbility();
        }

        // Update abilities that require cooldown
        foreach (Action ability in abilities)
        {
            if (ability.IsOnCooldown == true)
            {
                Debug.Log("Ability on cooldown: " + ability.Name);
                ability.UpdateCooldown();
            }
        }
    }

    public virtual void UseAbility(int abilityID)
    {
		if (activeAbility == null)
		{
            Action ability = abilities[abilityID];

            // Make sure the cooldown is off otherwise we cannot use the ability
            if (ability != null && ability.IsOnCooldown == false)
            {
                ability.StartAbility();
                activeAbility = ability;
            }
            else
            {
                Debug.Log("Ability: " + ability.Name + " is on cooldown");
            }
		}
    }

    public virtual void UseAbility(string ability)
    {
        if (activeAbility == null)
        {
            Action action = abilities.Find(a => a.Name == ability); // this is a lambda 
            if (action == null)
            {
                Debug.LogError("Could not find and use ability: " + ability);
            }
            else if (action.IsOnCooldown == false)
            {
                action.StartAbility();
                activeAbility = action;
            }
            else
            {
                Debug.Log("Ability: " + action.Name + " is on cooldown");
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

	public virtual void InterruptAbility()
	{
		if (activeAbility != null)
		{
			activeAbility.EndAbility();
			activeAbility = null;
		}
	}

	public virtual void StopAbility()
	{
		if (activeAbility != null)
		{
			activeAbility.EndAbility();
			activeAbility = null;
		}
	}

    public virtual void ApplyDamage(int unmitigatedDamage, EDamageType type)
    {
        // Obtain the health stat and subtract damage amount to the health.
        derivedStats.CurrentHealth -= unmitigatedDamage;

        // When the player takes a hit, spawn some damage text.
        HudManager.Singleton.TextDriver.SpawnDamageText(this.gameObject, unmitigatedDamage);

		if(OnDamageTaken != null)
		{
			OnDamageTaken.Invoke();
		}

        // If the character is dead
		if (derivedStats.CurrentHealth <= 0 && !isDead)
        {
            // On Death settings
            // Update states to kill character
            OnDeath();
        }
    }

	public delegate void DamageTaken();
	public event DamageTaken OnDamageTaken;

    public virtual void ApplyKnockback(Vector3 direction, float magnitude)
    {
		// Taking damage may or may not interrupt the current ability
		transform.rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public virtual void ApplySpellEffect()
    {
		// Taking damage may or may not interrupt the current ability
    }

    public virtual void ApplyStunEffect(float duration)
    {
        Debug.Log("Stunned: " + duration);
        stunDuration = duration;
    }

    /// <summary>
    /// When the character needs to respawn into the game.
    /// </summary>
    /// <param name="position">The position to spawn the character.</param>
    public virtual void Respawn(Vector3 position)
    {
        isDead = false;
        // Play this animation.
        //Animator.PlayAnimation("Respawn");
        transform.position = position;
    }

    public virtual void OnDeath()
    {
        // We may internally tell this character that they are dead.
        // The reason we do this is when we pool objects we will re-use 
        // this character.
        isDead = true;
        
        // Obtain the last object that killed this character
        if (lastObjectsDamagedBy.Count > 0)
        {
            Object obj = LastObjectsDamagedBy[lastObjectsDamagedBy.Count - 1];
            System.Type type = obj.GetType();

            // Check if the type of object is a weapon
            // then we can get the owner character.
            // TODO: Maybe the character should only know the object it was killed by and other characters will handle being killed by specific objects.
            if (type == typeof(Weapon))
            {
                Weapon weapon = obj as Weapon;
                Debug.Log("Killed by: " + weapon.Owner);
            }
			else
			{
				Debug.Log ("Killed by: " + type);
			}
        }
        else
        {
            //Debug.Log("Character somehow died by somethign and we do not know what caused it.");
        }
    }
	
	protected void AddSkill(Action skill)
	{
		skill.Initialise(this);
		abilities.Add(skill);
	}
}
