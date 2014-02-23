using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageResult
{
	public Character.EDamageType damageType;

	public Character source;
	public Character target;
	
	public bool criticalHit;
	public bool dodged;
	
	public int finalDamage;

	public Vector3 knockbackDirection;
	public float knockbackMagnitute;

	public bool first;

	public DamageResult(Character source, Character target)
	{
		this.source = source;
		this.target = target;
		first = true;
	}
}

public class CombatEvaluator
{
	private Character source;
	private Character target;

	public CombatEvaluator(Character source, Character target)
	{
		this.source = source;
		this.target = target;
	}

	private List<CombatProperty> damageProperties = new List<CombatProperty>();

	public DamageResult Apply()
	{
		DamageResult result = new DamageResult(source, target);

		foreach(CombatProperty prop in damageProperties)
		{
			if (!result.dodged)
			{
				result = prop.Evaluate(result, target, source);
			}

			result.first = false;
		}

		target.ApplyCombatEffects(result);

		return result;
	}

	public void Add(CombatProperty property)
	{
		damageProperties.Add(property);
	}
}

public abstract class CombatProperty
{
	public Character.EDamageType damageType;

	public abstract DamageResult Evaluate(DamageResult result, Character target, Character source);
}

public class DamageProperty : CombatProperty
{
	public float addFixedDamage;
	public float addMultiplerDamage;

	public DamageProperty(float addFixedDamage, float addMultiplerDamager)
	{
		this.addFixedDamage = addFixedDamage;
		this.addMultiplerDamage = addMultiplerDamager;
	}

	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		// Should be overridden
		return null;
	}
}

public class PhysicalDamageProperty : DamageProperty
{
	public PhysicalDamageProperty(float addFixedDamage, float addMultiplerDamager)
		: base(addFixedDamage, addMultiplerDamager)
	{
		damageType = Character.EDamageType.Physical;
	}

	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		// Check for Dodge
		bool dodged = false;
		float maxDodgeChance = 100.0f;
		float dodgeChance = Mathf.Min(source.Stats.DodgeChance, maxDodgeChance);

		if (Random.Range(0.0f, maxDodgeChance) <= dodgeChance)
		{
			// Dodged success
			dodged = true;
		}

		float finalDamage = 0.0f;
		bool criticalStrike = false;

		if (!dodged)
		{
			// Check for critical strike
			float maxCritChance = 100.0f;
			float critChance = Mathf.Min(source.Stats.CriticalHitChance, maxCritChance);

			if (Random.Range(0.0f, maxCritChance) <= critChance)
			{
				// Dodged success
				criticalStrike = true;
			}

			// Evaluate unmitigated damage
			float unmitigatedDamage = (source.Stats.Attack * addMultiplerDamage) + addFixedDamage;

			// Add crit damage if there was a crit!
			unmitigatedDamage = (criticalStrike ? unmitigatedDamage * source.Stats.CritalHitMultiplier : unmitigatedDamage);

			// Work out final damage
			finalDamage = Mathf.Max(Mathf.RoundToInt((float)source.Stats.Level * ((float)(source.Stats.Level * unmitigatedDamage)) / (float)(target.Stats.Level * target.Stats.PhysicalDefense)), 1);

		}

		result.damageType = Character.EDamageType.Physical;
		result.criticalHit = criticalStrike;
		result.dodged = dodged;
		result.finalDamage = (int)finalDamage;

		return result;
	}
}

public class MagicalDamageProperty : DamageProperty
{
	public MagicalDamageProperty(float addFixedDamage, float addMultiplerDamager)
		: base(addFixedDamage, addMultiplerDamager)
	{
		damageType = Character.EDamageType.Magical;
	}


	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		return null;
	}
}

public class TrapDamageProperty : DamageProperty
{
	public TrapDamageProperty(float addFixedDamage, float addMultiplerDamager)
		: base(addFixedDamage, addMultiplerDamager)
	{
		damageType = Character.EDamageType.Trap;
	}


	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		// Evaluate unmitigated damage
		float unmitigatedDamage = (source.Stats.Attack * addMultiplerDamage) + addFixedDamage;

		// Work out final damage
		float finalDamage = Mathf.Max(Mathf.RoundToInt((float)source.Stats.Level * ((float)(source.Stats.Level * unmitigatedDamage)) / (float)(target.Stats.Level * target.Stats.PhysicalDefense)), 1);


		result.damageType = Character.EDamageType.Trap;
		result.criticalHit = false;
		result.dodged = false;
		result.finalDamage = (int)finalDamage;

		return null;
	}
}

public class StatusEffectCombatProperty : CombatProperty
{
	public StatusEffect statusEffect;

	public StatusEffectCombatProperty(StatusEffect effect)
	{
		statusEffect = effect;
	}

	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		target.ApplyStatusEffect(statusEffect);

		return result;
	}
}

public class KnockbackCombatProperty : CombatProperty
{
	public Vector3 direction;
	public float magnitude;

	public KnockbackCombatProperty(Vector3 direction, float magnitude)
	{
		this.direction = direction;
		this.magnitude = magnitude;
	}


	public override DamageResult Evaluate(DamageResult result, Character target, Character source)
	{
		if (result.target.IsVulnerableTo(EStatus.Knock))
		{
			result.knockbackDirection = new Vector3(direction.x, 0.0f, direction.z);
			result.knockbackMagnitute = magnitude;
		}

		return result;
	}
}

public static class CombatCalculator  
{
	public static DamageResult PhysicalDamageFormulaA(float addFixedDamage, float addMultiplier, Character source, Character target)
	{
		DamageResult result = new DamageResult(source, target);

		// Check for Dodge
		bool dodged = false;
		float maxDodgeChance = 100.0f;
		float dodgeChance = Mathf.Min(source.Stats.DodgeChance, maxDodgeChance);

		if (Random.Range(0.0f, maxDodgeChance) <= dodgeChance)
		{
			// Dodged success
			dodged = true;
		}

		float finalDamage = 0.0f;
		bool criticalStrike = false;

		if (!dodged)
		{
			// Check for critical strike
			float maxCritChance = 100.0f;
			float critChance = Mathf.Min(source.Stats.CriticalHitChance, maxCritChance);

			if (Random.Range(0.0f, maxCritChance) <= critChance)
			{
				// Dodged success
				criticalStrike = true;
			}

			// Evaluate unmitigated damage
			float unmitigatedDamage = (source.Stats.Attack * addMultiplier) + addFixedDamage;

			// Add crit damage if there was a crit!
			unmitigatedDamage = (criticalStrike ? unmitigatedDamage * source.Stats.CritalHitMultiplier : unmitigatedDamage);
			
			// Work out final damage
			finalDamage = Mathf.Max(Mathf.RoundToInt((float)source.Stats.Level * ((float)(source.Stats.Level * unmitigatedDamage)) / (float)(target.Stats.Level * target.Stats.PhysicalDefense)), 1);

		}

		result.damageType = Character.EDamageType.Physical;
		result.source = source;
		result.target = target;
		result.criticalHit = criticalStrike;
		result.dodged = dodged;
		result.finalDamage = (int)finalDamage;

		return result;
	}
}
