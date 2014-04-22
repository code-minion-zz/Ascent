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
        cooldownFullDuration = 0.0f;
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

			int randomRandomMissiles = Random.Range(2, 5);
			for (int i = 0; i < randomRandomMissiles; ++i)
			{
				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/HomingMagicMissile")) as GameObject;
				arrowGO.GetComponent<HomingMagicMissile>().Initialise(eyePositions[i].position, owner);
				arrowGO.transform.parent = owner.transform;
				performed = true;
			}

			int randomHomingMissiles = Random.Range(8, 10);
			for (int i = 3; i < randomHomingMissiles; ++i)
			{
				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/RandomMagicMissile")) as GameObject;
                arrowGO.GetComponent<RandomMagicMissile>().Initialise(eyePositions[i].position, owner);
				arrowGO.transform.parent = owner.transform;
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
