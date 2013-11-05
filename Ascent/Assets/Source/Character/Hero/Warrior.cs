using UnityEngine;
using System.Collections;

public class Warrior : Hero 
{

	// Use this for initialization
	public override void Start () 
	{
	
	}
	
	// public is called once per frame
    public override void Update() 
	{
	
	}

    public override void Initialise(HeroSave saveData)
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
    }
}
