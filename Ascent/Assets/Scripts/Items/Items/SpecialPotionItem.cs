using UnityEngine;
using System.Collections;

public class SpecialPotionItem : ConsumableItem 
{
    protected override bool CanUse(Hero user)
    {
        return user.HeroStats.CurrentSpecial < user.HeroStats.MaxSpecial;
    }

	protected override void Consume(Hero user)
	{
		// Restore special
	}
}
