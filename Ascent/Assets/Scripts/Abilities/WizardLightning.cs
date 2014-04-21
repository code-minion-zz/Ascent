using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WizardLightning : Ability
{
    private bool performed = false;
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 0.5f;
        animationSpeed = 1.0f;
        animationTrigger = "Beam";
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
            GameObject lightningGO = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/Lightning")) as GameObject;
            lightningGO.GetComponent<Lightning>().Initialise(5, owner.transform.position + (owner.transform.forward), owner);

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
