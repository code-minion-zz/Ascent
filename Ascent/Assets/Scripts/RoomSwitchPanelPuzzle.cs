using UnityEngine;
using System.Collections;

public class RoomSwitchPanelPuzzle : MonoBehaviour 
{
	public Doors doors;
	public SwitchPanel[] switches;

	bool enabled = false;

	void OnEnable()
	{
		enabled = true;
	}

	void Update()
	{
		if (enabled)
		{
			foreach (Door d in doors.RoomDoors)
			{
				d.CloseDoor();
			}
			enabled = false;
		}

		int total = switches.Length;
		int accum = 0;

		for (int i = 0; i < total; ++i)
		{
			if (switches[i].isDown)
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
