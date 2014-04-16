using UnityEngine;
using System.Collections;

public class MagicMissile : Projectile
{
    private Character owner;
    private Vector3 velocity;
    private Vector3 curVelocity;

    private float timeElapsed;
    private float duration = 5.0f;

    public float maxSpeed;
    public float maxForce;
    public float mass;

    private Character target;

    public void Initialise(Vector3 startPos, Character owner)
    {
        this.owner = owner;
        transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
        transform.forward = owner.transform.forward;

        startPos.y = 0.65f;

        transform.position = startPos;
        //GetComponent<TweenPosition>().from = startPos;
        //GetComponent<TweenPosition>().to = startPos + Vector3.up * 0.5f;

        SelectTarget();
    }

    public void SelectTarget()
    {
        target = Game.Singleton.Players[0].Hero;
    }

    public Vector3 SteerToTarget()
    {
		Vector3 desiredVelocity = Vector3.zero;
        desiredVelocity = target.transform.position - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		desiredVelocity -= velocity;

		return desiredVelocity;
    }

    public void Update()
    {
        //if (timeElapsed >= duration)
        //{
        //    Destroy(gameObject);
        //}

        // Calculate the combined force from each steering behaviour
        Vector3 steeringForce = SteerToTarget();

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        // Acceleration = Force/Mass
        Vector3 acceleration = steeringForce / mass;

        // Update velocity
        velocity += acceleration * Time.deltaTime;

        // Do not allow velocity to exceed max
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), 100.0f);

        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
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
            SoundManager.PlaySound(AudioClipType.pop, transform.position, .1f);
        }
        // If the character hit is not the owner and it is not another enemy
        // then it can be destroyed.
        if (character != owner)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
