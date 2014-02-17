﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LootGenerator  
{
	// TODO: Allow this generator to take weight values to change how the randoms work

	public enum ELootType
	{
		Accessory = 0,
		Consumable,
		Any,
	}

	public enum EAccessoryTemplate
	{
		Power = 0,
		Finesse,
		Vitaliy,
		Spirit,
		PowerFinesse,
		PowerVitality,
		PowerSpirit,
		FinesseVitality,
		FinesseSpirit,
		VitalitySpirit,
		All,
		Max
	}

	public static Item RandomlyGenerateItem(int floorNum, ELootType type)
	{
		// Randomly select the type of item
		if (type == ELootType.Any)
		{
			type = (ELootType)Random.Range(0, 1);
		}

		Item brandSpankingNewItem = null;
		
		// Initialise based on type
		switch (type)
		{
			case ELootType.Accessory:
				{
					brandSpankingNewItem = RandomlyGenerateAccessory(floorNum);
				}
				break;
			case ELootType.Consumable:
				{
					brandSpankingNewItem = RandomlyGenerateConsumable(floorNum);
				}
				break;
		}

		return brandSpankingNewItem;
	}

	public static Item RandomlyGenerateAccessory(int floorNum)
	{
		Item.ItemGrade grade = RandomGrade();

		Item newItem = new AccessoryItem();
		AccessoryItem newAccItem = newItem as AccessoryItem;
		newAccItem.PrimaryStats = RandomAccessoryStats(floorNum);
		newAccItem.ItemStats = new ItemStats();
		newAccItem.ItemStats.Level = Mathf.Max(1, Random.Range(floorNum - 1, floorNum + 1));
		newAccItem.ItemStats.Name = RandomAccessoryName();
		newAccItem.ItemStats.Description = RandomAccessoryDescription();
		newAccItem.ItemStats.Grade = (int)grade;
        newAccItem.ItemProperties.Add(new AttackItemProperty());

		RandomAccessoryProperties((AccessoryItem)newItem);

		return newItem;
	}

	public static Item RandomlyGenerateConsumable(int floorNum)
	{
		Item.ItemGrade grade = RandomGrade();

		ConsumableItem.EConsumableType consumableType = (ConsumableItem.EConsumableType)(Random.Range((int)ConsumableItem.EConsumableType.INVALID + 1, (int)ConsumableItem.EConsumableType.MAX));
		ConsumableItem newItem = null;
        ItemStats stats = new ItemStats();
        stats.Level = floorNum;
        stats.Grade = (int)grade;
        stats.Name = RandomAccessoryName();
        stats.Description = RandomAccessoryDescription();

		// Create the consumable item
		// TODO: Randomly generate quanity and power of the consumable (Not all consumables benefit from power)

		switch (consumableType)
		{
			case ConsumableItem.EConsumableType.Health:
				{
					newItem = new HealthPotionItem();
                    stats.Name = "HPPot";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				} 
				break;
			case ConsumableItem.EConsumableType.Special:
				{
					newItem = new SpecialPotionItem();
                    stats.Name = "SPPot";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				}
				break;
			case ConsumableItem.EConsumableType.Bomb:
				{
					newItem = new BombItem();
                    stats.Name = "Bomb";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				}
				break;
			case ConsumableItem.EConsumableType.Key:
				{
					newItem = new KeyItem();
                    stats.Name = "Key";
                    newItem.Charges = 5;
				}
				break;
			default:
				{
					Debug.LogError("Unhandled Case");
				}
				break;
		}

        if (newItem != null)
        {
            newItem.ItemStats = stats;
        }

		return (Item)newItem;
	}

	private static Item.ItemGrade RandomGrade()
	{
		return (Item.ItemGrade)Random.Range((int)Item.ItemGrade.E, (int)Item.ItemGrade.S);
	}

	private static void RandomAccessoryProperties(AccessoryItem accessoryItem)
	{
        // Randomly choose properties based on grade.
        int numberOfProperties = Random.Range(0, accessoryItem.Grade);

        for (int i = 0; i < numberOfProperties; ++i)
        {
            ItemProperty newProperty = null;
            bool secondaryStatProperty = false;
            
            // Randomly choose a new property
            ItemProperty.EType propertyType = (ItemProperty.EType)Random.Range((int)ItemProperty.EType.None + 1, (int)ItemProperty.EType.Max);

            switch (propertyType)
            {
                // Secondary stats are all the same.
                case ItemProperty.EType.Attack:      newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.Accuracy:    newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.CriticalHit: newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.Dodge:       newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.MDefence:    newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.PDefence:   newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;
                case ItemProperty.EType.Special:    newProperty = new AttackItemProperty(); secondaryStatProperty = true; break;

                case ItemProperty.EType.Experience:
                    {
                        newProperty = new ExperienceItemProperty();

                        float buffValue = Random.Range(0.1f, 5.0f);

                        ((ExperienceItemProperty)newProperty).BuffValue = buffValue;
                        ((ExperienceItemProperty)newProperty).BuffType = StatusEffect.EApplyMethod.Percentange;
                    }
                    break;
                case ItemProperty.EType.Gold:
                    {
                        newProperty = new GoldItemProperty();

                        float buffValue = Random.Range(0.1f, 5.0f);

                        ((GoldItemProperty)newProperty).BuffValue = buffValue;
                        ((GoldItemProperty)newProperty).BuffType = StatusEffect.EApplyMethod.Percentange;
                    }
                    break;

                case ItemProperty.EType.Max: // Fall
                case ItemProperty.EType.None: // Fall
                default:
                    {
                        Debug.LogError("Unhandled case.");
                    }
                    break;
            }

            // If it is a secondary stat apply the buff value and type
            if (secondaryStatProperty)
            {
                // Randomly select the apply method.
                StatusEffect.EApplyMethod applyMethod = (StatusEffect.EApplyMethod)Random.Range(0, 1);

                float buffValue = 1.0f;

                // Based on the level, randomly choose values for the stats.
                switch (applyMethod)
                {
                    case StatusEffect.EApplyMethod.Percentange:
                        {
                             buffValue = Random.Range(0.1f, 5.0f);
                        }
                        break;
                    case StatusEffect.EApplyMethod.Fixed:
                        {
                            buffValue = (float)Random.Range(1, 5);
                        }
                        break;
                    default:
                        {
                            Debug.LogError("Unhandled case.");
                        }
                        break;
                }

                ((SecondaryStatItemProperty)newProperty).BuffValue = buffValue;
                ((SecondaryStatItemProperty)newProperty).BuffType = applyMethod;
            }

            accessoryItem.ItemProperties.Add(newProperty);
        }
	}

	private static string RandomAccessoryName()
	{
		return "RndName";
	}

	private static string RandomAccessoryDescription()
	{
		return "RndDesc";
	}

	private static PrimaryStats RandomAccessoryStats(int level)
	{
		// Random between different templates
		// The level impacts number of stats that can be allocated

		int points = RandomAcessoryStatPoints(level);

		PrimaryStats stats = new PrimaryStats();

		EAccessoryTemplate template = (EAccessoryTemplate)Random.Range(0, (int)EAccessoryTemplate.Max);

		switch (template)
		{
			case EAccessoryTemplate.Power:
				{
					stats.power = points;
				}
				break;
			case EAccessoryTemplate.Finesse:
				{
					stats.finesse = points;
				}
				break;
			case EAccessoryTemplate.Vitaliy:
				{
					stats.vitality = points;
				}
				break;
			case EAccessoryTemplate.Spirit:
				{
					stats.spirit = points;
				}
				break;
			case EAccessoryTemplate.PowerFinesse:
				{
					stats.power = points / 2;
					stats.finesse = points / 2;
				}
				break;
			case EAccessoryTemplate.PowerVitality:
				{
					stats.power = points / 2;
					stats.vitality = points / 2;
				}
				break;
			case EAccessoryTemplate.PowerSpirit:
				{
					stats.power = points / 2;
					stats.spirit = points / 2;
				}
				break;
			case EAccessoryTemplate.FinesseVitality:
				{
					stats.finesse = points / 2;
					stats.vitality = points / 2;
				}
				break;
			case EAccessoryTemplate.FinesseSpirit:
				{
					stats.finesse = points / 2;
					stats.spirit = points / 2;
				}
				break;
			case EAccessoryTemplate.VitalitySpirit:
				{
					stats.vitality = points / 2;
					stats.spirit = points / 2;
				}
				break;
			case EAccessoryTemplate.All:
				{
					stats.power = points / 4;
					stats.finesse = points / 4;
					stats.vitality = points / 4;
					stats.spirit = points / 4;
				}
				break;
			case EAccessoryTemplate.Max: // Fall
			default:
				{
					Debug.LogError("Unhandled case.");
				}
				break;
		}

		return stats;
	}

	private static int RandomAcessoryStatPoints(int level)
	{
		int minPoints = 1;
		int maxPoints = 30;

		int points = (int)(minPoints + ((float)maxPoints - (float)minPoints) * (float)(((float)level / (float)StatGrowth.KMaxLevel)));
		int variance = (int)Mathf.Max(((float)points * 0.15f), 1.0f);
		points +=  (int)Mathf.Max(Random.Range(-variance, variance), 2.0f);

		return points;
	}

}
