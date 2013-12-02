using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarStomp : Action
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        coolDown = 3.0f;
        animationTrigger = "WarStomp";
    }

    public override void StartAbility()
    {
        base.StartAbility();
    }

    public override void UpdateAbility()
    {

    }

    public override void EndAbility()
    {
        base.EndAbility();
    }
}
