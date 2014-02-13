using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerHUD : MonoBehaviour 
{
    public UILabel livesLabel;
	public StatBar hpBar;
	public StatBar spBar;
    public UILabel[] abilityLabels = new UILabel[maxAbilities];
    public UILabel[] itemLabels = new UILabel[maxItems];
    public GameObject statusEffectPrefab;
    public UIGrid statusEffectGrid;

    private Hero owner;
	private const int maxAbilities = 4;
	private const int maxItems = 3;
    private const int maxStatusEffects = 20;
    private StatusEffectHUDIcon[] statusEffectIcons = new StatusEffectHUDIcon[maxStatusEffects];


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
	}

	void Update () 
    {
		// Do Abilities
        List<Action> abilities = owner.Abilities;

		int abilityID = 0;
		for (int i = 0; i < abilityLabels.Length; ++i )
		{
			++abilityID;

			if (abilities[abilityID] != null)
			{
				abilityLabels[i].text = abilities[abilityID].AnimationTrigger + ". CD: " + Math.Round( abilities[abilityID].RemainingCooldown, 2);

				if (abilities[abilityID].RemainingCooldown <= 0.0f)
				{
					abilityLabels[i].color = Color.green;
				}
				else
				{
					abilityLabels[i].color = Color.red;
				}
			}
			else
			{
				abilityLabels[i].text = "NoAbility";
			}
		}

        //// Do Items
        //ConsumableItem[] consumables = owner.Backpack.ConsumableItems;

        //for (int i = 0; i < itemLabels.Length; ++i )
        //{

        //    if (itemLabels[i] != null)
        //    {
        //        if (consumables[i] != null)
        //        {
        //            itemLabels[i].text = consumables[i].ItemStats.Name + " Qty: " + consumables[i].Charges + " CD: " +  consumables[i].Cooldown;
        //        }
        //    }
        //    else
        //    {
        //        itemLabels[i].text = "NoItem";
        //    }
        //}

		// Do lives
		livesLabel.text = "Lives: " + owner.Lives;


        // Do Buffs
        List<StatusEffect> statusEffects = owner.StatusEffects;

        int statusEffectIconSize = statusEffectIcons.Length;
        for (int i = 0; i < statusEffectIconSize; ++i)
        {
            // If there is a status effect that can go into this slot then put it in
            if (i < statusEffects.Count)
            {
                statusEffectIcons[i].gameObject.SetActive(true);
                statusEffectIcons[i].Initialise(statusEffects[statusEffects.Count - (i + 1)]);
            }
            else // Deactivate the icon so it is not renderered or sorted.
            {
                statusEffectIcons[i].gameObject.SetActive(false);
            }
        }
        statusEffectGrid.Reposition();
	}
}
