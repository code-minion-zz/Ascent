using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherLazer : Projectile
{
	private class HeroHitTimerPair
	{
		public float timeElapsed;
		public Hero hero;
	}

	public LazerBeam beam;
	public float timeBetweenTicks;
	public int damage;

	private Character owner;
	private List<HeroHitTimerPair> heroesHit = new List<HeroHitTimerPair>();


	public void Initialise(Vector3 startPos, Character owner)
	{
		this.owner = owner;
		transform.position = startPos;
		transform.parent = owner.transform;
	}

	public void OnDisable()
	{
		ClearList();
	}

	public void Update()
	{
		UpdateDamageTicks();
		UpdateHeroList();
	}

	public void UpdateDamageTicks()
	{
		foreach(HeroHitTimerPair heroTimePair in heroesHit)
		{
			
			// Apply the damage tick
			heroTimePair.timeElapsed += Time.deltaTime;
			if (heroTimePair.timeElapsed >= timeBetweenTicks)
			{
				heroTimePair.timeElapsed -= timeBetweenTicks;
				DealDamage(heroTimePair.hero);
			}
			else
			{
				DealKnockback(heroTimePair.hero);
			}
		}
	}

	public void UpdateHeroList()
	{
		// Check if an enemy was hit
		if(beam.LastHit == null)
		{
			ClearList();

			return;
		}

		Hero hero = beam.LastHit.GetComponent<Hero>();
		if (hero == null)
		{
			ClearList();

			return;
		}

		// This will deal damage over time.
		if (!heroesHit.Exists(x => x.hero == hero))
		{
			HeroHitTimerPair heroTimePair = new HeroHitTimerPair();
			heroTimePair.timeElapsed = 0.0f;
			heroTimePair.hero = hero;

			heroesHit.Add(heroTimePair);

			DealDamage(hero);
		}
	}

	public void DealDamage(Hero damageTaker)
	{
		CombatEvaluator combatEvaluator = new CombatEvaluator(owner, damageTaker);
		combatEvaluator.Add(new PhysicalDamageProperty(3.0f, 1.0f));
		combatEvaluator.Add(new KnockbackCombatProperty(damageTaker.transform.position - owner.transform.position, 1.0f));
		combatEvaluator.Apply();
	}

	public void DealKnockback(Hero damageTaker)
	{
		CombatEvaluator combatEvaluator = new CombatEvaluator(owner, damageTaker);
		combatEvaluator.Add(new KnockbackCombatProperty(damageTaker.transform.position - owner.transform.position, 1.0f));
		combatEvaluator.Apply();
	}

	public void ClearList()
	{
		heroesHit.Clear();
		heroesHit.TrimExcess();
	}
}
