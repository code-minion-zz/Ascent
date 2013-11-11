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

	protected List<Action> abilities = new List<Action>();
	protected IAction activeAbility;
	protected GameObject weaponPrefab;
    protected bool isDead = false;

    protected Transform weaponSlot;
    public Transform WeaponSlot
    {
        get { return weaponSlot; }
    }

    protected Weapon equipedWeapon;
    public Weapon Weapon
    {
        get { return equipedWeapon; }
    }

    protected AnimatorController characterAnimator;
    public AnimatorController Animator
    {
        get { return characterAnimator; }
    }

    protected CharacterStatistics characterStatistics;
    public CharacterStatistics CharacterStats
    {
        get { return characterStatistics; }
    }

    protected List<Object> lastObjectsDamagedBy = new List<Object>();
    public List<Object> LastObjectsDamagedBy
    {
        get { return lastObjectsDamagedBy; }
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
		if (activeAbility != null)
		{
			activeAbility.UpdateAbility();
		}
    }

    public virtual void UpdateActiveAbility()
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
			abilities[abilityID].StartAbility();
			activeAbility = abilities[abilityID];
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

            action.StartAbility();
            activeAbility = action;
        }
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
		// Taking damage may or may not interrupt the current ability

        //Debug.Log(unmitigatedDamage);
        // Obtain the health stat and subtract damage amount to the health.
        characterStatistics.CurrentHealth -= unmitigatedDamage;

        // If the character is dead
		if (characterStatistics.CurrentHealth <= 0)
        {
            // On Death settings
            // Update states to kill character
            OnDeath();
        }
    }

    public virtual void ApplyKnockback(Vector3 direction, float magnitude)
    {
		// Taking damage may or may not interrupt the current ability
		transform.rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public virtual void ApplySpellEffect()
    {
		// Taking damage may or may not interrupt the current ability
    }

    public virtual void OnDeath()
    {
        // We may internally tell this character that they are dead.
        // The reason we do this is when we pool objects we will re-use 
        // this character.
        isDead = true;
    }
}
