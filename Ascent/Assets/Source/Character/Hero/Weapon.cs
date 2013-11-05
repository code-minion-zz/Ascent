using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class Weapon : MonoBehaviour
{
    BoxCollider boxCollider;
    Character owner;

    int damage;
    Character.EDamageType damageType;

    public bool EnableCollision
    {
        get { return boxCollider.enabled; }
        set { boxCollider.enabled = value; }
    }

    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        //enabled = false;
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

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case "Hero":
                {
                    Character otherCharacter = other.GetComponent<Character>();
                    otherCharacter.ApplyDamage(damage, damageType);
                    
                    Debug.Log(this.name + " collides with " + otherCharacter);
                }
                break;
            case "Enemy":
                {
                    Character otherCharacter = other.GetComponent<Character>();
                    otherCharacter.ApplyDamage(damage, damageType);


                    otherCharacter.ApplyKnockback(Vector3.Normalize(new Vector3(otherCharacter.transform.position.x, 0.0f, otherCharacter.transform.position.z) - new Vector3(owner.transform.position.x, 0.0f, owner.transform.position.z)), 10.0f);
                    Debug.Log(this.name + " collides with " + otherCharacter);
                }
                break;
            default:
                {
                    //Debug.Log(this.name + " colliding with " + other.name + " but not handled");
                }
                break;
        }
    }
}
