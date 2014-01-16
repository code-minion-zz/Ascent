using UnityEngine;
using System.Collections;

public class AICondition_ReachedTarget : AICondition
{
    private AISteeringAgent steerAgent;
    private bool targetReached = false;

    public AICondition_ReachedTarget(AISteeringAgent steerAgent)
    {
        this.steerAgent = steerAgent;
        steerAgent.OnTargetReached += OnTargetReached;
    }

    public override bool HasBeenMet()
    {
        bool hasBeenMet = targetReached;

        targetReached = false;

        return hasBeenMet;
    }

    public void OnTargetReached()
    {
        targetReached = true;
    }

}
