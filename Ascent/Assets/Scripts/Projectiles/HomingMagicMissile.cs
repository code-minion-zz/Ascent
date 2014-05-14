using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingMagicMissile : Projectile
{
    private Character owner;
    private Vector3 curVelocity;

    private float timeElapsed;

	public Vector2 SpeedMinMax;
	public Vector2 ForceMinMax;

	public float lifeSpan = 5.0f;

    private float maxSpeed;
	private float maxForce;

    private Character target;

	private bool exploded;

	public GameObject arcaneExplosionPrefab;

    public void Initialise(Vector3 startPos, Character owner, Character target)
    {
        this.owner = owner;
        transform.position = new Vector3(startPos.x, 2.0f, startPos.z);
        transform.forward = owner.transform.forward;

		maxSpeed = Random.Range(SpeedMinMax.x, SpeedMinMax.y);
		maxForce = Random.Range(ForceMinMax.x, ForceMinMax.y);

		Vector3 rotation = Vector3.zero;
		rotation.y = Random.Range(-360.0f, 360.0f);

		transform.Rotate(rotation);

		this.target = target;
        //SelectTarget();
    }

    public void SelectTarget()
    {
		var aliveHeroes =  Game.Singleton.AliveHeroes;

		if (aliveHeroes.Count > 0)
		{
			int randomPlayer = Random.Range(0, aliveHeroes.Count);

			target = aliveHeroes[randomPlayer];

			return;
		}


		int playerCount = Game.Singleton.NumberOfPlayers;

		int randPlayer = Random.Range(0, playerCount);

		target = Game.Singleton.Players[randPlayer].Hero;
    }

    public Vector3 SteerToTarget()
    {
		Vector3 desiredVelocity = Vector3.zero;
		Vector3 targetPos = target.transform.position;
		targetPos.y = 1.5f;
		desiredVelocity = targetPos - transform.position;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		desiredVelocity -= rigidbody.velocity;

		return desiredVelocity;
    }

    public void Update()
    {
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= lifeSpan)
		{
			Destroy(gameObject);
			return;
		}

        // Calculate the combined force from each steering behaviour
		Vector3 steeringForce = SteerToTarget();

		steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

		// Acceleration = Force/Mass
		Vector3 acceleration = steeringForce / rigidbody.mass;

		//// Update velocity
		Vector3 clampedVelocity = rigidbody.velocity;
		clampedVelocity += acceleration * Time.deltaTime;

		// Do not allow velocity to exceed max
		clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maxSpeed);

		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 100.0f * Time.deltaTime);
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), 100.0f);

		//rigidbody.AddForce(transform.forward * velocity.magnitude, ForceMode.VelocityChange);

		Vector3 velocityChange = steeringForce - clampedVelocity;

		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
		if (timeElapsed >= 3.55f)
		{
			return;
		}

		Character character = collision.gameObject.GetComponent<Character>();

        if (character == owner)
        {
            return;
        }

        if (collision.transform.tag == "Monster")
        {
            return;
        }

		if (collision.transform.tag == "Hero")
		{
			CombatEvaluator combatEvaluator = new CombatEvaluator(owner, character);
			combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
			combatEvaluator.Apply();
		}
		else
		{
			SoundManager.PlaySound(AudioClipType.pop, transform.position, .2f);
		}

        Vector3 closestPoint = collision.collider.ClosestPointOnBounds(transform.position);
		GameObject go = EffectFactory.Singleton.CreateArcaneExplosion(closestPoint, transform.rotation);
		go.transform.parent = EffectFactory.Singleton.transform;
		Destroy(gameObject);
    }
}
