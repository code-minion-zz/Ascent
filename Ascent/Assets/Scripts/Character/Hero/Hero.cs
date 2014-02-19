using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Hero : Character 
{
	protected ulong saveUID;
	public ulong SaveUID
	{
		get { return saveUID;  }
		set { saveUID = value;}
	}

	protected int lives;
	public int Lives
	{
		get { return lives; }
		set { lives = value; }
	}

	protected EHeroClass heroClass;
	protected HeroAnimator heroAnimator;
    protected HeroController heroController;
	protected Backpack backpack;
    protected HeroInventory inventory;
    protected FloorStats floorStatistics;
    protected AbilityTree abilityTree;
	protected uint highestFloorReached;
    public int unasignedAbilityPoints;

	public uint HighestFloorReached
	{
		get { return highestFloorReached; }
		set { highestFloorReached = value; }
	}

	protected HeroStats heroStats;
	public HeroStats HeroStats
	{
		get { return heroStats; }
		set
		{
			heroStats = value;
			stats = value;
		}
	}

	public EHeroClass HeroClass
	{
		get { return heroClass; }
	}

	public Backpack Backpack
	{
		get { return backpack; }
	}

	public HeroInventory HeroInventory
	{
		get { return inventory; }
	}

    public HeroAbilityLoadout HeroLoadout
    {
        get { return (HeroAbilityLoadout)loadout; }
    }

	public HeroController HeroController
	{
		get { return heroController; }
	}

    public FloorStats FloorStatistics
    {
        get { return floorStatistics; }
        set { floorStatistics = value; }
    }

    public AbilityTree AbilityTree
    {
        get { return abilityTree; }
        set { abilityTree = value; }
    }

	public virtual void Initialise(InputDevice input, HeroSaveData saveData)
	{
		animator = GetComponentInChildren<HeroAnimator>();
		if (animator == null)
		{
			Debug.LogError("No animator attached to " + name, this);
		}
		animator.Initialise();

		// Init base with things like shadow, tilt and motor
        base.Initialise();

		// Attempt to load character else create a new one.
		if(saveData != null)
		{
			Load(this, saveData);
		}
		else
		{
			Create(this);
		}

		// Initialise Controller, hook it up with the hero, hero animator and motor
		heroController = gameObject.GetComponent<HeroController>();
		heroController.Initialise(this, input, (HeroAnimator)animator, motor, HeroLoadout);
	}

	public void OnEnable()
	{
		RefreshEverything();
	}

	public static void Create(Hero hero)
	{
		// Create items
		hero.backpack = new Backpack();
		hero.inventory = new HeroInventory();

		// Create stats
		hero.HeroStats = new HeroStats(hero);
		hero.HeroStats.Reset();

        // Create abilities
        hero.loadout = new HeroAbilityLoadout();
        hero.loadout.Initialise(hero);

		Test_PopulateInventoryAndBackpack(hero);
		Test_DrawHeroStats(hero);
	}

    public static void Load(Hero hero, HeroSaveData data)
    {
        // Load in Items
        hero.inventory = data.inventory;
        hero.backpack = data.backpack;

        // Load in stats
        hero.HeroStats = new HeroStats(hero, data);
        hero.HeroStats.Reset();

        // Load in abilities
        hero.loadout = data.loadout;
        hero.loadout.Initialise(hero);

        Test_DrawHeroStats(hero);
    }

    public override void Update()
    {
        base.Update();

        // Update cooldown on items
        backpack.Process();
    }

    public void AddExperience(int experience)
    {
        // Add experience
        int curExp = heroStats.Experience + experience;
		int maxExp = heroStats.RequiredExperience;

        // Keep leveling while experience is above required.
        while (curExp >= maxExp)
        {
            curExp -= maxExp;

            LevelUp();

            // Recalculate required experience
			maxExp = heroStats.RequiredExperience;
        }

        // Set the new experience value
		heroStats.Experience = curExp;
    }

    public void LevelUp()
    {
		stats.Level += 1;
        unasignedAbilityPoints += 1;
    }

    public override void SetColor(Color color)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer render in renderers)
        {
            render.material.color = color;
        }
    }

    protected override void Respawn(Vector3 position)
    {
        // Reset the health
		RefreshEverything();
        motor.IsHaltingMovementToPerformAction = true;
        Animator.Dying = false;
        collider.enabled = true;

        base.Respawn(position);
    }

    protected override void OnDeath()
    {
        motor.StopMotion();
        motor.IsHaltingMovementToPerformAction = false;
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
        if (loadout != null)
        {
            loadout.Refresh();
        }
    }

	public override void ApplyDamage(int unmitigatedDamage, bool crit, Character.EDamageType type, Character source)
	{
        base.ApplyDamage(unmitigatedDamage, crit, type, source);

        // Apply durability Loss
        AccessoryItem[] accessories = backpack.AccessoryItems;
        foreach (AccessoryItem acc in accessories)
        {
            acc.ApplyDurabilityDamage(unmitigatedDamage, crit, this, source);
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
    }

    protected override void OnDamageDealt(int damage)
    {
        base.OnDamageDealt(damage);

        // Record damage dealt.
        FloorStatistics.TotalDamageDealt += damage;
    }

	public void Equip(int destinationSlot, Item toEquip)
	{
		if (!ValidSlot(destinationSlot)) return;

		if (toEquip != null)
		{
			Item returnItem = Backpack.ReplaceItem(destinationSlot, toEquip);
			HeroInventory.Items.Remove(toEquip);
			HeroInventory.AddItem(returnItem);
		}
	}

	bool ValidSlot(int slot)
	{
		if (slot >= 0 && slot <= 6)
		{
			return true;
		}
		return false;
	}

    public static void Test_PopulateInventoryAndBackpack(Hero hero)
    {
        Backpack backpack = hero.backpack;
        backpack.AddItem(Backpack.BackpackSlot.ACC1, LootGenerator.RandomlyGenerateAccessory(1, true));
        backpack.AddItem(Backpack.BackpackSlot.ACC2, LootGenerator.RandomlyGenerateAccessory(2, true));
        backpack.AddItem(Backpack.BackpackSlot.ACC3, LootGenerator.RandomlyGenerateAccessory(3, true));
        backpack.AddItem(Backpack.BackpackSlot.ACC4, LootGenerator.RandomlyGenerateAccessory(4, true));
        backpack.AddItem(Backpack.BackpackSlot.ITM1, LootGenerator.RandomlyGenerateConsumable(1, true));
        backpack.AddItem(Backpack.BackpackSlot.ITM2, LootGenerator.RandomlyGenerateConsumable(2, true));
        backpack.AddItem(Backpack.BackpackSlot.ITM3, LootGenerator.RandomlyGenerateConsumable(3, true));

        HeroInventory inventory = hero.inventory;
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(5, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(6, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(7, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(5, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(6, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(7, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(5, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(6, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateAccessory(7, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateConsumable(4, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateConsumable(5, false));
        inventory.AddItem(LootGenerator.RandomlyGenerateConsumable(6, false));
    }

    public static void Test_DrawHeroStats(Hero hero)
    {
        Debug.Log("Derived: " +
            "POW: " + hero.HeroStats.Power +
            " FIN: " + hero.HeroStats.Finesse +
            " VIT: " + hero.HeroStats.Vitality +
            " SPR: " + hero.HeroStats.Spirit + " | " +

            " ATK: " + hero.HeroStats.Attack +
            " PDEF: " + hero.HeroStats.PhysicalDefense +
            " MDEF: " + hero.HeroStats.MagicalDefense +
            " CRIT: " + hero.HeroStats.CriticalHitChance +
            " MULT: " + hero.HeroStats.CritalHitMultiplier +
            " DODGE: " + hero.HeroStats.DodgeChance
            );
    }
}
