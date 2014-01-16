using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIBehaviour  
{
    protected List<AITrigger> triggers = new List<AITrigger>();

    public void AddTrigger(AITrigger t)
    {
        triggers.Add(t);
    }

    public AITrigger AddTrigger()
    {
        AITrigger t = new AITrigger();
        triggers.Add(t);
        return t;
    }

    public void Process()
    {
        foreach (AITrigger t in triggers)
        {
            if (t.HasTriggered())
            {
                if (t.Priority == AITrigger.EConditionalExit.Stop)
                {
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
    public void OnGizmosDraw()
    {
        DebugDraw();
    }
#endif

    public void Reset()
    {
        foreach (AITrigger t in triggers)
        {
            t.Reset();
        }
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
        foreach (AITrigger t in triggers)
        {
            t.DebugDraw();
        }
    }
#endif
}
