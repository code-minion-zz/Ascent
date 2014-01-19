using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AITrigger  
{
    public enum EConditional
    {
        And,
        Or,
        Else,
        None,
    }

    public enum EConditionalExit
    {
        Continue,
        Stop,
    }

    private EConditionalExit priority;
    public EConditionalExit Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    protected List<KeyValuePair<AICondition, EConditional>> conditions = new List<KeyValuePair<AICondition, EConditional>>();
	public List<KeyValuePair<AICondition, EConditional>> Conditions
	{
		get { return conditions; }
	}
 
    public delegate void ConditionTriggered();
    public event ConditionTriggered OnTriggered;

    public void AddCondition(AICondition c)
    {
        conditions.Add(new KeyValuePair<AICondition, EConditional>(c, EConditional.And));
    }

    public void AddCondition(AICondition c, EConditional condition)
    {
        conditions.Add(new KeyValuePair<AICondition, EConditional>(c, condition));
    }

    public bool HasTriggered()
    {
        bool triggered = true;
        foreach (KeyValuePair<AICondition, EConditional> c in conditions)
        {
            bool conditionMet = c.Key.HasBeenMet();

            if (c.Value == EConditional.None)
            {
                triggered = conditionMet;
            }
            else if (c.Value == EConditional.And)
            {
                triggered = (triggered && conditionMet);
            }
            else if (c.Value == EConditional.Or)
            {
                triggered = (triggered || conditionMet);
            }
        }

        if (triggered)
        {
            if (OnTriggered != null)
            {
                OnTriggered.Invoke();
            }
        }

        return triggered;
    }

    public void Reset()
    {
        foreach (KeyValuePair<AICondition, EConditional> c in conditions)
        {
            c.Key.Reset();
        }
    }

#if UNITY_EDITOR
    public void DebugDraw()
    {
        foreach (KeyValuePair<AICondition, EConditional> c in conditions)
        {
            c.Key.DebugDraw();
        }
    }
#endif
}
