using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeReplicate : Action
{
    private const int splitLimit = 3;
    public int timesSplit = 0;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 0.2f;
        animationSpeed = 1.0f;
        animationTrigger = "Replicate";
        cooldownDurationMax = 2.0f;
        specialCost = 0;
        
    }

    public override void StartAbility()
    {
        if (timesSplit < splitLimit)
        {
            base.StartAbility();

            owner.Motor.StopMotion();
            owner.Motor.EnableMovementForce(false);
            owner.SetColor(Color.red);
        }
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        if (timesSplit < splitLimit)
        {
            Enemy enemy = owner as Enemy;
            GameObject go = enemy.ContainedRoom.InstantiateGameObject(Room.ERoomObjects.Enemy, "Slime");
            go.transform.position = owner.transform.position;
            go.transform.position += Vector3.left * 0.1f;
            owner.transform.position += Vector3.right * 0.1f;

            go.GetComponent<Enemy>().AIAgent.SteeringAgent.StartPosition = go.transform.position;

            owner.Stats.CurrentHealth = (int)((float)owner.Stats.CurrentHealth * 0.75f);

            float scale = (float)owner.Stats.CurrentHealth / (float)owner.Stats.MaxHealth;
            if (scale > 0.15f)
            {
                if (scale < 0.15f)
                {
                    scale = 0.15f;
                }
                owner.transform.localScale = new Vector3(scale, scale, scale);
            }
            go.transform.localScale = owner.transform.localScale;

            //go.GetComponent<Enemy>().Stats.MaxHealth = owner.Stats.MaxHealth;
            go.GetComponent<Enemy>().Stats.CurrentHealth = owner.Stats.CurrentHealth;

            //go.GetComponent<Enemy>().ApplyKnockback(Vector3.left, 100.0f);
            //enemy.ApplyKnockback(Vector3.right, 100.0f);

            SlimeReplicate slimeReplicateB = go.GetComponent<Slime>().GetAbility("SlimeReplicate") as SlimeReplicate;

            timesSplit += 1;
            slimeReplicateB.timesSplit = timesSplit;

            owner.SetColor(owner.OriginalColor);
            owner.Motor.EnableMovementForce(true);
        }
        base.EndAbility();
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {

    }
#endif

}
