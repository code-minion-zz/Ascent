using UnityEngine;
using System.Collections;

public class RoomSwitchPanelPuzzle : MonoBehaviour 
{
	public Doors doors;
	public SwitchPanel[] switches;

	bool initialised = false;

	void OnEnable()
	{
        initialised = true;
	}

	void Update()
	{
        if (initialised)
		{
			foreach (Door d in doors.RoomDoors)
			{
				d.CloseDoor();
			}
            initialised = false;
		}

		int total = switches.Length;
		int accum = 0;

		for (int i = 0; i < total; ++i)
		{
			if (switches[i].IsDown)
			{
				accum++;
			}
		}

		if (accum == total)
		{
			foreach (Door d in doors.RoomDoors)
			{
				d.OpenDoor();
			}
		}
		else
		{
			foreach (Door d in doors.RoomDoors)
			{
				d.CloseDoor();
			}
		}
	}
}
