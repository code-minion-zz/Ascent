using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Doors : MonoBehaviour 
{
	private const int maxDoors = 4;
    private List<Door> roomDoors = new List<Door>();

    public List<Door> RoomDoors
    {
        get { return roomDoors; }
        set { roomDoors = value; }
    }

	[HideInInspector]
	public int hiddenDoorCount;
	[HideInInspector]
	public int lockedDoorCount;

	public void Start () 
	{
		Door[] foundDoors = GetComponentsInChildren<Door>() as Door[];
		foreach (Door d in foundDoors)
		{
            RoomDoors.Add(d);

			if (d is HiddenDoor)
			{
				++hiddenDoorCount;
			}
			else if(d is LockedDoor)
			{
				++lockedDoorCount;
			}
		}
	}

	public HiddenDoor[] HiddenDoors
	{
		get 
		{
			if (hiddenDoorCount > 0)
			{
				HiddenDoor[] hiddenDoors = new HiddenDoor[hiddenDoorCount];

				int doorCount = 0;
				for(int i = 0; i < maxDoors; ++i)
				{
					if (RoomDoors[i] != null)
					{
						if (RoomDoors[i] is HiddenDoor)
						{
							hiddenDoors[doorCount] = (HiddenDoor)RoomDoors[i];
							++doorCount;

							if (doorCount == hiddenDoorCount)
							{
								break;
							}
						}
					}
				}

				return hiddenDoors;
			}
			return null;
		}
	}

	public LockedDoor[] LockedDoors
	{
		get
		{
			if (lockedDoorCount > 0)
			{
				LockedDoor[] lockedDoors = new LockedDoor[lockedDoorCount];
				int doorCount = 0;

				for (int i = 0; i < maxDoors; ++i)
				{
					if (RoomDoors[i] != null)
					{
						if (RoomDoors[i] is LockedDoor)
						{
							lockedDoors[doorCount] = (LockedDoor)RoomDoors[i];
							++doorCount;

							if (doorCount == lockedDoorCount)
							{
								break;
							}
						}
					}

				}

				return lockedDoors;
			}
			return null;
		}
	}
}
