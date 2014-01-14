using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorHeavyStrike : Action 
{
    bool performed = false;
    public float radius = 4.0f;
    public float arcAngle = 200.0f;

    private Arc swingArc;

    public override void Initialise(Character owner)
    {
        animationSpeed = 3.0f;
        animationLength = 1.167f / 1.5f;
        coolDownTime = 5.0f;
        currentTime = 0.0f;
        animationTrigger = "SwingAttack";
        specialCost = 0;

        swingArc = new Arc();
        swingArc.radius = radius;
        swingArc.arcAngle = arcAngle;
        swingArc.transform = owner.transform;

        base.Initialise(owner);
    }

    public override void StartAbility()
    {
        currentTime = 0.0f;
        performed = false;

        animationLength = 1.167f / animationSpeed;
        owner.Animator.Animator.SetFloat("SwordAttackSpeed", animationSpeed);

        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (!performed)
        {
            if (currentTime >= animationLength * 0.5f)
            {
                List<Character> enemies = new List<Character>();
                if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
                {
                    foreach (Enemy e in enemies)
                    {
                        e.ApplyDamage(25, Character.EDamageType.Physical);
                        e.ApplyKnockback(e.transform.position - owner.transform.position, 100000000000.0f);
                    }
                }

                performed = true;
            }
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
    }

    public override void DebugDraw()
    {
        //arcLine = MathUtility.RotateAboutPoint(owner.transform.forward * radius, owner.transform.position, -arcAngle * 0.5f);
        //arcLine2 = MathUtility.RotateAboutPoint(owner.transform.forward * radius, owner.transform.position, arcAngle * 0.5f);

        //Debug.DrawLine(owner.transform.position, owner.transform.position + arcLine); // To the rotated arc
        //Debug.DrawLine(owner.transform.position, owner.transform.position + arcLine2); // To the rotated arc

        //Handles.DrawWireArc(owner.transform.position, Vector3.up, arcLine, arcAngle, radius);

        swingArc.DebugDraw();
    }
}
