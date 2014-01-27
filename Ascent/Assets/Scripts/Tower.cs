using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	private Floor currentFloor;
	public Floor CurrentFloor
	{
		get { return currentFloor; }
		set { currentFloor = value; }
	}

	private int currentFloorNumber;
	public int CurrentFloorNumber
	{
		get { return currentFloorNumber; }
	}

    public void InitialiseFloor()
    {
		currentFloorNumber = 1;
        currentFloor = gameObject.AddComponent<Floor>();
        currentFloor.InitialiseFloor();
    }

	public void InitialiseRandomFloor()
	{
		currentFloorNumber = 1;
		currentFloor = gameObject.AddComponent<Floor>();
		currentFloor.InitialiseRandomFloor();
	}

}
