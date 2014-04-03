using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeField : Projectile
{
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
		transform.position = startPos;
		transform.rotation = owner.transform.rotation;

        circle = new Circle(transform, 3.0f, new Vector3(0.0f, 0.0f, 1.5f));

        List<Character> characters = new List<Character>();
        Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

        Character.EScope scope = owner is Enemy ? Character.EScope.Hero : Character.EScope.Enemy;

        if (curRoom.CheckCollisionArea(circle, scope, ref characters))
        {
            foreach (Character c in characters)
            {
                // Apply damage and knockback to the enemey
                CombatEvaluator combatEvaluator = new CombatEvaluator(owner, c);
                combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
                combatEvaluator.Add(new StatusEffectCombatProperty(new FrozenDebuff(owner, c, 3.0f)));
                combatEvaluator.Apply();

                // Create a blood splatter effect on the enemy.
                Game.Singleton.EffectFactory.CreateBloodSplatter(c.transform.position, c.transform.rotation, c.transform);
            }
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {

        //circle.DebugDraw();
    }
#endif
}
