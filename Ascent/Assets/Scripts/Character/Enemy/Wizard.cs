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
        AIBehaviour behaviour = null;

        // Defensive behaviour
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
			AITrigger trigger = behaviour.AddTrigger("Select closest target in range.");
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			trigger.AddCondition(new AICondition_Sensor(transform, AIAgent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 10.0f, Vector3.zero)), AITrigger.EConditional.And);
			trigger.OnTriggered += TargetInRange;

            teleportTrigger = behaviour.AddTrigger("Teleport if it is on CD or attacked");
            teleportTrigger.Operation = AITrigger.EConditionalExit.Stop;
            teleportTrigger.AddCondition(new AICondition_Attacked(this));
            teleportTrigger.AddCondition(new AICondition_Timer(4.0f, 7.0f), AITrigger.EConditional.Or);
            teleportTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[teleportID]), AITrigger.EConditional.And);
            teleportTrigger.OnTriggered += OnCanUseTeleport;

            spellTrigger = behaviour.AddTrigger("Use spell after random timer.");
            spellTrigger.Operation = AITrigger.EConditionalExit.Stop;
            spellTrigger.AddCondition(new AICondition_Timer(0.0f, 2.0f));
            spellTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[spellID]), AITrigger.EConditional.And);
            spellTrigger.OnTriggered += OnCanUseSpell;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

	public override void Update()
	{
		base.Update();

		if(TargetCharacter != null && !isDead)
		{
			float speed = 1.5f;
			Vector3 targetDir = TargetCharacter.transform.position - transform.position;
			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	public void TargetInRange()
	{
		TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
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
