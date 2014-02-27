using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpStrike : Ability
{
    private Circle damageArea;
    private float prevSpeed;
    private bool executedDamage;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 1.5f;
        animationSpeed = 1.0f;
        animationTrigger = "Strike";
        cooldownFullDuration = 2.0f;
        specialCost = 0;

        damageArea = new Circle(owner.transform, 1.0f, new Vector3(0.0f, 0.0f, 0.5f));

		//Validate();
    }

    public override void StartAbility()
    {
        base.StartAbility();

        owner.Motor.StopMotion();
        owner.Motor.EnableStandardMovement(false);
        owner.SetColor(Color.red);

        prevSpeed = owner.Motor.MaxSpeed;
        executedDamage = false;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

		if (timeElapsedSinceStarting <= animationLength * 0.45f && !executedDamage)
		{
			owner.Model.transform.position = new Vector3(owner.Model.transform.position.x, Mathf.PingPong(timeElapsedSinceStarting, animationLength * 0.075f) * 20.0f, owner.Model.transform.position.z);
		}
		else
		{
			owner.Model.transform.position = new Vector3(owner.Model.transform.position.x, 0.0f, owner.Model.transform.position.z);
		}

        if (timeElapsedSinceStarting >= animationLength * 1.0f)
        {
            owner.Motor.EnableStandardMovement(true);
            owner.ResetColor();
        }
        else if (timeElapsedSinceStarting >= animationLength * 0.8f)
        {
            owner.Motor.StopMotion();
            owner.Motor.EnableStandardMovement(false);
            owner.Motor.MaxSpeed = prevSpeed;
        }
        else if (timeElapsedSinceStarting >= animationLength * 0.40f && !executedDamage)
        {
            List<Character> characters = new List<Character>();

            if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(damageArea, Character.EScope.Hero, ref characters))
            {
                foreach (Character c in characters)
                {
					// Apply damage and knockback to the enemey.
					CombatEvaluator combatEvaluator = new CombatEvaluator(owner, c);
					combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 1.1f));
					combatEvaluator.Apply();

                    // Create a blood splatter effect on the enemy.
                    Game.Singleton.EffectFactory.CreateBloodSplatter(c.transform.position, c.transform.rotation, c.transform, 2.0f);
                }

                executedDamage = true;
				owner.ResetColor();
            }
        }
        else if (timeElapsedSinceStarting >= animationLength * 0.25f)
        {
            owner.Motor.EnableStandardMovement(true);
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Motor.EnableStandardMovement(true);
        owner.ResetColor();
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        damageArea.DebugDraw();
    }
#endif

}
