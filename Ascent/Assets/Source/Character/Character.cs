using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HealthBar))]
[RequireComponent(typeof(AnimatorController))]
public class Character : MonoBehaviour
{
    #region Fields

    protected CharacterStatistics characterStatistics;
    protected AnimatorController animatorController;
    protected HealthBar healthBar;

    #endregion

    #region Properties

    public CharacterStatistics CharacterStats
    {
        get { return characterStatistics; }
    }

    public AnimatorController Animator
    {
        get { return animatorController; }
    }

    #endregion

    #region Initialization

    public virtual void Awake()
    {
        // Initialize the character statistics
        characterStatistics = new CharacterStatistics();
        characterStatistics.Init();
        characterStatistics.Health.Set(100.0f, 100.0f);

        // Get all the components
        healthBar = GetComponent<HealthBar>();
        animatorController = GetComponent<AnimatorController>();
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
        HealthStat health = characterStatistics.Health;
        health -= damageAmount;

        // If the character is dead
        if (health <= 0)
        {
            // On Death settings
            // Update states to kill character
        }
    }

    #endregion
}
