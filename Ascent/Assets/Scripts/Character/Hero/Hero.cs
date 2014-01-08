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

//		HeroClassStatModifier(float _powerAttack, float _finesseCritChance, 
//		                      float _finesseCritBonus, float _finesseDodge, 
//		                      float _finesseBlock, float _vitalityHP, 
//		                      float _vitalityPhysRes, float _vitalityHpRegen, 
//		                      float _spiritSP, float _spiritMagRes)
//		{
//
//		}
	}

	protected HeroClassStatModifier classStatMod;

    protected HeroController heroController;
	protected Backpack backpack;

	public HeroClassStatModifier ClassStatMod
	{
		get { return classStatMod; }
	}

	public Backpack HeroBackpack
	{
		get { return backpack; }
	}

	protected HeroInventory heroInventory;
	public HeroInventory HeroInventory
	{
		get { return heroInventory; }
	}

	public HeroController HeroController
	{
		get { return heroController; }
	}

	public virtual void Initialise(InputDevice input, HeroSaveData saveData)
	{
		heroInventory = new HeroInventory();

		base.Initialise();
	}

    public void SetColor(Color color)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renderers)
        {
            render.material.color = color;
        }
    }

    public override void Respawn(Vector3 position)
    {
        base.Respawn(position);

        // Reset the health
        derivedStats.ResetHealth();
    }

    /// <summary>
    /// Specific on death event functionality for heroes
    /// </summary>
    public override void OnDeath()
    {
        base.OnDeath();
    }

    /// <summary>
    /// Tells the hero to open the specified chest
    /// </summary>
    /// <param name="chest">The chest which needs to be opened</param>
    public void OpenChest(TreasureChest chest)
    {

    }

	//void OnTriggerEnter(Collider collision)
	//{
	//    if (collision.transform.tag == "Monster")
	//    {

	//    }
	//}

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
		//if (collision.transform.tag == "Door")
		//{
		//    Debug.Log("Open Door");
		//    Door door = collision.transform.GetComponent<Door>();
		//    //door.IsOpen = true;
		//}
		//if (collision.transform.tag == "Loot")
		//{
		//    CoinSack coins = collision.transform.GetComponent<CoinSack>();
		//    coins.transform.gameObject.SetActive(false);
		//}
    }

    public override void RefreshEverything()
    {
        // Resets hp and sp
        base.RefreshEverything();

        // Reset cooldowns
        foreach(Action a in abilities)
        {
            a.RefreshCooldown();
        }
    }

	public override void ApplyDamage(int unmitigatedDamage, Character.EDamageType type)
	{
		if (heroController.GrabbingObject)
		{
			heroController.ReleaseGrabbedObject();
			GetComponent<CharacterMotor>().StopMovingAlongGrid();
		}

		base.ApplyDamage(unmitigatedDamage, type);
	}
}
