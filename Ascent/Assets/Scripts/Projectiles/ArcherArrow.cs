using UnityEngine;
using System.Collections;

public class ArcherArrow : Projectile
{
    private Character owner;
    private Vector3 velocity;
    private Vector3 curVelocity;

    public void Initialise(Vector3 startPos, Vector3 velocity, Character owner)
    {
        this.owner = owner;
        this.velocity = velocity;
        projectile.transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
        projectile.transform.LookAt(startPos - velocity, Vector3.up);
        projectile.rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        //projectile.rigidbody.AddTorque(new Vector3(Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f)));
    }

    public void Update()
    {
        projectile.rigidbody.AddForce(velocity, ForceMode.Force);
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

            Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position, 
                collision.transform.rotation, character.transform);
        }
		else
		{			
			SoundManager.PlaySound(AudioClipType.pop,transform.position,.1f);
		}
        // If the character hit is not the owner and it is not another enemy
        // then it can be destroyed.
        if (character != owner)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
