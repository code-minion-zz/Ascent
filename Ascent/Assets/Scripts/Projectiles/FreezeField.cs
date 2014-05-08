using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeField : Projectile
{
#pragma warning disable 0414
    private Character owner;
    private Vector3 velocity;
    private Vector3 curVelocity;

#pragma warning disable 0414
	private int targets;

#pragma warning disable 0414
	private List<Character> charactersHit = new List<Character>();

    private Circle circle;

    private bool hitSomething;


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
                // Create a blood splatter effect on the enemy.
                GameObject block = EffectFactory.Singleton.CreateIceblock(c.transform.position, c.transform.rotation);

                SM_destroyThisTimed effectTime = block.GetComponent<SM_destroyThisTimed>();

                // Apply damage and knockback to the enemey
                CombatEvaluator combatEvaluator = new CombatEvaluator(owner, c);
                combatEvaluator.Add(new PhysicalDamageProperty(owner.Stats.Attack, 1.0f));
                combatEvaluator.Add(new StatusEffectCombatProperty(new FrozenDebuff(owner, c, effectTime.destroyTime)));
                combatEvaluator.Apply();
            }
        }
    }

    public void Update()
    {

    }
}
