using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    private float damage;
    public Vector3 originalPos;
    public float startTime;
    public float distance;

    public void Initialise(float damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter(Collider trigger)
    {

        if (trigger.transform.tag == "Hero")
        {
            Hero hero = trigger.transform.GetComponent<Hero>();
            // Apply damage to the hero
            CombatEvaluator combatEvaluator = new CombatEvaluator(null, hero);
            combatEvaluator.Add(new TrapDamageProperty(damage, 1.0f));
            combatEvaluator.Apply();
            Game.Singleton.EffectFactory.CreateBloodSplatter(trigger.transform.position, trigger.transform.rotation, hero.transform);
        }
    }
}
