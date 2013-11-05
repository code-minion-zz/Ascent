using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
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

    #region Fields

    protected CharacterStatistics characterStatistics;
    //protected AnimatorController animatorController;

    Vector3 knockback;

    #endregion

    #region Properties

    public CharacterStatistics CharacterStats
    {
        get { return characterStatistics; }
    }

	//public AnimatorController Animator
	//{
	//    get { return animatorController; }
	//}

    #endregion

    #region Initialization

    public virtual void Awake()
    {
        // Get all the components
        //animatorController = GetComponent<AnimatorController>();
    }

    public virtual void Start()
    {

    }

    #endregion

    #region Operations

    public virtual void Update()
    {
        //if (knockback.magnitude > 0.0f)
        //{
        //    Debug.Log(knockback);
        //    transform.position += knockback * Time.deltaTime;

        //    knockback -= knockback * 2.0f;
        //}


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
        knockback += (magnitude * direction);
    }

    public virtual void ApplySpellEffect()
    {

    }



    #endregion
}
