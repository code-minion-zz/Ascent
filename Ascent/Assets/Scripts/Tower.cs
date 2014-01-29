using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	private Floor currentFloor;
	public Floor CurrentFloor
	{
		get 
        {
            // This is temp code.
            if (currentFloorNumber == 0)
            {	
                InitialiseFloor();
            }

            return currentFloor;
        }
		set { currentFloor = value; }
	}

	private int currentFloorNumber;
	public int CurrentFloorNumber
	{
		get { return currentFloorNumber; }
	}

    public void InitialiseFloor()
    {
        if (currentFloorNumber == 0)
        {
            currentFloorNumber = 1;
            currentFloor = gameObject.AddComponent<Floor>();
            currentFloor.InitialiseFloor();
        }
    }

	public void InitialiseRandomFloor()
	{
        if (currentFloorNumber == 0)
        {
            currentFloorNumber = 1;
            currentFloor = gameObject.AddComponent<Floor>();
            currentFloor.InitialiseRandomFloor();
        }
	}

}
