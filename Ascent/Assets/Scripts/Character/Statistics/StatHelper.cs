using UnityEngine;
using System.Collections;

public static class StatHelper
{
	public static int Power(Character character)
	{
		// Get character base statistics
		int power = character.CharacterStats.Power;
		
		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).HeroBackpack;
			int itemCount = backPack.AccessoryCount;
			
			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[(Backpack.BackpackSlot)i];
					if (item is ConsumableItem)
					{
						continue;
					}
					
					BaseStats baseStats = ((AccessoryItem)item).Stats;//.Power;
					
					if (baseStats != null)
					{
						power += baseStats.Power;
					}
				}
			}
		}
		
		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList =  character.BuffList;
		
		int buffCount =	buffList.size;
		
		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					power += ((BaseStatBuff)buffList[i]).Stats.Power;
				}
			}
		}
		
		return power;
	}

	public static int Finesse(Character character)
	{
		// Get character base statistics
		int finesse = character.CharacterStats.Finesse;
		
		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).HeroBackpack;
			int itemCount = backPack.AccessoryCount;
			
			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[(Backpack.BackpackSlot)i];
					if (item is ConsumableItem)
					{
						continue;
					}
					
					BaseStats baseStats = ((AccessoryItem)item).Stats;//.Power;
					
					if (baseStats != null)
					{
						finesse += baseStats.Finesse;
					}
				}
			}
		}
		
		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList =  character.BuffList;
		
		int buffCount =	buffList.size;
		
		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					finesse += ((BaseStatBuff)buffList[i]).Stats.Finesse;
				}
			}
		}
		
		return finesse;
	}

	public static int Vitality(Character character)
	{
		// Get character base statistics
		int vitality = character.CharacterStats.Vitality;
		
		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).HeroBackpack;
			int itemCount = backPack.AccessoryCount;
			
			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					//Backpack.BackpackSlot bs = (Backpack.BackpackSlot)i;
					Item item = backPack.AllItems[(Backpack.BackpackSlot)i];
					if (item is ConsumableItem)
					{
						continue;
					}
					
					BaseStats baseStats = ((AccessoryItem)item).Stats;//.Power;
					
					if (baseStats != null)
					{
						vitality += baseStats.Vitality;
					}
				}
			}
		}
		
		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList =  character.BuffList;
		
		int buffCount =	buffList.size;
		
		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					vitality += ((BaseStatBuff)buffList[i]).Stats.Vitality;
				}
			}
		}
		
		return vitality;
	}

	public static int Spirit(Character character)
	{
		// Get character base statistics
		int spirit = character.CharacterStats.Spirit;
		
		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).HeroBackpack;
			int itemCount = backPack.AccessoryCount;
			
			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[(Backpack.BackpackSlot)i];
					if (item is ConsumableItem)
					{
						continue;
					}
					
					BaseStats baseStats = ((AccessoryItem)item).Stats;//.Power;
					
					if (baseStats != null)
					{
						spirit += baseStats.Spirit;
					}
				}
			}
		}
		
		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList =  character.BuffList;
		
		int buffCount =	buffList.size;
		
		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					spirit += ((BaseStatBuff)buffList[i]).Stats.Spirit;
				}
			}
		}
		
		return spirit;
	}

//    public static int Attack(Character character, HeroEquipment equipment)
//    {
//        // Get character base statistics
//        int power = character.CharacterStats.Power;
//
//        if (equipment != null)
//        {
//            // Go through the list of items and add power
//
//        }
//
//        // Add status buff modifiers
//
//        return power;
//    }
}
