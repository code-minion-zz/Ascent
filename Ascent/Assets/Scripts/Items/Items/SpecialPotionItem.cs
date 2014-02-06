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
		// 10HP + 10% of maxHP
		user.HeroStats.CurrentSpecial += 10 + (int)(user.HeroStats.MaxSpecial * 0.1f);
	}
}
