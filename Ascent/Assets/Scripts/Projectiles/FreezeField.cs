using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeField : Projectile
{
    public Projectile projectile;

    private Character owner;

    private int targets;

    private List<Character> charactersHit = new List<Character>();

    private Circle circle;

    private bool hitSomething;

    private Vector3 velocity;

    private float time;

    public void Initialise(int targets, Vector3 startPos, Character owner)
    {
        this.targets = targets;
        this.owner = owner;

        circle = new Circle(owner.transform, 3.0f, new Vector3(0.0f, 0.0f, 1.5f));

        List<Character> characters = new List<Character>();
        Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

        if (curRoom.CheckCollisionArea(circle, Character.EScope.Enemy, ref characters))
        {
            foreach (Character c in characters)
            {
                // Apply damage and knockback to the enemey
                CombatEvaluator combatEvaluator = new CombatEvaluator(owner, c);
                combatEvaluator.Add(new PhysicalDamageProperty(0.0f, 1.0f));
                combatEvaluator.Add(new StatusEffectCombatProperty(new FrozenDebuff(owner, c, 3.0f)));
                combatEvaluator.Apply();

                // Create a blood splatter effect on the enemy.
                Game.Singleton.EffectFactory.CreateBloodSplatter(c.transform.position, c.transform.rotation, c.transform, 2.0f);
            }
        }
    }

    public void Update()
    {
        time += Time.time;
        if(time < 1.0f)
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
        circle.DebugDraw();
    }
#endif
}
