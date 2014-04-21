using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    private float damage;
    private bool collided;
    public Vector3 originalPos;
    public float startTime;
    public float distance;

    public bool IsCollided
    {
        get { return collided; }
        set { collided = value; }
    }

    public void Initialise(float damage)
    {
        this.damage = damage;
        collided = false;
    }

    void OnTriggerEnter(Collider trigger)
    {

        if (trigger.transform.tag == "Hero" && collided == false)
        {
            collided = true;
            Hero hero = trigger.transform.GetComponent<Hero>();
            // Apply damage to the hero
            CombatEvaluator combatEvaluator = new CombatEvaluator(null, hero);
            combatEvaluator.Add(new TrapDamageProperty(damage, 1.0f));
            combatEvaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(null, hero, 3.0f)));
            combatEvaluator.Apply();
            EffectFactory.Singleton.CreateBloodSplatter(trigger.transform.position, trigger.transform.rotation);
        }
    }
}
