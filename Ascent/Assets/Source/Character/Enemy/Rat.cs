// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Rat : Enemy 
{
	public override void Start()
	{
		Initialise();
	}

	public override void Initialise()
	{
		// Populate with stats

        characterStatistics = new CharacterStatistics();
        characterStatistics.MaxHealth = 100;
        characterStatistics.CurrentHealth = 100;

        // Add abilities
        IAction swordSwing = new SwingSword();

        swordSwing.Initialise(this);
        abilities.Add(swordSwing);

        // Add abilities
        IAction charge = new Charge();

        charge.Initialise(this);
        abilities.Add(charge);
	}

    // We want to override the on death for this rat as we have some specific behaviour here.
    public override void OnDeath()
    {
        base.OnDeath();
        // Play some cool animation
        // Maybe even play a sound here
        // Maybe even drop some loot here

        // Rat is going to destroy itself now
        //DestroyObject(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
