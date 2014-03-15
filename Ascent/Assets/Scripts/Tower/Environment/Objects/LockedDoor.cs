using UnityEngine;
using System.Collections;

public class LockedDoor : Door
{
	public GameObject lockedDoor;
	public TriggerRegion triggerRegion;

	[HideInInspector]
	public bool opened;

	public void Start()
	{
		openedDoor.SetActive(false);
	}

	public void Open()
	{
		if (!opened)
		{
			openedDoor.SetActive(true);
			lockedDoor.SetActive(false);
			opened = true;
		}
	}
}
