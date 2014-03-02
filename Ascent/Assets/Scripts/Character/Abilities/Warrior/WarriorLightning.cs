using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorLightning : Ability
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationSpeed = 1.00f;
        animationLength = 0.5f;
        cooldownFullDuration = 0.0f;
        animationTrigger = "Strike";
        specialCost = 0;


        isInstantCast = false;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        CanBeInterrupted = false;

        GameObject lightningGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/Lightning")) as GameObject;
        lightningGO.GetComponent<Lightning>().Initialise(5, owner.transform.position + (owner.transform.forward), owner);

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.Warcry, Warrior.ECombatAnimation.Warcry.ToString());
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

#if UNITY_EDITOR
    public override void DebugDraw()
    {

    }
#endif
}
