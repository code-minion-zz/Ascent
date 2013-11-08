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


}
