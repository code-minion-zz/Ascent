using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class ActRoam : RAIN.Action.Action
{
    private RAIN.Path.RAINPathManager path;
    private bool getNext = true;

    public ActRoam()
    {
        actionName = "ActRoam";
    }

    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
    {
        path = agent.PathManager as RAIN.Path.RAINPathManager;

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
    {
        //if(getNext)
        //{
        //    foreach(Transform t in path.waypointCollection.transform)
        //    {
        //        if(t.name != )
        //        {
        //        }
        //    }
        //}

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
    {
        return RAIN.Action.Action.ActionResult.SUCCESS;
    }
}
