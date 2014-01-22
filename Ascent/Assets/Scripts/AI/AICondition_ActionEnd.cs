using UnityEngine;
using System.Collections;

public class AICondition_ActionEnd : AICondition 
{
    bool actionEnded;
	string name;

    public AICondition_ActionEnd(Action action)
    {
        actionEnded = false;
        action.OnActionEnd += OnActionEnd;
		name = action.AnimationTrigger;
    }

    public override void Reset()
    {
        actionEnded = false;
    }

    public override bool HasBeenMet()
    {
        bool hasBeenMet = actionEnded;

        actionEnded = false;

        return hasBeenMet;
    }

    public void OnActionEnd()
    {
        actionEnded = true;
    }

	public override string ToString()
	{
		return "ActionEnded(" + name + "): "+ actionEnded;
	}
}
