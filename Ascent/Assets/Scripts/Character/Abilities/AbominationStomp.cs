using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbominationStomp : Action
{
    private const float explosionMaxRadius = 7.5f;
    private GameObject stompObject;
    private GameObject prefab;
    private bool performed;

    public float radius = 2.5f;
    public float knockBack = 60.0f;
    public int damage = 1;

    private Circle collisionShape;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        animationLength = 1.2f;
        animationSpeed = 2.0f;
        animationTrigger = "Stomp";
        cooldownDurationMax = 3.0f;
        specialCost = 0;

        prefab = Resources.Load("Prefabs/Effects/WarStompEffect") as GameObject;

        collisionShape = new Circle(owner.transform, radius, new Vector3(0.0f, 0.0f, 0.0f));
    }

    public override void StartAbility()
    {
        base.StartAbility();

        // Creation of the stomp visual appearence.
        stompObject = GameObject.Instantiate(prefab) as GameObject;
        stompObject.transform.position = owner.transform.position;
        stompObject.transform.localScale = new Vector3(0.0f, 1.0f, 0.0f);
        GameObject.Destroy(stompObject, animationLength / animationSpeed);

        performed = false;

        owner.Motor.StopMotion();
        owner.Motor.EnableMovementForce(false);
        owner.SetColor(Color.red);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (stompObject != null)
        {
            stompObject.transform.position = owner.transform.position;
            stompObject.transform.localScale = Vector3.Lerp(stompObject.transform.localScale, new Vector3(explosionMaxRadius, 1.0f, explosionMaxRadius), Time.deltaTime * animationSpeed);
        }

        if (!performed)
        {
            if (timeElapsedSinceStarting >= animationLength * 0.5f)
            {
                List<Character> characters = new List<Character>();

                if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(collisionShape, Character.EScope.Hero, ref characters))
                {
                    foreach (Character c in characters)
                    {
						c.ApplyDamage(owner.DamageFormulaA(1, 0.75f), Character.EDamageType.Physical, owner);
                        c.ApplyStunEffect(1.0f);
                        c.ApplyKnockback(c.transform.position - owner.transform.position, knockBack);

                        // Create a blood splatter effect on the enemy.
                        Game.Singleton.EffectFactory.CreateBloodSplatter(c.transform.position, c.transform.rotation, c.transform, 3.0f);
                    }
                }

                Game.Singleton.Tower.CurrentFloor.FloorCamera.ShakeCamera(0.05f, 0.02f);

                performed = true;
            }
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();

        owner.Motor.EnableMovementForce(true);
        owner.ResetColor();
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        collisionShape.DebugDraw();
    }
#endif
}
