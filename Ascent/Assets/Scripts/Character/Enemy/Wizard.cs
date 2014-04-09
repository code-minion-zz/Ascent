// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Wizard : Enemy
{
    private enum ESpell
    {
        Fireball = 0,
        Lightning,
        Freeze,
        Missile,

        Max,
    }

    private int teleportID;
    private int spellID;

    private AITrigger teleportTrigger;
    private AITrigger spellTrigger;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Rat, this);

        base.Initialise();

        // Add abilities
        loadout.SetSize(2);

        Ability ability = new WizardTeleport();
        teleportID = 0;
        loadout.SetAbility(ability, teleportID);

        ESpell randomSpell = (ESpell)UnityEngine.Random.Range(0, (int)ESpell.Max);

        switch (randomSpell)
        {
            case ESpell.Fireball:
                {
                    ability = new WizardFireball();
                }
                break;
            case ESpell.Lightning:
                {
                    ability = new WizardLightning();
                }
                break;
            case ESpell.Freeze:
                {
                    ability = new WizardFreezeField();
                }
                break;
            case ESpell.Missile:
                {
                    ability = new WizardMagicMissile();
                }
                break;
            default:
                {
                    Debug.LogError("Unhandled case: " + randomSpell);
                }
                break;
        }

        spellID = 1;
        loadout.SetAbility(ability, spellID);

        InitialiseAI();
    }

    public void InitialiseAI()
    {
        AIAgent.SteeringAgent.RotationSpeed = 15.0f;
        AIAgent.SteeringAgent.DistanceToKeepFromTarget = 1.5f;
        motor.MaxSpeed = 0.0f;
        motor.MinSpeed = 0.0f;
        motor.Acceleration = 1.0f;

        AIBehaviour behaviour = null;

        // Defensive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
            teleportTrigger = behaviour.AddTrigger();
            teleportTrigger.Priority = AITrigger.EConditionalExit.Stop;
            teleportTrigger.AddCondition(new AICondition_Attacked(this));
            teleportTrigger.AddCondition(new AICondition_Timer(3.0f, 1.0f, 3.0f), AITrigger.EConditional.Or);
            teleportTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[teleportID]), AITrigger.EConditional.And);
            teleportTrigger.OnTriggered += OnCanUseTeleport;

            spellTrigger = behaviour.AddTrigger();
            spellTrigger.Priority = AITrigger.EConditionalExit.Stop;
            spellTrigger.AddCondition(new AICondition_Timer(1.0f, -1.0f, 1.0f));
            spellTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[spellID]), AITrigger.EConditional.And);
            spellTrigger.OnTriggered += OnCanUseSpell;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

    public void OnCanUseTeleport()
    {
        // Teleport Away
        loadout.UseAbility(teleportID);
        teleportTrigger.Reset();

    }

    public void OnCanUseSpell()
    {
        loadout.UseAbility(spellID);
        spellTrigger.Reset();
    }
}
