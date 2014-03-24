using UnityEngine;
using System.Collections;

public class OpenDoorAction : EnvironmentAction 
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
				d.OpenDoor();
			}
			//initialised = false;
		}

	}
}
