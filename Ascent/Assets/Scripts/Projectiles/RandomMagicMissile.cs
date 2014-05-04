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
		transform.position = new Vector3(startPos.x, 2.0f, startPos.z);
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
			SoundManager.PlaySound(AudioClipType.pop, transform.position, .5f);
		}

        Vector3 closestPoint = collision.collider.ClosestPointOnBounds(transform.position);
        EffectFactory.Singleton.CreateArcaneExplosion(closestPoint, transform.rotation);
		GameObject.Destroy(gameObject);
	}
}
