using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WizardTeleport : Ability
{
    private bool performed = false;
    public override void Initialise(Character owner)
    {
        base.Initialise(owner);

        animationLength = 0.5f;
        animationSpeed = 1.0f;
        animationTrigger = "Strike";
        cooldownFullDuration = 5.0f;
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
            Room curRoom = Game.Singleton.Tower.CurrentFloor.CurrentRoom;

            owner.transform.position = curRoom.NavMesh.GetRandomPosition();
            owner.transform.rotation = Quaternion.LookRotation(FloorCamera.CalculateAverageHeroPosition() - owner.transform.position, Vector3.up);
           
            owner.ResetColor();

            performed = true;
        }
    }

    public override void EndAbility()
    {
        base.EndAbility();
        owner.Motor.EnableStandardMovement(true);
    }
}
