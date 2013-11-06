//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using RAIN.Core;
//using RAIN.Action;

//public class CustomAction : RAIN.Action.Action
//{
//    private Vector3 agentPos = Vector3.zero;
//    private Vector3 moveToLocation = Vector3.zero;
//    private Vector3 directionToRun = Vector3.zero;
//    private GameObject targetToAvoid;

//    public CustomAction()
//    {
//        actionName = "CustomAction";
//    }

//    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
//    {
//        agentPos = agent.Avatar.transform.position;

//        Mind mind = agent.Mind;
//        mind.GetObjectWithAspect("asd", out targetToAvoid);

//        if (targetToAvoid != null && moveToLocation == Vector3.zero)
//        {
//            directionToRun = agentPos - targetToAvoid.transform.position;
//            moveToLocation = agentPos - directionToRun;
//        }

//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }

//    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
//    {
//        if (!agent.MoveTo(moveToLocation, deltaTime))
//        {
//            return ActionResult.RUNNING;
//        }
//        else
//        {
//            moveToLocation = Vector3.zero;
//        }

//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }

//    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
//    {
//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }
//}