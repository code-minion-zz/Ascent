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
				if (d == null)
					continue;

				if (d.IsOpen == false)
				{
                	d.OpenDoor();
				}
            }
        }
    }
}
