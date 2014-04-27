using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CollisionTrigger : EnvironmentTrigger 
{
    private bool isCollision;
    // If you require a once off collision such as a trap trigger then the trigger condition will be met.
    public bool isTriggerOnce;
    // If you require the collision to be constant then the trigger condition is met.
    public bool isTriggerStay;
    private BoxCollider boxTrigger;

    protected override bool HasTriggerBeenMet()
    {
        if (isCollision)
        {
            Debug.Log("Triggered collision");
            return true;
        }

        return false;
    }

    public void Start()
    {
        boxTrigger = GetComponent<BoxCollider>();
        boxTrigger.isTrigger = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (isTriggerOnce)
        {
            isCollision = true;
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (isTriggerStay)
        {
            isCollision = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (isTriggerStay)
        {
            isCollision = false;
        }
    }

#if UNITY_EDITOR
	void OnDrawGizmos()
    {
        if (isCollision)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = new Color(255.0f, 140.0f, 0.0f);
        }

        if (boxTrigger == null)
        {
            boxTrigger = GetComponent<BoxCollider>();
        }

        Gizmos.DrawWireCube(boxTrigger.bounds.center, boxTrigger.bounds.size);
    }
#endif
}
