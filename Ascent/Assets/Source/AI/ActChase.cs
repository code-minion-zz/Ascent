using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class ActChase : RAIN.Action.Action
{
    private Vector3 agentPos = Vector3.zero;
    private Vector3 moveToLocation = Vector3.zero;
    private Vector3 directionToRun = Vector3.zero;
    private GameObject targetToAvoid;

    public ActChase()
    {
        actionName = "ActChase";
    }

    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
    {
        agentPos = agent.Avatar.transform.position;

        Mind mind = agent.Mind;
        mind.GetObjectWithAspect("asd", out targetToAvoid);

        if (targetToAvoid != null && moveToLocation == Vector3.zero)
        {
            directionToRun = agentPos - targetToAvoid.transform.position;
            moveToLocation = agentPos - directionToRun;
        }

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
    {
        // Keep following the player
        if (!agent.MoveTo(moveToLocation, deltaTime))
        {
            return ActionResult.RUNNING;
        }
        else
        {
            moveToLocation = Vector3.zero;
        }

        // Temp
        //RAIN.Sensors.RaycastSensor s = (RAIN.Sensors.RaycastSensor)agent.GetSensor("Sensor");
        //if(!s.CanSee())
        //{
            // Do stuff
            //agent.Mind.CancelInvoke("Move");
            //agent.Mind.Reset();
        //}

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
    {
        return RAIN.Action.Action.ActionResult.SUCCESS;
    }
}
