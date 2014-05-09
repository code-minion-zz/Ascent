using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherMagicMissile : Ability
{
	//private Vector2 randomHomingMissiles = new Vector2(1, 3);
	private Vector2 randomRandomMissiles = new Vector2(7, 15);

	private int thirdMissileCount;
	 
    private bool performedA = false;
	private bool performedB = false;
	private bool performedC = false;

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
		randomRandomMissiles = new Vector2(13, 20);
		//randomHomingMissiles = new Vector2(3, 4);
	}

    public override void StartAbility()
    {
        base.StartAbility();
		performedA = false;
		performedB = false;
		performedC = false;

        owner.Animator.PlayAnimation(animationTrigger, true);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

		WatcherBoss boss = owner.GetComponent<WatcherBoss>();

		if (boss == null)
			return;

		if (boss.IsDead)
			return;

		Transform[] eyePositions = boss.eyes;

		if (timeElapsedSinceStarting >= animationLength * 0.5f && !performedA)
        {
			var players = Game.Singleton.Players;
			for (int i = 0; i < Game.Singleton.AlivePlayerCount; ++i)
			{
				if (players[i].Hero.IsDead)
				{
					continue;
				}
				
				int eyePos = i;
				if (eyePos > eyePositions.Length)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/HomingMagicMissile")) as GameObject;
				arrowGO.GetComponent<HomingMagicMissile>().Initialise(eyePositions[eyePos].position, owner, players[i].Hero);
				arrowGO.transform.parent = owner.transform;
			}

			int x = (int)(randomRandomMissiles.x);
			int y = (int)(randomRandomMissiles.y);
			int randomRandomMissilesCount = Random.Range(x, y);

			thirdMissileCount = randomRandomMissilesCount / 3;
			for (int i = 0; i < thirdMissileCount; ++i)
			{
				int eyePos = i;
				if (eyePos >= eyePositions.Length)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/RandomMagicMissile")) as GameObject;
				arrowGO.GetComponent<RandomMagicMissile>().Initialise(eyePositions[eyePos].position, owner);
				arrowGO.transform.parent = owner.transform;
			}

			performedA = true;
        }
		if (timeElapsedSinceStarting >= animationLength * 0.6f && !performedB)
		{
			for (int i = 0; i < thirdMissileCount; ++i)
			{
				int eyePos = i;
				if (eyePos >= eyePositions.Length)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/RandomMagicMissile")) as GameObject;
				arrowGO.GetComponent<RandomMagicMissile>().Initialise(eyePositions[eyePos].position, owner);
				arrowGO.transform.parent = owner.transform;
			}

			performedB = true;
		}
		if (timeElapsedSinceStarting >= animationLength * 0.7f && !performedC)
		{
			for (int i = 0; i < thirdMissileCount; ++i)
			{
				int eyePos = i;
				if (eyePos >= eyePositions.Length)
				{
					eyePos = Random.Range(0, 10);
				}

				GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/RandomMagicMissile")) as GameObject;
				arrowGO.GetComponent<RandomMagicMissile>().Initialise(eyePositions[eyePos].position, owner);
				arrowGO.transform.parent = owner.transform;
			}

			performedC = true;
		}
    }

    public override void EndAbility()
    {
        owner.Animator.PlayAnimation(animationTrigger, false);
        base.EndAbility();
    }
}
