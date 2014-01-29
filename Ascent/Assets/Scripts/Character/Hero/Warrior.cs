using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
    public override void Initialise(InputDevice input, HeroSaveData saveData)
    {
		classType = EHeroClass.Warrior;

		base.Initialise(input, saveData);

        baseStatistics = null;

		if (saveData != null)
		{
			baseStatistics = saveData.baseStats;
			derivedStats = new DerivedStats(baseStatistics);
			derivedStats.MaxSpecial = 25;
			derivedStats.CurrentSpecial = 25;
		}
		else
		{
			// Populate the hero with Inventory, stats, basic abilities (if any)

			baseStatistics = HeroBaseStats.GetNewBaseStatistics(Character.EHeroClass.Warrior);
			derivedStats = new DerivedStats(baseStatistics);
			derivedStats.MaxSpecial = 25;
			derivedStats.CurrentSpecial = 25;
		}

		// Initialise hero Specific animator
		heroAnimator = GetComponent<HeroAnimator>();
		heroAnimator.Initialise();
		
		// Initialise Controller
        heroController = gameObject.GetComponent<HeroController>();
		heroController.Initialise(this, animator, motor);
        heroController.SetInputDevice(input);
		
        // Add abilities
		AddSkill(new WarriorStrike());
		AddSkill(new WarriorHeavyStrike());
		AddSkill(new WarriorCharge());
		AddSkill(new WarriorWarStomp());
        AddSkill(new WarriorWarCry());

		// Set Stat Mods
		classStatMod = new Hero.HeroClassStatModifier();
		classStatMod.PowerAttack = 1f;
		classStatMod.FinesseCritChance = 1f;
		classStatMod.FinesseCritBonus = 1f;
		classStatMod.FinesseDodge = 1f;
		classStatMod.FinesseBlock = 1f;
		classStatMod.VitalityHP = 1f;
		classStatMod.VitalityPhysRes = 1f;
		classStatMod.VitalityHPRegen = 1f;
		classStatMod.SpiritSP = 1f;
		classStatMod.SpiritMagRes = 1f;
    }
}
