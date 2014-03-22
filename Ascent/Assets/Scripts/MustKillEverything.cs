using UnityEngine;
using System.Collections;

public class MustKillEverything : MonoBehaviour 
{

	public Doors doors;
	public Enemy[] enemies;

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

		int total = enemies.Length;
		int accum = 0;

		for (int i = 0; i < total; ++i)
		{
			if (enemies[i].IsDead)
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
