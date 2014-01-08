using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour 
{
	protected TriggerRegion triggerRegion;
	public TriggerRegion TriggerRegion
	{
		get { return triggerRegion; }
	}

	public virtual void Start()
	{
		triggerRegion = GetComponent<TriggerRegion>();
	}
}
