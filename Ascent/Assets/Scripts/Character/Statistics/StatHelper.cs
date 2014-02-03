using UnityEngine;
using System.Collections;

public static class StatHelper
{
	public static int Power(Character character)
	{
		// Get character base statistics
		int power = character.Stats.Power;

		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).Backpack;
			int itemCount = backPack.AccessoryCount;

			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[i];
					if (item is ConsumableItem)
					{
						continue;
					}

					power += (int)((AccessoryItem)item).AcessoryStats.Power;
				}
			}
		}

		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList = character.BuffList;

		int buffCount = buffList.size;

		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					power += (int)((BaseStatBuff)buffList[i]).Power;
				}
			}
		}

		return power;
	}

	public static int Finesse(Character character)
	{
		// Get character base statistics
		int finesse = character.Stats.Finesse;

		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).Backpack;
			int itemCount = backPack.AccessoryCount;

			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[i];
					if (item is ConsumableItem)
					{
						continue;
					}

					finesse += (int)((AccessoryItem)item).AcessoryStats.Finesse;
				}
			}
		}

		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList = character.BuffList;

		int buffCount = buffList.size;

		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					finesse += (int)((BaseStatBuff)buffList[i]).Finesse;
				}
			}
		}

		return finesse;
	}

	public static int Vitality(Character character)
	{
		// Get character base statistics
		int vitality = character.Stats.Vitality;

		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).Backpack;
			int itemCount = backPack.AccessoryCount;

			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					//Backpack.BackpackSlot bs = (Backpack.BackpackSlot)i;
					Item item = backPack.AllItems[i];
					if (item is ConsumableItem)
					{
						continue;
					}

					vitality += (int)((AccessoryItem)item).AcessoryStats.Vitality;
				}
			}
		}

		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList = character.BuffList;

		int buffCount = buffList.size;

		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					vitality += (int)((BaseStatBuff)buffList[i]).Vitality;
				}
			}
		}

		return vitality;
	}

	public static int Spirit(Character character)
	{
		// Get character base statistics
		int spirit = character.Stats.Spirit;

		if (character is Hero)
		{
			// Go through the list of items and add power
			Backpack backPack = ((Hero)character).Backpack;
			int itemCount = backPack.AccessoryCount;

			if (itemCount > 0)
			{
				int i;
				for (i = 0; i < itemCount; ++i)
				{
					Item item = backPack.AllItems[i];
					if (item is ConsumableItem)
					{
						continue;
					}

					spirit += (int)((AccessoryItem)item).AcessoryStats.Spirit;
				}
			}
		}

		// Add status buff modifiers
		//character.}
		BetterList<Buff> buffList = character.BuffList;

		int buffCount = buffList.size;

		if (buffCount > 0)
		{
			int i;
			for (i = 0; i < buffCount; ++i)
			{
				if (buffList[i] is BaseStatBuff)
				{
					spirit += (int)((BaseStatBuff)buffList[i]).Spirit;
				}
			}
		}

		return spirit;
	}
}
