using UnityEngine;
using System.Collections;

public class ArcherArrow : Projectile
{
    private Character owner;
    private Vector3 velocity;
    private Vector3 curVelocity;

    public GameObject fireBallExplosionPrefab;

    public void Initialise(Vector3 startPos, Vector3 velocity, Character owner)
    {
        this.owner = owner;
        this.velocity = velocity;
        transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
        transform.LookAt(startPos - velocity, Vector3.up);
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        //projectile.rigidbody.AddTorque(new Vector3(Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f)));
    }

    public void Update()
    {
        rigidbody.AddForce(velocity, ForceMode.Force);
    }

    public void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        Hero hero = character as Hero;

        if (character == owner)
        {
            return;
        }

        if (other.tag == "Monster")
        {
            return;
        }

        if (hero != null)
        {
            CombatEvaluator combatEvaluator = new CombatEvaluator(owner, character);
            combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
            combatEvaluator.Apply();
        }
        else
        {
            SoundManager.PlaySound(AudioClipType.pop, transform.position, .1f);
        }

        // If the character hit is not the owner and it is not another enemy
        // then it can be destroyed.
        Vector3 closestPoint = other.ClosestPointOnBounds(transform.position);
        GameObject.Instantiate(fireBallExplosionPrefab, closestPoint, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
