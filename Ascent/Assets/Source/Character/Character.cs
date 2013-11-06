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

    protected GameObject weaponPrefab;

    protected AnimatorController characterAnimator;
    public AnimatorController Animator
    {
        get { return characterAnimator; }
    }

    protected List<IAbility> abilities = new List<IAbility>();

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
        // To be derived
    }

    //public abstract void Move(Vector3 direction, float speed)
    //{
    //}

    //public abstract void UseAbility(int abilityID);

    public virtual void UseAbility(int abilityID)
    {
        abilities[abilityID].StartAbility();
    }

    public virtual void ApplyDamage(int unmitigatedDamage, EDamageType type)
    {
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
        
    }

    public virtual void ApplySpellEffect()
    {

    }

}
