using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour 
{
	public Doors doors;

	void OnTriggerEnter(Collider other)
	{
		foreach (Door d in doors.RoomDoors)
		{
			d.CloseDoor();
		}
	}
}

