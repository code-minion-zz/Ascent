using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class ActChase : RAIN.Action.Action
{
    private GameObject player;

    public ActChase()
    {
        actionName = "ActChase";
    }

    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
    {
        if (actionContext.ContextItemExists("player"))
        {
            player = actionContext.GetContextItem<GameObject>("player");
        }

        if (actionContext.ContextItemExists("seeking"))
        {
            actionContext.SetContextItem("seeking", 1);
        }

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
    {
        if (player != null)
        {
            agent.LookAt(player.transform.position, deltaTime);
            agent.MoveTarget.VectorTarget = player.transform.position;

            if(!agent.Move(deltaTime))
            {
                RAIN.Sensors.RaycastSensor raySensor = agent.GetSensor("Sensor") as RAIN.Sensors.RaycastSensor;
                if (!raySensor.CanSee(player))
                {
                    Debug.Log("cansee");
                    if (actionContext.ContextItemExists("seeking"))
                    {
                        actionContext.SetContextItem("seeking", 0);
                        agent.Mind.CancelInvoke("Move");
                        agent.Mind.Reset();

                        return ActionResult.SUCCESS;
                    }
                }

                return ActionResult.RUNNING;
            }
        }
        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
    {
        return RAIN.Action.Action.ActionResult.SUCCESS;
    }
}
