using UnityEngine;
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

	private enum EAccessoryTemplate
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

	private enum EAccessoryMaterial
	{
		// http://www.makermends.com/gemstones.html

		Ruby = EAccessoryTemplate.Power,
		Topaz = EAccessoryTemplate.Finesse,
		Emerald = EAccessoryTemplate.Vitaliy,
		Sapphire = EAccessoryTemplate.Spirit,

		Jet = EAccessoryTemplate.PowerFinesse,
		Bloodstone = EAccessoryTemplate.PowerVitality,
		Moonstone = EAccessoryTemplate.PowerSpirit,

		Jasper = EAccessoryTemplate.FinesseVitality,
		Turquiose = EAccessoryTemplate.FinesseSpirit,

		Lazuli = EAccessoryTemplate.VitalitySpirit,

		Opal = EAccessoryTemplate.All,

		Max
	}

	private static EAccessoryTemplate currentAccessoryTemplate;

	private const int maxCraftSynoynms = 5;
	static string[] craftSynoynms = new string[maxCraftSynoynms] 
	{
		"Crafted",
		"Forged",
		"Constructed",
		"Produced",
		"Manufactured",
	};

	private const int maxLocationNames = 1;
	static string[] locationNames = new string[maxLocationNames] 
	{
		"The Land of Loofs",
	};

	private const int maxPersonTitle = 3;
	static string[] personNameTitles = new string[maxPersonTitle] 
	{
		"World Famous ",
		"Locally Produced ",
		"Home Grown "
	};

	private const int maxPersonNames = 3;
	static string[] personNames = new string[maxPersonNames] 
	{
		"Kitler",
		"Kittens",
		"Kittles",
	};

	private const int maxPersonSuffix = 2;
	static string[] personNameSuffixes = new string[maxPersonSuffix] 
	{
		" the Loof",
		" the Calculator"
	};

	private const int maxRace = 1;
	static string[] raceNames = new string[maxRace] 
	{
	    "Loofen",
	};

	private const int maxStoreNames = 1;
	static string[] storeNames = new string[maxStoreNames] 
	{
	    "Kitler's Kool Kilo-Jools",
	};

	private const int maxBrandNames = 1;
	static string[] brandNames = new string[maxBrandNames] 
	{
	    "Kitch",
	};

	private const int maxGradeE_Adjectives = 1;
	static string[] gradeEAdjectives = new string[maxBrandNames] 
	{
	    "Damaged",
	};

	private enum EMaxGradeAjective
	{
		E = 1,
		D = 1,
		C = 1,
		B = 1,
		A = 1,
		S = 1,
	}

	static Dictionary<Item.EGrade, string[]> gradeQualitySynonyms = new Dictionary<Item.EGrade, string[]>()
	{
		{ Item.EGrade.E, new string[(int)EMaxGradeAjective.E]
			{
				   "Damaged",
			}
		},

		{ Item.EGrade.D, new string[(int)EMaxGradeAjective.D]
			{
				   "Flawed",
			}
		},

		{ Item.EGrade.C, new string[(int)EMaxGradeAjective.C]
			{
				   "Clean",
			}
		},

		{ Item.EGrade.B, new string[(int)EMaxGradeAjective.B]
			{
				   "Polished",
			}
		},

		{ Item.EGrade.A, new string[(int)EMaxGradeAjective.A]
			{
				   "Superb",
			}
		},

		{ Item.EGrade.S, new string[(int)EMaxGradeAjective.S]
			{
				   "Immaculate",
			}
		},
	};


	public static Item RandomlyGenerateItem(int floorNum, ELootType type, bool identified)
	{
		// Randomly select the type of item
		if (type == ELootType.Any)
		{
			type = (ELootType)Random.Range(0, 2);
		}

		Item brandSpankingNewItem = null;
		
		// Initialise based on type
		switch (type)
		{
			case ELootType.Accessory:
				{
					brandSpankingNewItem = RandomlyGenerateAccessory(floorNum, identified);
				}
				break;
			case ELootType.Consumable:
				{
					brandSpankingNewItem = RandomlyGenerateConsumable(floorNum, identified);
				}
				break;
		}

		return brandSpankingNewItem;
	}

	public static Item RandomlyGenerateAccessory(int floorNum, bool idendified)
	{
		Item.EGrade grade = RandomGrade();

		Item newItem = new AccessoryItem();

		currentAccessoryTemplate = (EAccessoryTemplate)Random.Range(0, (int)EAccessoryTemplate.Max);

		AccessoryItem newAccItem = newItem as AccessoryItem;
		newAccItem.PrimaryStats = RandomAccessoryStats(floorNum);
		newAccItem.ItemStats = new ItemStats();
		newAccItem.ItemStats.Level = Mathf.Max(1, Random.Range(floorNum - 1, floorNum + 1));
        newAccItem.ItemStats.Grade = (int)grade;
		newAccItem.DurabilityMax = RandomDurability(grade);
		newAccItem.Durability = newAccItem.DurabilityMax;
		((AccessoryItem)newAccItem).Type = RandomAccessoryType();
		

        RandomTitleAndDescription((AccessoryItem)newAccItem);
		RandomAccessoryProperties((AccessoryItem)newItem);

		return newItem;
	}

	public static Item RandomlyGenerateConsumable(int floorNum, bool identified)
	{
		Item.EGrade grade = RandomGrade();

		ConsumableItem.EConsumableType consumableType = (ConsumableItem.EConsumableType)(Random.Range((int)ConsumableItem.EConsumableType.INVALID + 1, (int)ConsumableItem.EConsumableType.MAX));
		ConsumableItem newItem = null;
        ItemStats stats = new ItemStats();
        stats.Level = floorNum;
        stats.Grade = (int)grade;
        stats.Name = "Consumables";
        stats.Description = "Description";

		// Create the consumable item
		// TODO: Randomly generate quanity and power of the consumable (Not all consumables benefit from power)

		switch (consumableType)
		{
			case ConsumableItem.EConsumableType.HealthPotion:
				{
					newItem = new HealthPotionItem();
                    stats.Name = "Health Potion";
					stats.Description = "Restore some Health.";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				} 
				break;
			case ConsumableItem.EConsumableType.SpecialPotion:
				{
					newItem = new SpecialPotionItem();
                    stats.Name = "Special Potion";
					stats.Description = "Restore some Special Power.";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				}
				break;
			case ConsumableItem.EConsumableType.Bomb:
				{
					newItem = new BombItem();
                    stats.Name = "Bomb";
					stats.Description = "Use em' to blow up hidden pathways or enemies!";
                    newItem.Charges = 5;
                    newItem.CooldownMax = 1.0f;
				}
				break;
			case ConsumableItem.EConsumableType.Key:
				{
					newItem = new KeyItem();
                    stats.Name = "Key";
					stats.Description = "Use to open locked doors!";
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
			newItem.Type = consumableType;
            newItem.ItemStats = stats;
        }

		return (Item)newItem;
	}

	private static AccessoryItem.EAccessoryType RandomAccessoryType()
	{
		return (AccessoryItem.EAccessoryType)Random.Range((int)AccessoryItem.EAccessoryType.None + 1, (int)AccessoryItem.EAccessoryType.Max);
	}

	private static Item.EGrade RandomGrade()
	{
		return (Item.EGrade)Random.Range((int)Item.EGrade.INVALID_GRADE + 1, (int)Item.EGrade.MAX_GRADE);
	}

    private static void RandomTitleAndDescription(AccessoryItem accessoryItem)
    {
        accessoryItem.ItemStats.Name = RandomAccessoryName(accessoryItem);

		if(Random.Range(0, 2) == 0)
		{
			accessoryItem.ItemStats.Description = RandomAccessoryCityThemedDescription(accessoryItem);
		}
		else
		{
			accessoryItem.ItemStats.Description = RandomAccessoryTowerThemedDescription(accessoryItem);
		}
    }

	private static int RandomDurability(Item.EGrade grade)
	{
		// https://docs.google.com/spreadsheet/ccc?key=0ApF1sRIB-wxQdHpVaEE0OGdRd0FYTlQwWngtTFpkeHc&usp=drive_web#gid=1

		int maxDuability = 0;

		switch (grade)
		{
			case Item.EGrade.E:
				{
					maxDuability = 30 + (Random.Range(0, 41) - 20);
				}
				break;
			case Item.EGrade.D:
				{
					maxDuability = 50 + (Random.Range(0, 31) - 15);
				}
				break;
			case Item.EGrade.C:
				{
					maxDuability = 60 + (Random.Range(0, 21) - 10);
				}
				break;
			case Item.EGrade.B:
				{
					maxDuability = 70 + (Random.Range(0, 11) - 5);
				}
				break;
			case Item.EGrade.A:
				{
					maxDuability = 80 + (Random.Range(0, 11) - 5);
				}
				break;
			case Item.EGrade.S:
				{
					maxDuability = 100;
				}
				break;
			case Item.EGrade.MAX_GRADE:	// Fall
			case Item.EGrade.INVALID_GRADE:	// Fall
			default:
				{
					Debug.LogError("Unhandled case.");		
				}
				break;
		}

		return maxDuability;
	}

	private static void RandomAccessoryProperties(AccessoryItem accessoryItem)
	{
        // Randomly choose properties based on grade.
        int numberOfProperties = Random.Range(0, accessoryItem.Grade + 1);

        for (int i = 0; i < numberOfProperties; ++i)
        {
            ItemProperty newProperty = null;
            bool secondaryStatProperty = false;
            
            // Randomly choose a new property
            ItemProperty.EType propertyType = (ItemProperty.EType)Random.Range((int)ItemProperty.EType.None + 1, (int)ItemProperty.EType.Max);

            switch (propertyType)
            {
                // Secondary stats are all the same.
				case ItemProperty.EType.Attack:
					{
						newProperty = new AttackItemProperty(); secondaryStatProperty = true;
					}
					break;
                case ItemProperty.EType.Accuracy:  
					{
						newProperty = new AccuracyItemProperty(); secondaryStatProperty = true; 
					}
					break;
                case ItemProperty.EType.CriticalHit:
					{
						newProperty = new CriticalItemProperty(); secondaryStatProperty = true;
					}
					break;
				case ItemProperty.EType.Dodge:
					{
						newProperty = new DodgeItemProperty(); secondaryStatProperty = true;
					}
					break;
				case ItemProperty.EType.MDefence:
					{
						newProperty = new MDefenceItemProperty(); secondaryStatProperty = true;
					}
					break;
				case ItemProperty.EType.PDefence:
					{
						newProperty = new PDefenceItemProperty(); secondaryStatProperty = true;
					}
					break;
				case ItemProperty.EType.Special:
					{
						newProperty = new SpecialItemProperty(); secondaryStatProperty = true;
					}
					break;
                case ItemProperty.EType.Experience:
                    {
                        newProperty = new ExperienceItemProperty();

						float buffValue = Random.Range(1.0f, (float)accessoryItem.ItemStats.Level);

                        ((ExperienceItemProperty)newProperty).ExperienceGainBonus = buffValue;
                        ((ExperienceItemProperty)newProperty).ApplyMethod = StatusEffect.EApplyMethod.Percentange;
                    }
                    break;
                case ItemProperty.EType.Gold:
                    {
                        newProperty = new GoldItemProperty();

						float buffValue = Random.Range(1.0f, (float)accessoryItem.ItemStats.Level);

                        ((GoldItemProperty)newProperty).GoldGainBonus = buffValue;
						((GoldItemProperty)newProperty).ApplyMethod = StatusEffect.EApplyMethod.Percentange;
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
                StatusEffect.EApplyMethod applyMethod = (StatusEffect.EApplyMethod)Random.Range(0, 0);

                float buffValue = 1.0f;

                // Based on the level, randomly choose values for the stats.
                switch (applyMethod)
                {
                    case StatusEffect.EApplyMethod.Percentange:
                        {
                             buffValue = Random.Range(1.0f, (float)accessoryItem.ItemStats.Level);
                        }
                        break;
                    case StatusEffect.EApplyMethod.Fixed:
                        {
							buffValue = (float)Random.Range(1, (float)accessoryItem.ItemStats.Level);
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

	private static PrimaryStats RandomAccessoryStats(int level)
	{
		// Random between different templates
		// The level impacts number of stats that can be allocated

		int points = RandomAcessoryStatPoints(level);

		PrimaryStats stats = new PrimaryStats();

		switch (currentAccessoryTemplate)
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

	private static string RandomAccessoryName(AccessoryItem accessoryItem)
	{
		AccessoryItem.EAccessoryType type = accessoryItem.Type;

		string name = RandomFlavourWord(accessoryItem.GradeEnum) + " " + (EAccessoryMaterial)currentAccessoryTemplate + " " + type.ToString();

		return name;
	}

	private static string RandomAccessoryCityThemedDescription(AccessoryItem accessoryItem)
	{
		string description = "Generic " + accessoryItem.Type;

		int maxStories = 2;
		int randomStory = Random.Range(0, maxStories + 1);

		switch (randomStory)
		{
			case 0: // [CraftSynonym] by [Person/Race] of [Location/Store].
				{
					int name = Random.Range(0, 2);
					int from = Random.Range(0, 2);

					string strName = (name == 0 ? RandomPersonName() : RandomRaceName(false));
					string strLocation = (from == 0 ? (" from " + RandomLocationName()) : (" of " + RandomStoreName(false)));

					description = RandomCreationSynoynm(false) + " by " + strName + strLocation + ".";
				}
				break;
			case 1: // [Store] accessories are currently the hottest store in town!
				{
					description = RandomBrandName(false) + " accessories are currently the hottest things in town!";
				}
				break;
			case 2: // Accessories designed by [Person/Store] are the latest craze!
				{
					int by = Random.Range(0, 2);
					string strBy = (by == 0 ? RandomPersonName() : RandomStoreName(false));

					description = "Accessories designed by " + strBy + " are the latest craze!";
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case.");
				}
				break;
		}

		return description;
	}

	private static string RandomAccessoryTowerThemedDescription(AccessoryItem accessoryItem)
	{
		string description = "Generic " + accessoryItem.Type;

		int maxStories = 2;
		int randomStory = Random.Range(0, maxStories);

		switch (randomStory)
		{
			case 0: 
				{
					description = "Really dirty but pretty.";
				}
				break;
			case 1: 
				{
					description = "Feels like it has been inside something.";
				}
				break;
			case 2: 
				{
					description = "Has a weird smell to it.";
				}
				break;
			default:
				{
					Debug.LogError("Unhandled case.");
				}
				break;
		}

		return description;
	}


	private static string RandomFlavourWord(Item.EGrade grade)
	{
		int rand = Random.Range(0, gradeQualitySynonyms[grade].Length);
	
		string flavourWord = gradeQualitySynonyms[grade][rand];

		return flavourWord;
	}

	private static string RandomCreationSynoynm(bool lower)
	{
		int rand = Random.Range(0, maxCraftSynoynms);

		string creationSynoynm = craftSynoynms[rand];

		if (lower)
		{
			creationSynoynm = creationSynoynm.ToLower();
		}

		return creationSynoynm;
	}

	private static string RandomPersonName()
	{
		// Title
		int rand = Random.Range(0, maxPersonTitle);
		string name = personNameTitles[rand];

		// Name
		rand = Random.Range(0, maxPersonNames);
		name += personNames[rand];

		// Suffix
		rand = Random.Range(0, maxPersonSuffix);
		if (personNameSuffixes[rand] != "")
		{
			name += personNameSuffixes[rand];
		}

		return name;
	}

	private static string RandomLocationName()
	{
		int rand = Random.Range(0, maxLocationNames);
		return locationNames[rand];
	}

	private static string RandomBrandName(bool lower)
	{
		int rand = Random.Range(0, maxBrandNames);
		return brandNames[rand];
	}

	private static string RandomStoreName(bool lower)
	{
		int rand = Random.Range(0, maxStoreNames);
		return storeNames[rand];
	}

	private static string RandomRaceName(bool lower)
	{
		int rand = Random.Range(0, maxRace);
		return raceNames[rand];
	}
}
