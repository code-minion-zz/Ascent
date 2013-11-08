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

	protected List<IAction> abilities = new List<IAction>();
	protected IAction activeAbility;

	protected GameObject weaponPrefab;

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

    public virtual void UseAbility(int abilityID)
    {
		if (activeAbility == null)
		{
			abilities[abilityID].StartAbility();
			activeAbility = abilities[abilityID];
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

		

        //Debug.Log(characterStatistics.CurrentHealth);

        // If the character is dead
		if (characterStatistics.CurrentHealth <= 0)
        {
            // On Death settings
            // Update states to kill character
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

}
