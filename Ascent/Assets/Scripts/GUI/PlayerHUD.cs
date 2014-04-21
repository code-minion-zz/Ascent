using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerHUD : MonoBehaviour 
{
    public UILabel playerLabel;
	public StatBar hpBar;
	public StatBar spBar;
	public UISprite[] abilityIcons = new UISprite[maxAbilities];
	public UISprite[] itemIcons = new UISprite[maxItems];
	public UISprite[] accessoryIcons = new UISprite[maxAccessories];
	public UILabel[] itemQuantityLabels = new UILabel[maxItems];
    public GameObject statusEffectPrefab;
    public UIGrid statusEffectGrid;
	public UILabel livesLabel;

    private Hero owner;
	private const int maxAbilities = 4;
	private const int maxAccessories = 4;
	private const int maxItems = 3;
    private const int maxStatusEffects = 20;
    private StatusEffectHUDIcon[] statusEffectIcons = new StatusEffectHUDIcon[maxStatusEffects];

	public UISprite[] lowHealthIndicators = new UISprite[2];
	private float time;
	

	public void Initialise(Hero _owner)
	{
		owner = _owner;
		hpBar.Init(StatBar.eStat.HP,owner);
		spBar.Init(StatBar.eStat.SP,owner);

        // Create a pool of blank status effect icons.
        for (int i = 0; i < maxStatusEffects; ++i)
        {
            GameObject statusEffectIcon = NGUITools.AddChild(statusEffectGrid.gameObject, statusEffectPrefab);
            statusEffectIcons[i] = statusEffectIcon.GetComponent<StatusEffectHUDIcon>();
            statusEffectIcons[i].gameObject.SetActive(false);
        }
        statusEffectGrid.Reposition();

		// Set the ability icons
        Ability[] abilities = owner.Loadout.AbilityBinds;

		int abilityID = 0;
		for (int i = 0; i < abilityIcons.Length; ++i)
		{
			++abilityID;

			if (abilities[i] != null)
			{
				abilityIcons[i].spriteName = "Ability_" + abilities[abilityID].GetType().ToString();
				//Debug.Log(abilityIcons[i].spriteName);
			}
		}

		// Set the item icons
		ConsumableItem[] consumables = owner.Backpack.ConsumableItems;

		for (int i = 0; i < itemIcons.Length; ++i)
		{
			if (consumables[i] != null)
			{
				itemIcons[i].spriteName = "Consumable_" + consumables[i].GetType().ToString();
				//Debug.Log(itemIcons[i].spriteName);

				itemQuantityLabels[i].text = consumables[i].Charges.ToString();
			}
		}

		// Set broken accessories
		//AccessoryItem[] accessories = owner.Backpack.AccessoryItems;

		for (int i = 0; i < accessoryIcons.Length; ++i)
		{
			accessoryIcons[i].gameObject.SetActive(false);
		}

		ProcessBrokenAccessories();
	}

	void Update ()
	{
		// Do Abilities
		Ability[] abilities = owner.Loadout.AbilityBinds;

		int abilityID = 0;
		for (int i = 0; i < abilityIcons.Length; ++i)
		{
			++abilityID;

			if (abilities[i] != null)
			{
				Color color = abilityIcons[i].color;
				color.a = 1.0f - (abilities[abilityID].RemainingCooldown / abilities[abilityID].CooldownFullDuration);
				abilityIcons[i].color = color;
			}
			else
			{
				abilityIcons[i].gameObject.SetActive(false);
			}
		}

        //// Do Items
        //ConsumableItem[] consumables = owner.Backpack.ConsumableItems;

        //for (int i = 0; i < itemIcons.Length; ++i )
        //{
        //    if (consumables[i] != null)
        //    {
        //        Color color = itemIcons[i].color;
        //        color.a = 1.0f - (consumables[i].Cooldown / consumables[i].CooldownMax);
        //        itemIcons[i].color = color;

        //        itemQuantityLabels[i].text = consumables[i].Charges.ToString();
        //    }
        //    else
        //    {
        //        itemIcons[i].gameObject.SetActive(false);
        //    }
        //}

		// Health indicators
		float healthRatio = ((float)owner.Stats.CurrentHealth / (float)owner.Stats.MaxHealth);
		if (healthRatio < 0.50f && healthRatio > 0.0f)
		{
			foreach (UISprite sprite in lowHealthIndicators)
			{
				Color color = sprite.color;
				//color.a = 1.0f - ((1.0f - healthRatio) * 0.75f);
				time += Time.deltaTime * 0.25f;
				color.a = 0.25f + Mathf.PingPong(time, 1.0f - ((1.0f - healthRatio) * 0.75f) + 0.25f);
				sprite.color = color;
			}
		}
		else
		{
			foreach (UISprite sprite in lowHealthIndicators)
			{
				Color color = sprite.color;
				if (color.a != 1.0f)
				{
					color.a -= Time.deltaTime;
					sprite.color = color;
				}
			}
		}

		// Do lives
		livesLabel.text = (owner.Lives).ToString();


		// Do Buffs
        //List<StatusEffect> statusEffects = owner.StatusEffects;

        //int statusEffectIconSize = statusEffectIcons.Length;
        //for (int i = 0; i < statusEffectIconSize; ++i)
        //{
        //    // If there is a status effect that can go into this slot then put it in
        //    if (i < statusEffects.Count)
        //    {
        //        statusEffectIcons[i].gameObject.SetActive(true);
        //        statusEffectIcons[i].Initialise(statusEffects[statusEffects.Count - (i + 1)]);
        //    }
        //    else // Deactivate the icon so it is not renderered or sorted.
        //    {
        //        statusEffectIcons[i].gameObject.SetActive(false);
        //    }
        //}
        //statusEffectGrid.Reposition();


		// Set broken accessories
		//ProcessBrokenAccessories();
	}

	private void ProcessBrokenAccessories()
	{
		AccessoryItem[] accessories = owner.Backpack.AccessoryItems;

		int accessIconSlot = 0;
		for (int i = 0; i < accessories.Length; ++i)
		{
			if (accessories[i] != null)
			{
				float durabilityRatio = ((float)accessories[i].Durability / (float)accessories[i].DurabilityMax);
				if (durabilityRatio <= 0.45f)
				{
					//Debug.Log(durabilityRatio);
					accessoryIcons[accessIconSlot].gameObject.SetActive(true);
					accessoryIcons[accessIconSlot].spriteName = ("accessory_" + accessories[i].Type + "_" + accessories[i].GradeEnum.ToString()).ToLower();
					//Debug.Log(accessoryIcons[i].spriteName);

					accessoryIcons[accessIconSlot].color = new Color(1.0f - ((1.0f - durabilityRatio) * 0.5f), 1.0f - (1.0f - durabilityRatio), 1.0f - (1.0f - durabilityRatio), 0.75f);

					++accessIconSlot;
				}
			}
		}
	}
}
