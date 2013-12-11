//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public abstract class ItemProperty 
//{
//	float timeAccumulator;
//	//public abstract void Initialize();
////	public abstract void CheckCondition();
////	public abstract void DoAction ();	// condition is met, do action
////	public abstract void OnEquip();
////	public abstract void OnUnequip();
//}
//
///// <summary>
///// Performs an action every frame
///// </summary>
//public class ConstantItemProperty : ItemProperty
//{
//	public void CheckCondition()
//	{
//		//DoAction ();
//	}
//}
//
//public class ThresholdItemProperty : ItemProperty
//{
//}
//
//public class AuraItemProperty : ItemProperty
//{
//	//protected Hero hero;
//	protected AuraAbility aura;
//
//	public  void Initialise(Hero hero)
//	{
//		//base.Initialise(hero);
//
//		aura = new AuraAbility();
//	}
//
//	public void Process()
//	{
//		aura.Process();
//
//	}
//}
//
//public class AuraAbility : Ability
//{
//	protected List<Character> charactersInAuraRange;
//
//	public void Initialise(Hero hero)
//	{
//		// Add collider to hero
//	}
//
//	public override void Activate()
//	{
//	}
//
//	public void Process()
//	{
//		// Objects entering collider added to list.
//		// Objects removing collider remove from list.
//		// Apply effect onto characters in the list
//	}
//}
//
////public class OnDamageTakenItemProperty : ItemProperty
////{
////	protected int chance;
////	protected Ability ability;
////
////	public override void Initialise(Hero hero)
////	{
////		base.Initialise(hero);
////		//ability = new SpawnSkull();
////	}
////
////	public void OnEnable()
////	{
////		hero.OnDamageTaken += OnDamageTaken;
////	}
////
////	public void OnDisable()
////	{
////		hero.OnDamageTaken -= OnDamageTaken;
////	}
////
////	public void OnDamageTaken()
////	{
////		if( Random.Range(0, 100) > chance)
////		{
////			ability.Activate(); 
////		}
////	}
//
////	public void OnTargetHit(Character target)
////	{
////
////	}
////}
