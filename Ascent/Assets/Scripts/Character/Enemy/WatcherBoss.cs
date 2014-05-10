// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WatcherBoss : Enemy
{
    private int lazerID;
    private int magicMissilesID;

    public Transform mainEye;
    public Transform[] eyes;

	AICondition_Timer missileTimer;
	AICondition_Timer lazerTimer;

	WatcherMagicMissile magicMissileAbility;
	WatcherLazerBeam lazerBeamAbility;

    public override void Initialise()
    {
        base.Initialise();

        // Add abilities
        loadout.SetSize(2);

		magicMissileAbility = new WatcherMagicMissile();
        magicMissilesID = 0;
		loadout.SetAbility(magicMissileAbility, magicMissilesID);

		lazerBeamAbility = new WatcherLazerBeam();
		lazerID = 1;
		loadout.SetAbility(lazerBeamAbility, lazerID);

        InitialiseAI();


		vulnerabilities = EStatus.None;
    }

    public void InitialiseAI()
    {
        AIBehaviour behaviour = null;
        AITrigger trigger = null;

        // Aggressive
        behaviour = AIAgent.MindAgent.AddBehaviour(AIMindAgent.EBehaviour.Aggressive);
        {
			trigger = behaviour.AddTrigger("Reset timer after MagicMissiles ends.");
			trigger.Operation = AITrigger.EConditionalExit.Continue;
			trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[magicMissilesID]));
			trigger.OnTriggered += delegate() { missileTimer.Reset(); };

			trigger = behaviour.AddTrigger("Use MagicMissiles after random timer.");
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			missileTimer = new AICondition_Timer(1.5f, 2.5f);
			trigger.AddCondition(missileTimer);
			trigger.OnTriggered += UseMagicMissile;

			trigger = behaviour.AddTrigger("Reset timer after Lazer ends.");
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			trigger.AddCondition(new AICondition_ActionEnd(loadout.AbilityBinds[lazerID]));
			trigger.OnTriggered += delegate() { lazerTimer.Reset(); };

			trigger = behaviour.AddTrigger("Use Lazer after random timer.");
			trigger.Operation = AITrigger.EConditionalExit.Stop;
			lazerTimer = new AICondition_Timer(7.5f, 12.0f);
			trigger.AddCondition(lazerTimer);
			trigger.OnTriggered += UseLazer;
        }

        StateTransitionToAggressive();
    }

	protected override void Enrage()
	{
		base.Enrage();

		missileTimer.Reset(1.0f, 1.5f);
		lazerTimer.Reset(5.0f, 8.0f);

		lazerBeamAbility.Enrage();
		magicMissileAbility.Enrage();
	}

    public override void StateTransitionToPassive()
    {
        base.StateTransitionToPassive();
		AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Passive);
    }

    public override void StateTransitionToAggressive()
    {
        base.StateTransitionToAggressive();
		AIAgent.MindAgent.ResetBehaviour(AIMindAgent.EBehaviour.Aggressive);
    }

    public void UseMagicMissile()
    {
        loadout.UseAbility(magicMissilesID);
    }

	public void UseLazer()
	{
		loadout.UseAbility(lazerID);
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
