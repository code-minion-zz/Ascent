using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
    //bool chargeCollision = false;	
	
    public override void Initialise(InputDevice input, HeroSaveData saveData)
    {		
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
        weaponPrefab = Resources.Load("Prefabs/Heroes/angelic_sword_03") as GameObject;
        //weaponSlot = transform.FindChild("Reference/Hips/Spine/Chest/RightShoulder/RightArm/RightForeArm/RightHand/WeaponSlot1");
        weaponSlot = GetComponentInChildren<WeaponSlot>().Slot.transform;

        if (weaponPrefab == null)
            Debug.Log("Weapon prefab not found");

        // Create the weapon in the weapon slot
        // Assign its parent to this object, ideally we will equip it to the players
        // weapon bone.
        weaponPrefab = Instantiate(weaponPrefab) as GameObject;
        weaponPrefab.transform.parent = weaponSlot;
        weaponPrefab.transform.localPosition = Vector3.zero;
        weaponPrefab.transform.localRotation = Quaternion.identity;
        weaponPrefab.transform.localScale = Vector3.one;


        // Obtain the equiped weapon class from this weapon
        equipedWeapon = weaponPrefab.GetComponent<Weapon>();

        equipedWeapon.Initialise(this);


        // Add the animator and controller
        characterAnimator = gameObject.AddComponent<HeroAnimator>();
        heroController = gameObject.AddComponent<HeroController>();
		heroController.Initialise(this);
        heroController.EnableInput(input);
		
		// Add charge collider
		GameObject collisionBall = Resources.Load("Prefabs/CollisionBall") as GameObject;
        if (collisionBall == null)
            Debug.Log("CollisionBall prefab not found");
        collisionBall = Instantiate(collisionBall) as GameObject;
		collisionBall.transform.parent = transform;
		collisionBall.transform.localPosition = transform.forward * 0.5f + Vector3.up;
		collisionBall.transform.localScale = new Vector3(1f,1f,1f);
		chargeBall = collisionBall.GetComponent<Collidable>();
		chargeBall.Init(this);
		
        // Add abilities
		AddSkill(new SwingSword());
		
		//AddSkill(new Jump());
		
		AddSkill(new Whirlwind());
		AddSkill(new Charge());
		AddSkill(new WarStomp());
        AddSkill(new WarCry());

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
