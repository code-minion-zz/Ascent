using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{
    public override void Initialise(AscentInput input, HeroSave saveData)
    {
        characterStatistics = null;

        if (saveData != null)
        {
            // Populate with the savedata
            //characterStatistics = new CharacterStatistics();
            //characterStatistics.MaxHealth = saveData.health;
        }
        else
        {
            // Populate the hero with Inventory, stats, basic abilities (if any)

            characterStatistics = HeroBaseStats.GetNewBaseStatistics(Character.EHeroClass.Warrior);
        }

        // Attach the weapon mesh

        // Load the prefab
        weaponPrefab = Resources.Load("Prefabs/angelic_sword_03") as GameObject;
        weaponSlot = transform.FindChild("Reference/Hips/Spine/Chest/RightShoulder/RightArm/RightForeArm/RightHand/WeaponSlot1");

        if (weaponPrefab == null)
            Debug.Log("Weapon prefab not found");

        // Create the weapon in the weapon slot
        // Assign its parent to this object, ideally we will equip it to the players
        // weapon bone.
        weaponPrefab = Instantiate(weaponPrefab) as GameObject;
        weaponPrefab.transform.parent = weaponSlot.transform;
        weaponPrefab.transform.localPosition = Vector3.zero;


        // Obtain the equiped weapon class from this weapon
        equipedWeapon = weaponPrefab.GetComponent<Weapon>();

        equipedWeapon.Initialise(this);


        // Add the animator and controller
        characterAnimator = gameObject.AddComponent<HeroAnimator>();
        heroController = gameObject.AddComponent<HeroController>();
		heroController.Initialise(this);
        heroController.EnableInput(input);
		
		// Add charge collider
		GameObject hitDisc = Resources.Load("Prefabs/HitDisc") as GameObject;
        if (hitDisc == null)
            Debug.Log("HitDisc prefab not found");
		hitDisc = Instantiate(hitDisc) as GameObject;
		hitDisc.GetComponent<HitBox>().Init(HitBox.EBoxAnimation.BA_HIT_THRUST,0,0f,100f,0f);
		hitDisc.transform.localPosition = Vector3.zero;
		hitDisc.transform.parent = transform;
		
        // Add abilities
		AddSkill(new SwingSword());
//        IAction swordSwing = new SwingSword();
//        swordSwing.Initialise(this);
//        abilities.Add(swordSwing);
		
		AddSkill(new Jump());
//		IAction jump = new Jump();
//		jump.Initialise(this);
//		abilities.Add(jump);
		
		AddSkill(new Roll());
//		IAction roll = new Roll();
//		roll.Initialise(this);
//		abilities.Add(roll);
		
		AddSkill(new Charge());
//		IAction charge = new Charge();
//		charge.Initialise(this);
//		abilities.Add(charge);
    }
	
	// public is called once per frame
    public override void Update() 
	{
		base.Update();
	}
	
	private void AddSkill(IAction skill)
	{
		skill.Initialise(this);
		abilities.Add(skill);
	}
}
