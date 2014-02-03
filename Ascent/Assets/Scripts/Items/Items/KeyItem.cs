using UnityEngine;
using System.Collections;

public class KeyItem : ConsumableItem 
{
    protected override bool CanUse(Hero user)
    {
        return true;
    }

	protected override void Consume(Hero user)
	{
		// Open nearby door
	}
}
