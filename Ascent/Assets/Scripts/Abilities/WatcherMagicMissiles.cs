using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherMagicMissile : Ability
{
	private Vector2 randomHomingMissiles = new Vector2(1, 3);
	private Vector2 randomRandomMissiles = new Vector2(5, 10);
	 
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

	public void Enrage()
	{
		randomRandomMissiles = new Vector2(8, 12);
		randomHomingMissiles = new Vector2(3, 4);
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

			int x = (int)(randomRandomMissiles.x);
			int y = (int)(randomRandomMissiles.y);
			int randomRandomMissilesCount = Random.Range(x, y);
			for (int i = 0; i < randomRandomMissilesCount; ++i)
			{
				int eyePos = i;
				if (eyePos > 10)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/HomingMagicMissile")) as GameObject;
				arrowGO.GetComponent<HomingMagicMissile>().Initialise(eyePositions[eyePos].position, owner);
				arrowGO.transform.parent = owner.transform;
				performed = true;
			}

			x = (int)(randomHomingMissiles.x);
			y = (int)(randomHomingMissiles.y);
			int randomHomingMissilesCount = Random.Range(x, y);
			for (int i = 3; i < randomHomingMissilesCount; ++i)
			{
				int eyePos = i;
				if (eyePos > 10)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/RandomMagicMissile")) as GameObject;
				arrowGO.GetComponent<RandomMagicMissile>().Initialise(eyePositions[eyePos].position, owner);
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
