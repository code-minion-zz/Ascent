using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour 
{
	private const int maxDoors = 4;
	public Door[] doors = new Door[maxDoors];

	public void Start () 
	{
		Door[] foundDoors = GetComponentsInChildren<Door>() as Door[];

		foreach (Door d in foundDoors)
		{
			doors[(int)d.direction] = d;
		}
	}
}
