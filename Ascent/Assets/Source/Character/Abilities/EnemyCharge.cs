using UnityEngine;
using System.Collections;

public class EnemyCharge : Action
{
    private Vector3 target;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);
    }

    public override void StartAbility()
    {
        RAIN.Core.AIRig ai = owner.GetComponentInChildren<RAIN.Core.AIRig>();

        GameObject t = ai.AI.WorkingMemory.GetItem<GameObject>("targetCharge");

        target = t.transform.position;
    }

    public override void UpdateAbility()
    {
        owner.transform.position = target - Vector3.Normalize(target - owner.transform.position) * 2.0f;
        owner.StopAbility();
    }

    public override void EndAbility()
    {
    }
}
