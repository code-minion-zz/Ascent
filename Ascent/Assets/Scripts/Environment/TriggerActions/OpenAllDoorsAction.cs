using UnityEngine;
using System.Collections;

public class OpenAllDoorsAction : EnvironmentAction 
{
	public Doors doors;

	private bool initialised;

	void OnEnable()
	{
		initialised = true;
	}

	public override void ExecuteAction()
	{
		if (initialised)
		{
			foreach (Door d in doors.RoomDoors)
			{
				if (d.IsOpen == false)
				{
					d.OpenDoor();
				}
			}
		}

	}
}
