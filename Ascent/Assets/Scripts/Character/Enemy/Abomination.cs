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

	bool isFirstTime = true;

    public override void Initialise()
	{
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
		//AITrigger trigger = null;

		// Charge straight into facing (hopefully hitting something).
		// Immediately switch to other behaviour.
		//behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Passive);
		//{
		//    trigger = behaviour.AddTrigger();
		//    trigger.Operation = AITrigger.EConditionalExit.Stop;
		//    trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[chargeActionID]));
		//    trigger.OnTriggered += StateTransitionToAggressive;
		//}
	
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
			// Charge at the hero.
			chargeTrigger = behaviour.AddTrigger();
			chargeTrigger.Operation = AITrigger.EConditionalExit.Stop;
			chargeTrigger.AddCondition(new AICondition_Timer(1.0f, 1.5f), AITrigger.EConditional.And);
			chargeTrigger.AddCondition(new AICondition_ActionCooldown(loadout.AbilityBinds[chargeActionID]));
			chargeTrigger.OnTriggered += OnCanUseCharge;

			// Attempt to rotate to a Hero. 
			changeTargetTrigger = behaviour.AddTrigger();
			changeTargetTrigger.Operation = AITrigger.EConditionalExit.Continue;
			changeTargetTrigger.AddCondition(new AICondition_Sensor(transform, agent.MindAgent, new AISensor_Sphere(transform, AISensor.EType.Closest, AISensor.EScope.Enemies, 100.0f, Vector3.zero)));
			changeTargetTrigger.OnTriggered += StateTransitionToAggressive;
        }

		StateTransitionToAggressive();
		loadout.UseAbility(chargeActionID);
    }

	public override void StateTransitionToAggressive()
	{
		AIAgent.SteeringAgent.steerTypes = AISteeringAgent.ESteerTypes.Seek;

		SetTarget();

		base.StateTransitionToAggressive();
	}

	public void SetTarget()
	{

		if (AIAgent.MindAgent.SensedCharacters != null && AIAgent.MindAgent.SensedCharacters.Count > 0)
		{
			TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
		}
	}

	public void OnCanUseStomp()
	{
		loadout.UseAbility(stompActionID);
	}

	public void OnCanUseCharge()
	{
		if (loadout.UseAbility(chargeActionID))
		{
			TargetCharacter = null;
			chargeTrigger.Reset();
		}
	}

	public void OnChargeEnd()
	{
		AIAgent.MindAgent.SetBehaviour(AIMindAgent.EBehaviour.Aggressive);
		TargetCharacter = null;
	}

	public void OnCanChangeTarget()
	{
		AIAgent.MindAgent.TargetCharacter = AIAgent.MindAgent.SensedCharacters[0];
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (isFirstTime)
			isFirstTime = false;
		else
			MusicManager.Instance.PlayMusic(MusicManager.MusicSelections.Boss);
	}

    public override void OnDisable()
    {
        base.OnDisable();
		MusicManager.Instance.PlayMusic(MusicManager.MusicSelections.Tower);
        AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);

        motor.StopMotion();
    }

	protected override void PositionHpBar()
	{
		Vector3 screenPos = Game.Singleton.Tower.CurrentFloor.MainCamera.WorldToViewportPoint(transform.position);
        screenPos.y += 0.19f;
        screenPos.x -= 0.12f;
		Vector3 barPos = FloorHUDManager.Singleton.hudCamera.ViewportToWorldPoint(screenPos);
		barPos = new Vector3(barPos.x, barPos.y);
		hpBar.transform.position = barPos;
	}
}
