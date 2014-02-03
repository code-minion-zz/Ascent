using UnityEngine;
using System.Collections;

public class HealthPotionItem : ConsumableItem 
{
    protected override bool CanUse(Hero user)
    {
        return user.HeroStats.CurrentHealth < user.HeroStats.MaxHealth;
    }

	protected override void Consume(Hero user)
	{
        // 10HP + 10% of maxHP
        user.HeroStats.CurrentHealth += 10 + (int)(user.HeroStats.MaxHealth * 0.1f);
	}
}
