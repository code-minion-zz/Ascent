// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Abomination : Enemy
{
    private AITrigger changeTargetTrigger;
	private AITrigger chargeTrigger;

    private int strikeActionID;
    private int stompActionID;
    private int chargeActionID;

    public override void Initialise()
    {
        EnemyStats = EnemyStatLoader.Load(EEnemy.Abomination, this);

        base.Initialise();

        // Add abilities
        loadout.SetSize(3);

        Ability strike = new AbominationStrike();
        strike.Initialise(this);
        strikeActionID = 0;
        loadout.SetAbility(strike, strikeActionID);

        Ability stomp = new AbominationStomp();
        stomp.Initialise(this);
        stompActionID = 1;
        loadout.SetAbility(stomp, stompActionID);


        Ability charge = new AbominationCharge();
        charge.Initialise(this);
        chargeActionID = 2;
        loadout.SetAbility(charge, chargeActionID);


        InitialiseAI();

		vulnerabilities = EStatus.None;
    }

    public void InitialiseAI()
    {

        AIBehaviour behaviour = null;

		// Charge straight into facing (hopefully hitting something).
		// Immediately switch to other behaviour.
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Defensive);
        {
            AITrigger trigger = behaviour.AddTrigger();
            trigger.Operation = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[chargeActionID]));
			trigger.OnTriggered += OnChargeEnd;

			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
            trigger.AddCondition(new AICondition_Timer(1.0f, 0.0f, 0.0f));
			trigger.OnTriggered += OnCanUseCharge;
        }

        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
			AITrigger trigger = null;

			// Back out from the collision point.
			trigger = behaviour.AddTrigger();
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[chargeActionID]));
			trigger.OnTriggered += OnChargeEnd;

			//// Do the stomp first to bring boulders down from the roof.
			//trigger = behaviour.AddTrigger();
			//trigger.Priority = AITrigger.EConditionalExit.Stop;
			//trigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[stompActionID]));
			//trigger.OnTriggered += OnCanUseStomp;

			// Attempt to rotate to a Hero. 
			changeTargetTrigger = behaviour.AddTrigger();
			changeTargetTrigger.Operation = AITrigger.EConditionalExit.Continue;
			changeTargetTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.FirstFound, AISensor.EScope.Enemies, 100.0f, Vector3.zero)));
			//ChangeTargetTrigger.AddCondition(new AICondition_Attacked(this));
			changeTargetTrigger.OnTriggered += OnCanChangeTarget;

			// Charge at the hero.
			chargeTrigger = behaviour.AddTrigger();
			chargeTrigger.Operation = AITrigger.EConditionalExit.Stop;
			chargeTrigger.AddCondition(new AICondition_Timer(0.5f, 1.0f, 2.0f), AITrigger.EConditional.And);
			chargeTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[chargeActionID]));
			chargeTrigger.OnTriggered += OnCanUseCharge;
        }

        AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Defensive);
    }

	public void OnCanUseStomp()
	{
		loadout.UseAbility(stompActionID);
	}

	public void OnCanUseCharge()
	{
		if (loadout.UseAbility(chargeActionID))
		{
			chargeTrigger.Reset();
		}
	}

	public void OnChargeEnd()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
	}

	public void OnCanChangeTarget()
	{
		AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
	}

	public override void OnEnable()
	{
		//MusicManager.Instance.PlayMusic(MusicManager.MusicSelections.Boss);
	}

    public override void OnDisable()
    {
		//MusicManager.Instance.PlayMusic(MusicManager.MusicSelections.Tower);
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);

        motor.StopMotion();
    }
}
