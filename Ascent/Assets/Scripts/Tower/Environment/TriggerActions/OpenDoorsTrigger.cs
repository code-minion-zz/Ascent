using UnityEngine;
using System.Collections;

public class OpenDoorsTrigger : EnvironmentAction 
{
    public Door[] doors;

    private bool initialised;

    void OnEnable()
    {
        initialised = true;
    }

    public override void ExecuteAction()
    {
        if (initialised)
        {
            foreach (Door d in doors)
            {
                d.OpenDoor();
            }
        }
    }
}
