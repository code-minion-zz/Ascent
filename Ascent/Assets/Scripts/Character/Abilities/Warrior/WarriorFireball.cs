using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorFireball : Ability
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationSpeed = 1.00f;
        animationLength = 1.067f;
        cooldownFullDuration = 0.0f;
        animationTrigger = "Strike";
        specialCost = 0;

        isInstantCast = false;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        GameObject fireballGO =  GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/Fireball")) as GameObject;
        fireballGO.GetComponent<Fireball>().Initialise(owner.transform.position, owner.transform.forward, owner);
    }

    public override void StartCast()
    {
        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.ChargeCrouch, Warrior.ECombatAnimation.Charge.ToString());
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        Debug.Log("DONT");
        ((HeroAnimator)Owner.Animator).CombatAnimationEnd();
        base.EndAbility();
    }

#if UNITY_EDITOR
    public override void DebugDraw()
    {
       
    }
#endif
}
