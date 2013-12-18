using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour {

	Hero owner;
	public StatBar hpBar;
	public StatBar spBar;

	private const int maxAbilities = 4;
    public UILabel[] abilityLabels = new UILabel[maxAbilities];

	private const int maxItems = 3;
	public UILabel[] itemLabels = new UILabel[maxItems];

	private const int maxBuffs = 3;
	public UILabel[] buffLabels = new UILabel[maxBuffs];

	private const int maxDebuffs = 3;
	public UILabel[] debuffLabels = new UILabel[maxDebuffs];

	public UILabel livesLabel;

	public void Init(Hero _owner)
	{
		owner = _owner;
		hpBar.Init(StatBar.eStat.HP,owner);
		spBar.Init(StatBar.eStat.SP,owner);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
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
				abilityLabels[i].text = abilities[abilityID].Name + ". CD: " + abilities[abilityID].RemainingCooldown;

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

		// Do Items
		//ConsumableItem[] consumables = owner.HeroBackpack.ConsumableItems;

		for (int i = 0; i < itemLabels.Length; ++i )
		{
			if (itemLabels[i] != null)
			{
				itemLabels[i].text = "NoItem" + ". Qty: " + "0" + ". CD: " + "0";
			}
			else
			{
				itemLabels[i].text = "NoItem";
			}
		}

		// Do lives
		livesLabel.text = "Lives: " + owner.DerivedStats.Lives;


		// Do Buffs
		BetterList<Buff> buffs = owner.BuffList;

 
        for (int i = 0; i < 3; ++i)
        {
            if (i < buffs.size && buffs[i] != null)
            { 
                buffLabels[i].text = buffs[i].Name + ": " + buffs[i].Duration;
            }
            else
            {
                buffLabels[i].text = "";
            }
        }

        for (int i = 0; i < debuffLabels.Length; ++i)
         {
             debuffLabels[i].text = "";
         }
	}
}
