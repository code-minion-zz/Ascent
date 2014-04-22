using UnityEngine;
using System.Collections;

public class RandomMagicMissile : Projectile
{
	private Character owner;
	private Vector3 curVelocity;

	private float timeElapsed;

	public float lifeSpan = 5.0f;

	private Character target;

	private bool exploded;

	public GameObject arcaneExplosionPrefab;

	public void Initialise(Vector3 startPos, Character owner)
	{
		this.owner = owner;
		transform.position = new Vector3(startPos.x, 0.5f, startPos.z);
		transform.forward = owner.transform.forward;

		Vector3 rotation = Vector3.zero;
		rotation.y = Random.Range(-360.0f, 360.0f);

		transform.Rotate(rotation);

		transform.parent = EffectFactory.Singleton.transform;
	}


	public void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= 3.5f && !exploded)
		{
			exploded = true;
			EffectFactory.Singleton.CreateArcaneExplosion(transform.position, transform.rotation);
		}
		else if (timeElapsed >= lifeSpan)
		{
			Destroy(gameObject);
			return;
		}

		transform.Translate(new Vector3(0.0f, 0.0f, 4.0f * Time.deltaTime));
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (timeElapsed >= 3.5f)
		{
			return;
		}

		Character character = collision.gameObject.GetComponent<Character>();

		if (collision.transform.tag == "Hero")
		{
			CombatEvaluator combatEvaluator = new CombatEvaluator(owner, character);
			combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
			//combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 10000.0f));
			combatEvaluator.Apply();

            EffectFactory.Singleton.CreateBloodSplatter(collision.transform.position, collision.transform.rotation);

			EffectFactory.Singleton.CreateArcaneExplosion(transform.position, transform.rotation);
		}
		else
		{
			SoundManager.PlaySound(AudioClipType.pop, transform.position, .1f);
		}
		// If the character hit is not the owner and it is not another enemy
		// then it can be destroyed.
		if (character != owner)
		{
			EffectFactory.Singleton.CreateArcaneExplosion(transform.position, transform.rotation);
			GameObject.Destroy(gameObject);
		}
	}
}
