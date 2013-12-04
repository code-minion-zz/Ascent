using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarStomp : Action
{
    private const float explosionMaxRadius = 10.0f;
    private const float speed = 4.0f;
    private GameObject stompObject;
    private GameObject prefab;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        animationLength = 3.0f;
        animationTrigger = "WarStomp";
        coolDownTime = 5.0f;

        prefab = Resources.Load("Prefabs/WarStompEffect") as GameObject;
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
        GameObject.Destroy(stompObject, animationLength);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (currentTime >= 1.5f)
        {
            if (stompObject != null)
            {
                stompObject.transform.position = owner.transform.position;
                stompObject.transform.localScale = Vector3.Lerp(stompObject.transform.localScale, new Vector3(explosionMaxRadius, 0.0f, explosionMaxRadius), Time.deltaTime * speed);
            }
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
    }
}
