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


    public void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (collision.transform.tag == "Hero")
		{
            CombatEvaluator combatEvaluator = new CombatEvaluator(owner, character);
            combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
            //combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 10000.0f));
            combatEvaluator.Apply();
        }
		else
		{			
			SoundManager.PlaySound(AudioClipType.pop,transform.position,.1f);
		}

        // If the character hit is not the owner and it is not another enemy
        // then it can be destroyed.
        if (character != owner)
        {
            GameObject.Instantiate(fireBallExplosionPrefab, transform.position, transform.rotation);
            GameObject.Destroy(this.gameObject);
        }
    }
}
