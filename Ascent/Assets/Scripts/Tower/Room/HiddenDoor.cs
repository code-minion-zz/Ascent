using UnityEngine;
using System.Collections;

public class HiddenDoor : Door
{
	public GameObject blockedDoor;
	public GameObject openedDoor;

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
			blockedDoor.SetActive(false);
			opened = true;
		}
	}
}
