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
		GameObject go = Game.Singleton.Tower.CurrentFloor.CurrentRoom.InstantiateGameObject("Prefabs/Bomb");

		go.GetComponent<Bomb>().Initialise(user, 3.5f, 3.0f, 25.0f);
	}
}
