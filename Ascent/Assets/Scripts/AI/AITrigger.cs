using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
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

	public AITrigger()
	{
	}

	public AITrigger(string name)
	{
		this.name = name;
	}

    [SerializeField]
	public string name;

    [SerializeField]
    private EConditionalExit operation;
    public EConditionalExit Operation
    {
        get { return operation; }
        set { operation = value; }
    }

    protected List<KeyValuePair<AICondition, EConditional>> conditions = new List<KeyValuePair<AICondition, EConditional>>();
	public List<KeyValuePair<AICondition, EConditional>> Conditions
	{
		get { return conditions; }
	}


    public List<AIConditionType> AIConditionType = new List<AIConditionType>();
 
    [SerializeField]
    public delegate void ConditionTriggered();
    [SerializeField]
    public event ConditionTriggered OnTriggered;
    [SerializeField]
    public EventDelegate onTriggered = new EventDelegate();

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
