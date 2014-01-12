using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarStomp : Action
{
    private const float explosionMaxRadius = 10.0f;
    private GameObject stompObject;
    private GameObject prefab;
    private bool performed;

    public float radius = 3.14f;
    public float arcAngle = 365.0f;
    public float knockBack = 10.0f;

    private Vector3 arcLine;
    private Vector3 arcLine2;
    private Arc swingArc;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        animationLength = 1.2f;
        animationSpeed = 2.0f;
        animationTrigger = "WarStomp";
        coolDownTime = 0.0f;
        specialCost = 5;

        prefab = Resources.Load("Prefabs/Effects/WarStompEffect") as GameObject;


        swingArc = new Arc();
        swingArc.radius = radius;
        swingArc.arcAngle = arcAngle;
        swingArc.transform = owner.transform;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        // Creation of the stomp
        stompObject = GameObject.Instantiate(prefab) as GameObject;
        stompObject.transform.position = owner.transform.position;
        stompObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

        //SphereCollider sc = stompObject.GetComponent<SphereCollider>();
        //sc.isTrigger = true;

        stompObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        GameObject.Destroy(stompObject, animationLength / animationSpeed);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (stompObject != null)
        {
            stompObject.transform.position = owner.transform.position;
            stompObject.transform.localScale = Vector3.Lerp(stompObject.transform.localScale, new Vector3(explosionMaxRadius, 0.0f, explosionMaxRadius), Time.deltaTime * animationSpeed);
        }

        swingArc.Process();

        if (!performed)
        {
            if (currentTime >= animationLength * 0.5f)
            {
                List<Character> enemies = new List<Character>();

                if (Game.Singleton.Tower.CurrentFloor.CurrentRoom.CheckCollisionArea(swingArc, Character.EScope.Enemy, ref enemies))
                {
                    foreach (Enemy e in enemies)
                    {
                        e.ApplyDamage(10, Character.EDamageType.Physical);
                        e.ApplyKnockback(e.transform.position - owner.transform.position, knockBack);
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
        swingArc.DebugDraw();
    }
}
