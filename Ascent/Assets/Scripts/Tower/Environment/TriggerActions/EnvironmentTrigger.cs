using UnityEngine;
using System.Collections;

public class EnvironmentTrigger : MonoBehaviour 
{
	protected bool activated;
	public bool repeatable;
	public EnvironmentAction action;
	public EnvironmentAction falseAction;

	public virtual void Update()
	{
		// Has trigger been met AND it hasn't been met yet (OR it has been met before but it can be repeated)
		bool met = HasTriggerBeenMet();

		if (met && (!activated || (activated && repeatable)))
		{
			PerformAction();
		}
		else if(!met)
		{
			PerformFalseAction();
		}
	}

	protected virtual bool HasTriggerBeenMet()
	{
		// To be derived
		return false;
	}

	protected void PerformAction()
	{
		activated = true;

		if (action != null)
		{
			action.ExecuteAction();
		}
		else
		{
			Debug.LogError("No action assigned to this environment trigger.");
		}
	}

	protected void PerformFalseAction()
	{
		if (falseAction != null)
		{
			activated = false;
			falseAction.ExecuteAction();
		}
	}
}
