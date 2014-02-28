﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : Projectile
{
    public Projectile projectile;

    private Character owner;

    private int targets;

    private List<Character> charactersHit = new List<Character>();

    private Circle circle;

    private bool hitSomething;

    private Vector3 velocity;

    public void Initialise(int targets, Vector3 startPos, Character owner)
    {
        this.targets = targets;
        this.owner = owner;

        projectile.transform.position = new Vector3(startPos.x, 1.0f, startPos.z);
        projectile.rigidbody.AddForce(owner.transform.forward * 10.0f, ForceMode.VelocityChange);

        velocity = owner.transform.forward * 10.0f;

        circle = new Circle(transform, 3.0f, Vector3.zero);
    }

    public void Update()
    {
        projectile.rigidbody.AddForce(velocity, ForceMode.Force);
    }


    public void OnCollisionEnter(Collision collision)
    {
        bool lightningExpired = false;

        if (!hitSomething)
        {
            switch ((Layer)collision.gameObject.layer)
            {
                case Layer.Monster:
                    {
                        Character hitCharacter = collision.gameObject.GetComponent<Character>();

                        if (!charactersHit.Contains(hitCharacter))
                        {
                            hitSomething = true;
                            charactersHit.Add(hitCharacter);

                            // Apply damage and knockback to the enemey
                            CombatEvaluator combatEvaluator = new CombatEvaluator(owner, hitCharacter);
                            combatEvaluator.Add(new PhysicalDamageProperty(0.0f, 1.0f));
                            combatEvaluator.Apply();

                            // Create a blood splatter effect on the enemy.
                            Game.Singleton.EffectFactory.CreateBloodSplatter(hitCharacter.transform.position, hitCharacter.transform.rotation, hitCharacter.transform, 2.0f);

                            // Find next target
                            if (charactersHit.Count < targets)
                            {
                                List<Character> characters = new List<Character>();
                                Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

                                Character nextTarget = null;
                                if (curRoom.CheckCollisionArea(circle, Character.EScope.Enemy, ref characters))
                                {
                                    foreach (Character c in characters)
                                    {
                                        if (!charactersHit.Contains(c))
                                        {
                                            nextTarget = c;
                                            break;
                                        }
                                    }

                                    if (nextTarget != null)
                                    {
                                        // Move to next target
                                        //projectile.transform.position = collision.gameObject.transform.position;
                                        velocity = nextTarget.transform.position - projectile.transform.position;
                                        projectile.rigidbody.AddForce(velocity, ForceMode.VelocityChange);
                                        hitSomething = false;
                                    }
                                    else
                                    {
                                        // No targets around to just expire
                                        lightningExpired = true;
                                    }
                                }
                            }
                            else
                            {
                                // No targets around to just expire
                                lightningExpired = true;
                            }
                        }
                        else
                        {
                            lightningExpired = true;
                        }

                    }
                    break;
                default:
                    {
                        lightningExpired = true;
                    }
                    break;
            }
        }


        if (lightningExpired)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
