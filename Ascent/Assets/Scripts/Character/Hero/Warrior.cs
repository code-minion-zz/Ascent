using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
    public override void Initialise(InputDevice input, HeroSaveData saveData)
    {
		heroClass = EHeroClass.Warrior;

		// Initialise Warrior Specific animator
		heroAnimator = GetComponent<HeroAnimator>();
		heroAnimator.Initialise();

		// Create or Load character and init other essential things
		base.Initialise(input, saveData);
		
        // Add abilities (TODO: Save/Load/Create this.)
		AddSkill(new WarriorStrike());
		AddSkill(new WarriorHeavyStrike());
		AddSkill(new WarriorCharge());
		AddSkill(new WarriorWarStomp());
        AddSkill(new WarriorWarCry());
    }
}
