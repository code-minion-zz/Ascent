using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapActivationAction : EnvironmentAction
{
    public List<EnvironmentHazard> listHazzards;

    void OnEnable()
    {
    }

    public override void ExecuteAction()
    {
        foreach (EnvironmentHazard hazard in listHazzards)
        {
            hazard.ActivateHazard();
        }
    }
}
