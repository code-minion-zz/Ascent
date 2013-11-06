//using UnityEngine;
//using System.Collections;
//using RAIN.Core;
//using RAIN.Action;

//public class ActRoam : RAIN.Action.Action
//{
//    private RAIN.Path.RAINPathManager path;
//    Transform[] waypoints;
//    private GameObject player;
//    int currentWaypoint = 1;

//    public ActRoam()
//    {
//        actionName = "ActRoam";
//    }

//    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
//    {
//        // TODO: Find out if start is only called once... else find out what the init code is.
//        // TODO: Handle errors here

//        path = agent.PathManager as RAIN.Path.RAINPathManager;

//        // This seems to include self
//        waypoints = path.waypointCollection.GetComponentsInChildren<Transform>();

//        //// Looks up variables that are used in RAIN BT. The types need to be casted to the obvious thing.
//        //if(actionContext.ContextItemExists("player"))
//        //{
//        //    player = actionContext.GetContextItem<GameObject>("player");
//        //}

//        //// Can be used to look up anything really
//        //if (actionContext.ContextItemExists("canSee"))
//        //{
//        //    //actionContext.SetContextItem<int>("canSee", 1);
//        //    actionContext.SetContextItem("canSee", 1);
//        //}

//        SetTarget(agent);

//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }

//    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
//    {
//        if (!agent.MoveTo(agent.LookTarget.TransformTarget.position, deltaTime))
//        {
//            return RAIN.Action.Action.ActionResult.RUNNING;
//        }
//        else
//        {
//            ++currentWaypoint;
//            if (currentWaypoint > waypoints.Length)
//            {
//                currentWaypoint = 1;
//            }
//            SetTarget(agent);
//            Debug.Log(currentWaypoint);
//        }

//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }

//    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
//    {
//        return RAIN.Action.Action.ActionResult.SUCCESS;
//    }

//    private void SetTarget(RAIN.Core.Agent agent)
//    {
//        foreach (Transform t in waypoints)
//        {
//            if(t.name == currentWaypoint.ToString())
//            {
//                agent.LookTarget.TransformTarget = t;
//                return;
//            }
//        }
//    }
//}
