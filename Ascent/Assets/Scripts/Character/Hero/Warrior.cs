using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
    //bool chargeCollision = false;	
	
    public override void Initialise(InputDevice input, HeroSaveData saveData)
    {
		base.Initialise(input, saveData);

        baseStatistics = null;

        if (saveData != null)
        {
            // Populate with the savedata
            //characterStatistics = new CharacterStatistics();
            //characterStatistics.MaxHealth = saveData.health;
        }
        else
        {
            // Populate the hero with Inventory, stats, basic abilities (if any)
             
            baseStatistics = HeroBaseStats.GetNewBaseStatistics(Character.EHeroClass.Warrior);
			derivedStats = new DerivedStats(baseStatistics);
            derivedStats.MaxSpecial = 25;
            derivedStats.CurrentSpecial = 25;
        }

        // Attach the weapon mesh

        // Load the prefab
        // TODO: Change this make it more easier to load
       // weaponPrefab = Resources.Load("Prefabs/Heroes/angelic_sword_02") as GameObject;
        //weaponSlot = GetComponentInChildren<WeaponSlot>().Slot.transform;

       // if (weaponPrefab == null)
        //    Debug.Log("Weapon prefab not found");

        // Create the weapon in the weapon slot
        // Assign its parent to this object, ideally we will equip it to the players
        // weapon bone.
       // weaponPrefab = Instantiate(weaponPrefab) as GameObject;
       // weaponPrefab.transform.parent = weaponSlot;
       // weaponPrefab.transform.localPosition = Vector3.zero;
       // weaponPrefab.transform.localRotation = Quaternion.identity;
       // weaponPrefab.transform.localScale = Vector3.one;


		// Obtain the equiped weapon class from this weapon
		//equipedWeapon = weaponPrefab.GetComponent<Weapon>();
		//equipedWeapon.Initialise(this);

        // Add the animator and controller
        characterAnimator = gameObject.AddComponent<HeroAnimatorController>();
        heroController = gameObject.AddComponent<HeroController>();
		heroController.Initialise(this);
        heroController.EnableInput(input);
		
        // Add abilities
		AddSkill(new WarriorStrike());
		AddSkill(new WarriorHeavyStrike());
		AddSkill(new WarriorCharge());
		AddSkill(new WarriorWarStomp());
        AddSkill(new WarriorWarCry());

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
	
	// public is called once per frame
    public override void Update() 
	{
		base.Update();
	}
}
