using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WizardFreezeField : Ability
{
    private bool performed = false;
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 0.5f;
        animationSpeed = 1.0f;
        animationTrigger = "Strike";
        cooldownFullDuration = 2.1f;
        specialCost = 0;
    }

    public override void StartAbility()
    {
        base.StartAbility();

        owner.Motor.StopMotion();
        owner.Motor.EnableStandardMovement(false);
        owner.SetColor(Color.red);
        performed = false;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (timeElapsedSinceStarting >= animationLength * 0.5f && !performed)
        {
            GameObject fireballGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/FreezeField")) as GameObject;
            fireballGO.GetComponent<FreezeField>().Initialise(0, owner.transform.position, owner);
            performed = true;
            owner.ResetColor();
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Motor.EnableStandardMovement(true);
    }
}
