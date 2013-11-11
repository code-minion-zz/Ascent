using UnityEngine;
using System.Collections;

public class EnemyTackle : Action
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
    }

    public override void StartAbility()
    {

    }

    public override void UpdateAbility()
    {
        owner.StopAbility();
    }

    public override void EndAbility()
    {

    }

}
