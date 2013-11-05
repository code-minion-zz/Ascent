// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;

public class Rat : Enemy 
{
	public override void Initialise()
	{
		// Populate with stats

        characterStatistics = new CharacterStatistics();
        characterStatistics.MaxHealth = 100;
        characterStatistics.CurrentHealth = 100;
	}

    public override void Start()
    {
        Initialise();
    }
	
}
