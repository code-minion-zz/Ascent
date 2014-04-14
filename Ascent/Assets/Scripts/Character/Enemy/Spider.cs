using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spider : Enemy
{
    private int shootID;

    public override void Initialise()
    {
        base.Initialise();

        // Add abilities
        loadout.SetSize(1);

        Ability tackle = new RatTackle();
        shootID = 0;
        loadout.SetAbility(tackle, shootID);

        InitialiseAI();
    }

    public void InitialiseAI()
    {
        //AIAgent.SteeringAgent.RunIfTooClose = true;
        motor.MaxSpeed = 3.0f;
        motor.MinSpeed = 0.5f;
        motor.Acceleration = 1.0f;

        AIBehaviour behaviour = null;
        AITrigger trigger = null;

        // Aggressive
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
            // Change target to closest
            trigger = behaviour.AddTrigger();
            trigger.Operation = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 4.5f, Vector3.zero)));
            trigger.OnTriggered += ChangeTarget;

            //// Shoot if can shoot
            //trigger = behaviour.AddTrigger();
            //trigger.Priority = AITrigger.EConditionalExit.Stop;
            //trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[shootID]));
            //trigger.OnTriggered += Shoot;

            //// Move close but Keep distance
            //trigger = behaviour.AddTrigger();
            //trigger.Priority = AITrigger.EConditionalExit.Stop;
            //trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[shootID]));
            //trigger.OnTriggered += Move;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        //AIAgent.SteeringAgent.SetTargetPosition(containedRoom.NavMesh.GetRandomOrthogonalPositionWithinRadius(transform.position, 7.5f));
    }

    public void ChangeTarget()
    {

        Debug.Log("SENSE");
        AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
    }

    //public void Shoot()
    //{
    //    loadout.UseAbility(shootID);
    //}

    //public void Move()
    //{
    //    AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    //    AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);

    //    if (lastDamagedBy != null)
    //    {
    //        AIAgent.MindAgent.TargetCharacter = lastDamagedBy;
    //    }
    //    else if (AIAgent.MindAgent.SensedCharacters != null && AIAgent.MindAgent.SensedCharacters.Count > 0)
    //    {
    //        AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
    //    }
    //}

    public void OnCanUseTackle()
    {
        motor.LookAt(AIAgent.MindAgent.TargetCharacter.transform.position);
        //AIAgent.SteeringAgent.RemoveTarget();
        motor.StopMotion();
        loadout.UseAbility(shootID);
    }

    public override void OnDisable()
    {
        motor.StopMotion();
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
        AIAgent.MindAgent.TargetCharacter = null;
        //AIAgent.SteeringAgent.RemoveTarget();
    }
}
