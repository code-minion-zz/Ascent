using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMonstersAction : EnvironmentAction 
{
    public List<Character> activateMonsters;

    public override void ExecuteAction()
    {
        foreach (Enemy monster in activateMonsters)
        {
            // TODO: Fix this, the monsters attack with weird shaddow.
            // Make them just respawn.
            monster.gameObject.SetActive(true);
            monster.Initialise();
            monster.InitiliseHealthbar();
        }
    }
}
