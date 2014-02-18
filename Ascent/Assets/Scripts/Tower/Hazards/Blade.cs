using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour 
{
	private int damage;

	public void Initialise(int damage)
	{
		this.damage = damage;
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
		hero.ApplyDamage(damage, Character.EDamageType.Trap, null);
		hero.ApplyKnockback(-collision.contacts[0].normal, 1.0f);

	}
}
