using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AIBehaviour  
{
    [SerializeField]
    protected List<AITrigger> triggers = new List<AITrigger>();
	public List<AITrigger> Triggers
	{
		get { return triggers; }
	}

    [SerializeField]
	protected AIMindAgent.EBehaviour type = AIMindAgent.EBehaviour.Defensive;
	public AIMindAgent.EBehaviour Type
	{
		get { return type; }
	}

	public AIBehaviour(AIMindAgent.EBehaviour e)
	{
		type = e;
	}

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

	public AITrigger AddTrigger(string name)
	{
		AITrigger t = new AITrigger(name);
		triggers.Add(t);
		return t;
	}

    public void Process()
    {
        foreach (AITrigger t in triggers)
        {
            if (t.HasTriggered())
            {
                if (t.Operation == AITrigger.EConditionalExit.Stop)
                {
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
	public void OnDrawGizmos()
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
