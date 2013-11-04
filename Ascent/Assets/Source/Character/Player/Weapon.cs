using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public void Awake()
    {

    }

    public void Start()
    {

    }

    public void Initialise(HeroSave saveData)
    {
        //if(saveData != null)
        //{
        //    // Populate with the savedata
        //}
        //else
        //{
        //    if (weaponPrefab == null)
        //        Debug.Log("Weapon prefab not found");

        //    // Create the weapon in the weapon slot
        //    // Assign its parent to this object, ideally we will equip it to the players
        //    // weapon bone.
        //    weaponPrefab = Instantiate(weaponPrefab) as GameObject;
        //    weaponPrefab.transform.parent = weaponSlot.transform;
        //    weaponPrefab.transform.localPosition = Vector3.zero;


        //    // Obtain the equiped weapon class from this weapon
        //    equipedWeapon = weaponPrefab.GetComponent<Weapon>();
        //}
    }
}
