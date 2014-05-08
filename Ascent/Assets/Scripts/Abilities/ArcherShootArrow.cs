using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherShootArrow : Ability
{
    private bool performed = false;
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 2.167f;
        animationSpeed = 2.0f;
        animationTrigger = "Strike";
        cooldownFullDuration = 0.0f;
        specialCost = 0;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        owner.Motor.StopMotion();
        owner.Motor.EnableStandardMovement(false);
        owner.SetColor(Color.red);
		performed = false;
		SoundManager.PlaySound(AudioClipType.shootFire,owner.transform.position,.1f);

		owner.Animator.PlayAnimation(animationTrigger, true);
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (timeElapsedSinceStarting >= animationLength * 0.75f && !performed)
        {
            GameObject arrowGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/Archer/ArcherArrow")) as GameObject;
            arrowGO.GetComponent<ArcherArrow>().Initialise(owner.transform.position + owner.transform.forward, owner.transform.forward * 10.0f, owner);
            performed = true;
            owner.ResetColor();
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Motor.EnableStandardMovement(true);
		owner.Animator.PlayAnimation(animationTrigger, false);
    }
}
