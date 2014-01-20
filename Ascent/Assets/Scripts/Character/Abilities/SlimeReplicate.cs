using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeReplicate : Action
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 0.2f;
        animationSpeed = 1.0f;
        animationTrigger = "Replicate";
        coolDownTime = 2.0f;
        specialCost = 0;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        owner.Motor.StopMotion();
        owner.Motor.EnableMovementForce(false);
        owner.SetColor(Color.red);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        Enemy enemy = owner as Enemy;
        GameObject go = enemy.ContainedRoom.InstantiateGameObject(Room.ERoomObjects.Enemy, "Slime");
        go.transform.position = owner.transform.position;
        go.transform.position += Vector3.left * 0.1f;
        owner.transform.position += Vector3.right * 0.1f;

        owner.DerivedStats.CurrentHealth = (int)((float)owner.DerivedStats.CurrentHealth * 0.5f);

        float scale = (float)owner.DerivedStats.CurrentHealth / (float)owner.DerivedStats.MaxHealth;
        if (scale > 0.15f)
        {
            if (scale < 0.15f)
            {
                scale = 0.15f;
            }
            owner.transform.localScale = new Vector3(scale, scale, scale);
        }
        go.transform.localScale = owner.transform.localScale;

        go.GetComponent<Enemy>().DerivedStats.MaxHealth = owner.DerivedStats.MaxHealth;
        go.GetComponent<Enemy>().DerivedStats.CurrentHealth = owner.DerivedStats.CurrentHealth;

        //go.GetComponent<Enemy>().ApplyKnockback(Vector3.left, 100.0f);
        //enemy.ApplyKnockback(Vector3.right, 100.0f);

        base.EndAbility();
        owner.SetColor(owner.OrigionalColor);
        owner.Motor.EnableMovementForce(true);
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {

    }
#endif

}
