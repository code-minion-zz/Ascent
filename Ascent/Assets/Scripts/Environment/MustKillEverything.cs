using UnityEngine;
using System.Collections;

public class MustKillEverything : MonoBehaviour 
{

	public Doors doors;
	public Enemy[] enemies;

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
