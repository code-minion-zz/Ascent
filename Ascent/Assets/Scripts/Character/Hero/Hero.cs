using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
	public struct HeroClassStatModifier
	{
		public float PowerAttack;
		public float FinesseCritChance;
		public float FinesseCritBonus;
		public float FinesseDodge;
		public float FinesseBlock;
		public float VitalityHP;
		public float VitalityPhysRes;
		public float VitalityHPRegen;
		public float SpiritSP;
		public float SpiritMagRes;


		HeroClassStatModifier(float _powerAttack, float _finesseCritChance,
							  float _finesseCritBonus, float _finesseDodge,
							  float _finesseBlock, float _vitalityHP,
							  float _vitalityPhysRes, float _vitalityHpRegen,
							  float _spiritSP, float _spiritMagRes)
		{
			PowerAttack = _powerAttack;
			FinesseCritChance = _finesseCritChance;
			FinesseCritBonus = _finesseCritBonus;
			FinesseDodge = _finesseDodge;
			FinesseBlock = _finesseBlock;
			VitalityHP = _vitalityHP;
			VitalityPhysRes = _vitalityPhysRes;
			VitalityHPRegen = _vitalityHpRegen;
			SpiritSP = _spiritSP;
			SpiritMagRes = _spiritMagRes;
		}
	}

	protected ulong saveUID;
	public ulong SaveUID
	{
		get { return saveUID;  }
		set { saveUID = value;}
	}

	protected EHeroClass classType;
	protected HeroAnimator heroAnimator;
	protected HeroClassStatModifier classStatMod;
    protected HeroController heroController;
	protected Backpack backpack;
    protected HeroInventory heroInventory;
    protected FloorStats floorStatistics;
	protected uint highestFloorReached;

	public uint HighestFloorReached
	{
		get { return highestFloorReached; }
		set { highestFloorReached = value; }
	}

	public EHeroClass ClassType
	{
		get { return classType; }
	}

	public HeroClassStatModifier ClassStatMod
	{
		get { return classStatMod; }
	}

	public Backpack HeroBackpack
	{
		get { return backpack; }
	}

	public HeroInventory HeroInventory
	{
		get { return heroInventory; }
	}

	public HeroController HeroController
	{
		get { return heroController; }
	}

    public FloorStats FloorStatistics
    {
        get { return floorStatistics; }
    }

	public virtual void Initialise(InputDevice input, HeroSaveData saveData)
	{
        base.Initialise();

	}

    public override void SetColor(Color color)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renderers)
        {
            render.material.color = color;
        }
    }

    public void ResetFloorStatistics()
    {
        floorStatistics = new FloorStats();
    }

    protected override void Respawn(Vector3 position)
    {
        // Reset the health
        derivedStats.ResetHealth();
        motor.canMove = true;
        Animator.Dying = false;
        collider.enabled = true;

        base.Respawn(position);
    }

    protected override void OnDeath()
    {
        motor.StopMotion();
        motor.canMove = false;
        collider.enabled = false;
        Animator.Dying = true;

        base.OnDeath();
    }

    public override void RefreshEverything()
    {
		if(!Game.Singleton.InTower)
		{
			transform.position = Vector3.zero;
		}

        // Resets hp and sp
        base.RefreshEverything();

        // Reset cooldowns
        foreach(Action a in abilities)
        {
            a.RefreshCooldown();
        }
    }

	public override void ApplyDamage(int unmitigatedDamage, Character.EDamageType type, Character owner)
	{
        base.ApplyDamage(unmitigatedDamage, type, owner);

		if (heroController.GrabbingObject)
		{
			heroController.ReleaseGrabbedObject();
			GetComponent<CharacterMotor>().StopMovingAlongGrid();
		}

        if (type == Character.EDamageType.Trap)
        {
            floorStatistics.NumberOfTrapsTripped++;
        }
	}

    protected override void OnDamageTaken(int damage)
    {
        base.OnDamageTaken(damage);

        // Record damage taken.
        floorStatistics.DamageTaken += damage;
        // Hero takes hit.
        animator.TakeHit = true;
    }

    protected override void OnDamageDealt(int damage)
    {
        base.OnDamageDealt(damage);

        // Record damage dealt.
        FloorStatistics.TotalDamageDealt += damage;
    }

	// moved to Backpack Window script
//	public bool EquipItem(int backpackSlot, int inventorySlot)
//	{
//		if (backpackSlot > 0 && backpackSlot < 8)
//		{
//			if (backpackSlot < 4) // this is an accessory
//			{
//				if (HeroBackpack.AllItems.Count < backpackSlot)
//				{
//					return false;
//				}
//				if (HeroBackpack.AllItems[backpackSlot] == null)
//				{
//				//	HeroBackpack.ReplaceItem
//				}
//			}
//			else // this is a consumable
//			{
//
//			}
//			return true;
//		}
//	}
}
