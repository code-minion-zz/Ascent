using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorFreezeField : Ability
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationSpeed = 1.00f;
        animationLength = 0.5f;
        cooldownFullDuration = 2.0f;
        animationTrigger = "Strike";
        specialCost = 3;


        isInstantCast = false;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        CanBeInterrupted = false;

        GameObject frostFieldGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/FreezeField")) as GameObject;
        frostFieldGO.GetComponent<FreezeField>().Initialise(0, owner.transform.position + new Vector3(0.0f, 1.0f, 0.0f), owner);

        // Destroy after 5 seconds allowing plenty of time for the animation.
        GameObject.Destroy(frostFieldGO, 5.0f);

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.Warcry, Warrior.ECombatAnimation.Warcry.ToString());

		SoundManager.PlaySound(AudioClipType.freezeBlast, owner.transform.position, 1f);
    }

    public override void StartCast()
    {
    }

    public override void UpdateAbility()
    {
        if (timeElapsedSinceStarting >= animationLength * 0.75f)
        {
            CanBeInterrupted = true;
        }

        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        ((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
    }
}
