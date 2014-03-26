using UnityEngine;
using System.Collections;

public class ChestOpenTrigger : EnvironmentTrigger 
{
    public TreasureChest chest;

    protected override bool HasTriggerBeenMet()
    {
        return (!chest.IsClosed);
    }
}
