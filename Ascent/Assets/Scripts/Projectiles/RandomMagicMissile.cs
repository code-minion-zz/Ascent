﻿using UnityEngine;
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
		transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
		transform.forward = owner.transform.forward;

		startPos.y = 1.25f;

		transform.position = startPos;

		Vector3 rotation = Vector3.zero;
		rotation.y = Random.Range(-360.0f, 360.0f);

		transform.Rotate(rotation);
	}


	public void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= 3.5f && !exploded)
		{
			exploded = true;
			GameObject.Instantiate(arcaneExplosionPrefab, transform.position, transform.rotation);
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

			Game.Singleton.EffectFactory.CreateBloodSplatter(collision.transform.position,
				collision.transform.rotation, character.transform);

			GameObject.Instantiate(arcaneExplosionPrefab, transform.position, transform.rotation);
		}
		else
		{
			SoundManager.PlaySound(AudioClipType.pop, transform.position, .1f);
		}
		// If the character hit is not the owner and it is not another enemy
		// then it can be destroyed.
		if (character != owner)
		{
			GameObject.Instantiate(arcaneExplosionPrefab, transform.position, transform.rotation);
			GameObject.Destroy(gameObject);
		}
	}
}
