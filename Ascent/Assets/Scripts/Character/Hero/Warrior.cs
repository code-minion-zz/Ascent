using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
	public enum ECombatAnimations
	{
		None = -1,
		Strike1 = 0,
        Strike2 = 1,
        Strike3 = 2,
		HeavyStrike = 3,
		WarStromp = 4,
		Charge = 5,
		Warcry = 6,
        ChargeCrouch = 7,
        
	}

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
