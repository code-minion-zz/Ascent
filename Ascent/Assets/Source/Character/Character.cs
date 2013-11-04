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

    #region Fields

    protected CharacterStatistics characterStatistics;
    //protected AnimatorController animatorController;

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

    }

    public virtual void TakeDamage(int damageAmount)
    {
        // Obtain the health stat and subtract damage amount to the health.
		characterStatistics.CurrentHealth -= damageAmount;

        // If the character is dead
		if (characterStatistics.CurrentHealth <= 0)
        {
            // On Death settings
            // Update states to kill character
        }
    }

    #endregion
}
