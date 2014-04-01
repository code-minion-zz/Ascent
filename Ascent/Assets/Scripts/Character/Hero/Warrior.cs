using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
	public enum ECombatAnimation
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
        //heroAnimator = GetComponent<HeroAnimator>();
        //heroAnimator.Initialise();

		// Create or Load character and init other essential things
		base.Initialise(input, saveData);
		
        // Add abilities (TODO: Save/Load/Create this.)
        loadout.SetAbility(new WarriorStrike(), (int)HeroController.EHeroAction.Strike);

        //loadout.SetAbility(new WarriorFireball(), (int)HeroController.EHeroAction.Action1);
        loadout.SetAbility(new WarriorHeavyStrike(), (int)HeroController.EHeroAction.Action1);
        loadout.SetAbility(new WarriorCharge(), (int)HeroController.EHeroAction.Action2);
        loadout.SetAbility(new WarriorWarStomp(), (int)HeroController.EHeroAction.Action3);
        //loadout.SetAbility(new WarriorFreezeField(), (int)HeroController.EHeroAction.Action3);
        //loadout.SetAbility(new WarriorWarCry(), (int)HeroController.EHeroAction.Action4);
        loadout.SetAbility(new WarriorLightning(), (int)HeroController.EHeroAction.Action4);

		vulnerabilities = EStatus.All;
	}
}
