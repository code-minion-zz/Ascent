using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorLightning : Ability
{
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationSpeed = 1.5f;
        animationLength = 0.8f;
        cooldownFullDuration = 0.0f;
        animationTrigger = "Strike";
        specialCost = 3;


        isInstantCast = true;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        CanBeInterrupted = false;

        GameObject lightningGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/Lightning")) as GameObject;
		lightningGO.GetComponent<Lightning>().Initialise(5, 3, owner.transform.position + (owner.transform.forward), owner);
		//SoundManager.PlaySound(AudioClipType.lightning, owner.transform.position, 0.1f);

        ((HeroAnimator)Owner.Animator).PlayCombatAction((int)Warrior.ECombatAnimation.Warcry, Warrior.ECombatAnimation.Warcry.ToString());

		EffectFactory.Singleton.CreateLightningCastCircle(owner.transform.position, owner.transform.rotation);
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
