using UnityEngine;
using System.Collections;

public class BombItem : ConsumableItem
{
    protected override bool CanUse(Hero user)
    {
        return true;
    }

	protected override void Consume(Hero user)
	{
		// Drop a bomb on the groud (No animation for now...)
	}
}
