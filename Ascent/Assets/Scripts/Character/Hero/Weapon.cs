using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Weapon works as more of a description that tells abilities how to perform. If an item
/// has different properties it will taken into affect by abilities that use the weapon.
/// </summary>
public class Weapon : MonoBehaviour
{
    private Character owner;

    private int damage;
    private float knockBackValue;
    private Character.EDamageType damageType;

    /// <summary>
    /// Gets the owner of this weapon
    /// </summary>
    public Character Owner
    {
        get { return owner; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float KnockBackValue
    {
        get { return knockBackValue; }
        set { knockBackValue = value; }
    }

    public Character.EDamageType DamageType
    {
        get { return damageType; }
    }

    public void Awake()
    {

    }

    public void Initialise(Character character)
    {
        owner = character;
    }

    public void SetAttackProperties(int damage, Character.EDamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public void Update()
    {

    }
}
