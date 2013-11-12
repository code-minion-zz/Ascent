using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class Weapon : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Character owner;
    private List<Character> collidedTargets = new List<Character>();

    private int damage;
    private float knockBackValue = 5.0f;
    private Character.EDamageType damageType;
    private GameObject bloodSplat;

    /// <summary>
    /// Enables and disables the collision box collider.
    /// </summary>
    public bool EnableCollision
    {
        get { return boxCollider.enabled; }
        set { boxCollider.enabled = value; }
    }

    /// <summary>
    /// Gets the owner of this weapon
    /// </summary>
    public Character Owner
    {
        get { return owner; }
    }

    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        bloodSplat = Resources.Load("BloodSplat/BloodSplat") as GameObject;
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

    public void Update()
    {
        // When the collision is disabled
        if (EnableCollision == false)
        {
            // If we have targets that we collided with
            if (collidedTargets.Count > 0)
            {
                Debug.Log("Collided with " + collidedTargets.Count + " targets");

                foreach (Character other in collidedTargets)
                {
                    // Sanity check to make sure that the other character still exists
                    if (other != null)
                    {
                        // Apply damage value to other character
                        other.ApplyDamage(damage, damageType);

                        //other.LastDamagedBy = null;
                        // We can remove this collision as it is no longer in effect.
                        other.LastObjectsDamagedBy.Remove(this);

                        // Log out the character that was hit and by.
                        Debug.Log(this.name + " collides with " + other);
                    }
                }

                // We want to clear the list now so that we do not repeat this process.
                collidedTargets.Clear();
            }
        }
    }

    /// <summary>
    /// In the case that this weapon collider entered collision with another collider.
    /// </summary>
    /// <param name="other">The other collider that this weapon went into collision with.</param>
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
            case "Monster":
                {
                    CollideWithEnemy(other.GetComponent<Character>() as Enemy);
                }
                break;
            default:
                {
                    //Debug.Log(this.name + " colliding with " + other.name + " but not handled");
                }
                break;
        }
    }

    /// <summary>
    /// In the case that this weapon collider exited collision with another collider.
    /// </summary>
    /// <param name="other">The other collider that this weapon came out of collision with.</param>
    void OnTriggerExit(Collider other)
    {
 
    }

    /// <summary>
    /// When the weapon collides with an object of type enemy
    /// </summary>
    /// <param name="other">The enemy that the weapon collided with</param>
    protected void CollideWithEnemy(Enemy other)
    {
        // If there is an object in this list that is the same
        // as this object it means we have a double collision.
        foreach (Object obj in other.LastObjectsDamagedBy)
        {
            if (obj == this)
                return;
        }

        // Apply knockback direction by obtaining the distance between weapon pos and enemy pos.
        // We should apply knock back immediatly so that it does not look strange.
        Vector3 ownerPos = owner.transform.position;
        Vector3 enemyPos = other.transform.position;

        Vector3 direction = Vector3.Normalize(enemyPos - ownerPos);
        //Vector3 splatterStart = other.transform.position;
        Vector3 splatterStart = other.collider.ClosestPointOnBounds(this.transform.position);

        // Apply knock back and tell the enemy it was hit by this weapon object.
        // We can succesfully say we hit this character now and we set their last hit by to null.
        other.ApplyKnockback(direction, knockBackValue);

        // Apply particle blood splatter and make it a parent of the enemy so that it will move with the enemy.
        // TODO: make a pool of these emitters and dont instantiate them on the frame.
        GameObject bloodSplatter = Instantiate(bloodSplat, splatterStart, other.collider.transform.rotation) as GameObject;
        bloodSplatter.transform.parent = other.transform;

        // Now we say ok this enemy was hit by this weapon.
        other.LastObjectsDamagedBy.Add(this);

        // Update our list of collided targets
        // If a weapon has special properties where it may only be able to hit a number of targets, 
        // we would check to see if the count is too high before adding to the targets list.
        collidedTargets.Add(other);
    }
}
