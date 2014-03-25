using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour 
{
	public void Initialise(int damage)
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.transform.tag)
		{
			case "Hero":
				{
					CollideWithHero(collision.transform.GetComponent<Character>() as Hero, collision);
				}
				break;
			case "Block":
				{
					transform.parent.parent.GetComponent<SpinningBlade>().HaltRotation();
				}
				break;
		}
	}

	void OnCollisionExit(Collision collision)
	{

		switch (collision.transform.tag)
		{
			case "Block":
				{
					transform.parent.parent.GetComponent<SpinningBlade>().ResumeRotation();
				}
				break;
		}
	}

	/// <summary>
	/// When the arrow collides with a hero.
	/// </summary>
	/// <param name="hero">Hero.</param>
	/// <param name="collision">Collision.</param>
	private void CollideWithHero(Hero hero, Collision collision)
	{
		// Apply damage to the hero
		CombatEvaluator combatEvaluator = new CombatEvaluator(null, hero);
		combatEvaluator.Add(new TrapDamageProperty(2.0f, 1.0f));
		combatEvaluator.Add(new KnockbackCombatProperty(-collision.contacts[0].normal, 1.0f));
		combatEvaluator.Apply();

	}
}
