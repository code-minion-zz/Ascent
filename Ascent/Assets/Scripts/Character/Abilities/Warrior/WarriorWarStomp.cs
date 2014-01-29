using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorWarStomp : Action
{
    private const float explosionMaxRadius = 10.0f;
    private GameObject stompObject;
    private GameObject prefab;
    private bool performed;

    public float radius = 3.0f;
    public float knockBack = 10.0f;
    public int damage = 28;

    private Circle collisionShape;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        animationLength = 1.2f;
        animationSpeed = 2.0f;
        animationTrigger = "WarStomp";
        coolDownTime = 3.0f;
        specialCost = 5;

        prefab = Resources.Load("Prefabs/Effects/WarStompEffect") as GameObject;

        collisionShape = new Circle(owner.transform, radius, new Vector3(0.0f,0.0f,0.0f));
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
            if (currentTime >= animationLength * 0.5f)
            {
                List<Character> enemies = new List<Character>();

				if (Game.Singleton.InTower)
				{
					if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(collisionShape, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							e.ApplyDamage(damage, Character.EDamageType.Physical, owner);
							e.ApplyStunEffect(2.25f);

							// Create a blood splatter effect on the enemy.
							Game.Singleton.EffectFactory.CreateBloodSplatter(e.transform.position, e.transform.rotation, e.transform, 3.0f);
						}
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

#if UNITY_EDITOR
    public override void DebugDraw()
    {
        collisionShape.DebugDraw();
    }
#endif
}
