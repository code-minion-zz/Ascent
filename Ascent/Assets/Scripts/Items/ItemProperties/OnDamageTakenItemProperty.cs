using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
	
	public void OnDamageTaken(float val)
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
