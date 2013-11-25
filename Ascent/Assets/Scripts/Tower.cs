using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	private Floor currentFloor;
	public Floor CurrentFloor
	{

		get 
		{
			if (currentFloor == null)
			{
				currentFloor = gameObject.AddComponent<Floor>();
			}
			return currentFloor;
		}
		set { currentFloor = value; }
	}
}
