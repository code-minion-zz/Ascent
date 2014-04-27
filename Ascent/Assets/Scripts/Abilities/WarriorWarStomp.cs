using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorWarStomp : Ability
{
    private const float explosionMaxRadius = 10.0f;
    private GameObject stompObject;
    private GameObject prefab;
    private bool performed;
	private bool soundPlayed;

    public float radius = 3.0f;
    public float knockBack = 10.0f;
    public int damage = 0;

    private Circle collisionShape;

    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        // TODO: remove this from hardcoded animation data.
        animationLength = 1.667f;
        animationSpeed = 1.5f;
        animationTrigger = "WarStomp";
        cooldownFullDuration = 0.0f;
        specialCost = 3;

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
		soundPlayed = false;

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.WarStromp, Warrior.ECombatAnimation.WarStromp.ToString());
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
			if (timeElapsedSinceStarting >= animationLength * 0.3f)
			{
				if (!soundPlayed)
				{
					soundPlayed = true;
					SoundManager.PlaySound(AudioClipType.earthshock, owner.transform.position, 1f);
				}
			}

            if (timeElapsedSinceStarting >= animationLength * 0.5f)
            {
                List<Character> enemies = new List<Character>();

				if (Game.Singleton.InTower)
				{
					Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;
					if (curRoom.CheckCollisionArea(collisionShape, Character.EScope.Enemy, ref enemies))
					{
						foreach (Enemy e in enemies)
						{
							CombatEvaluator combatEvaluator = new CombatEvaluator(owner, e);
							combatEvaluator.Add(new PhysicalDamageProperty(1.0f, 0.5f));
							combatEvaluator.Add(new StatusEffectCombatProperty(new StunnedDebuff(owner, e, 1.0f)));
							combatEvaluator.Apply();

							// Create a blood splatter effect on the enemy.
                            EffectFactory.Singleton.CreateBloodSplatter(e.transform.position, e.transform.rotation);
						}
					}

					curRoom.ProcessCollisionBreakables(collisionShape);
				}

                performed = true;
            }
        }
    }

    public override void EndAbility()
    {
        ((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
    }

	public override void StartCast()
	{
		base.StartCast ();
	}


#if UNITY_EDITOR
    public override void DebugDraw()
    {
        collisionShape.DebugDraw();
    }
#endif
}
