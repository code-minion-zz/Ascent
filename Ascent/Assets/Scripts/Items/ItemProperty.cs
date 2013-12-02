using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemProperty 
{
	protected Hero hero;
	int trigger;

	public virtual void Initialise(Hero hero)
	{
		this.hero = hero;
	}

	public enum Trigger
	{
		INVALID_CONDITION = -1,

		ALWAYS,
		ONATTACK,
		ONDAMAGED,
		HPABOVE,
		HPBELOW,
		SPABOVE,
		SPBELOW,
		ONKILL,
		ONDEATH,

		MAX_CONDITION
	}

	public Trigger ipTrigger;
}

public class ConstantStatItemProperty : ItemProperty
{

}

public class CriticalHitChanceItemProperty : ItemProperty
{
	protected int value;
}

public class AuraItemProperty : ItemProperty
{
	//protected Hero hero;
	protected AuraAbility aura;

	public override void Initialise(Hero hero)
	{
		base.Initialise(hero);

		aura = new AuraAbility();
	}

	public void Process()
	{
		aura.Process();

	}
}

public class AuraAbility : Ability
{
	protected List<Character> charactersInAuraRange;

	public void Initialise(Hero hero)
	{
		// Add collider to hero
	}

	public override void Activate()
	{
	}

	public void Process()
	{
		// Objects entering collider added to list.
		// Objects removing collider remove from list.
		// Apply effect onto characters in the list
	}
}

public class OnDamageTakenItemProperty : ItemProperty
{
	protected int chance;
	protected Ability ability;

	public override void Initialise(Hero hero)
	{
		base.Initialise(hero);
		//ability = new SpawnSkull();
	}

	public void OnEnable()
	{
		hero.OnDamageTaken += OnDamageTaken;
	}

	public void OnDisable()
	{
		hero.OnDamageTaken -= OnDamageTaken;
	}

	public void OnDamageTaken()
	{
		if( Random.Range(0, 100) > chance)
		{
			ability.Activate(); 
		}
	}

	public void OnTargetHit(Character target)
	{

	}
}
