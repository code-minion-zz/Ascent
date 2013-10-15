using UnityEngine;
using System.Collections;

public class ActPatrolWaypoints : RAIN.Action.Action 
{
    // Problem: How do I make the initial values customisable???

    // This Action will make the Agent walk through a collection of waypoints
    // Close Enough Distance and Close Enough Angle might need to be increased to make it work

    private RAIN.Path.RAINPathManager path;
    Transform[] waypoints;
    int currentWaypoint = 1;

    public ActPatrolWaypoints()
    {
        actionName = "ActPatrolWaypoints";
    }

    public override RAIN.Action.Action.ActionResult Start(RAIN.Core.Agent agent, float deltaTime)
    {
        // Grab path
        path = agent.PathManager as RAIN.Path.RAINPathManager;

        // Grab path waypoints
        waypoints = path.waypointCollection.GetComponentsInChildren<Transform>();

        // Set target to the first waypoint
        SetTarget(agent);

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Execute(RAIN.Core.Agent agent, float deltaTime)
    {
        // Check if AI has reached the target
        if (!agent.MoveTo(agent.LookTarget.TransformTarget.position, deltaTime))
        {
            return RAIN.Action.Action.ActionResult.RUNNING;
        }
        else
        {
            // Target has been reached so set the next one
            ++currentWaypoint;
            if (currentWaypoint > waypoints.Length)
            {
                currentWaypoint = 1;
            }

            // Set target to the next waypoint
            SetTarget(agent);
        }

        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    public override RAIN.Action.Action.ActionResult Stop(RAIN.Core.Agent agent, float deltaTime)
    {
        return RAIN.Action.Action.ActionResult.SUCCESS;
    }

    private void SetTarget(RAIN.Core.Agent agent)
    {
        // Get children does not give an ordered list of waypoints
        // We need to check against the names to make sure the waypoints are Cycled correctly

        foreach (Transform t in waypoints)
        {
            if(t.name == currentWaypoint.ToString())
            {
                Debug.Log(t.name);
                agent.LookTarget.TransformTarget = t;
                return;
            }
        }
    }
}
