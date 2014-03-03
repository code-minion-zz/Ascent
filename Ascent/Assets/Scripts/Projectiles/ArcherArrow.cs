using UnityEngine;
using System.Collections;

public class ArcherArrow : Projectile
{
    public Projectile projectile;

    private Character owner;
    private Vector3 velocity;
    private Vector3 curVelocity;

    public void Initialise(Vector3 startPos, Vector3 velocity, Character owner)
    {
        this.owner = owner;
        this.velocity = velocity;
        projectile.transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
        projectile.rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        projectile.rigidbody.AddTorque(new Vector3(Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f), Random.Range(1.0f, 100.0f)));
    }

    public void Update()
    {
        projectile.rigidbody.AddForce(velocity, ForceMode.Force);
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Hero")
        {
            CombatEvaluator combatEvaluator = new CombatEvaluator(owner, collision.gameObject.GetComponent<Character>());
            combatEvaluator.Add(new PhysicalDamageProperty(0.0f, 1.0f));
            combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 10000.0f));
            combatEvaluator.Apply();
        }

        GameObject.Destroy(this.gameObject);
    }
}
