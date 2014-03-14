using UnityEngine;
using System.Collections;

public class EnvironmentTrigger : MonoBehaviour 
{
    public enum ECondition
    {
        collision,
        activate,
        destroyed,
    }

    public enum ETriggerEffect
    {
        destroyObject,
        activateObject,
    }

    public EnvironmentObj triggerTarget;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
