using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour 
{
	private const int maxDoors = 4;
	public Door[] doors = new Door[maxDoors];

	[HideInInspector]
	public int hiddenDoorCount;
	[HideInInspector]
	public int lockedDoorCount;

	public void Start () 
	{
		Door[] foundDoors = GetComponentsInChildren<Door>() as Door[];

		foreach (Door d in foundDoors)
		{
			doors[(int)d.direction] = d;

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
					if (doors[i] != null)
					{
						if (doors[i] is HiddenDoor)
						{
							hiddenDoors[doorCount] = (HiddenDoor)doors[i];
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
					if (doors[i] != null)
					{
						if (doors[i] is LockedDoor)
						{
							lockedDoors[doorCount] = (LockedDoor)doors[i];
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
