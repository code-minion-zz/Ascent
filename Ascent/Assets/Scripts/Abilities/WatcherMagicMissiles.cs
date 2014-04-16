using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherMagicMissile : Ability
{
    private bool performed = false;
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 1.833f;
        animationSpeed = 1.0f;
        animationTrigger = "Spin";
        cooldownFullDuration = 3.0f;
        specialCost = 0;
    }

    public override void StartAbility()
    {
        base.StartAbility();
        performed = false;

        owner.Animator.PlayAnimation(animationTrigger, true);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (timeElapsedSinceStarting >= animationLength * 0.5f && !performed)
        {
            WatcherBoss boss = owner.GetComponent<WatcherBoss>();
            Transform[] eyePositions = boss.eyes;

            foreach (Transform t in eyePositions)
            {
                GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/MagicMissile")) as GameObject;
                arrowGO.GetComponent<MagicMissile>().Initialise(t.position, owner);
                performed = true;
            }
        }
    }

    public override void EndAbility()
    {
        owner.Animator.PlayAnimation(animationTrigger, false);
        base.EndAbility();
    }
}
