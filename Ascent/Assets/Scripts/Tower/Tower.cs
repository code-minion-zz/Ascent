using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
	private Floor currentFloor;
	public Floor CurrentFloor
	{
		get { return currentFloor;  }
		set { currentFloor = value; }
	}

	private int currentFloorNumber;
	public int CurrentFloorNumber
	{
		get { return currentFloorNumber; }
	}


    public void InitialiseTestFloor()
	{
        currentFloorNumber = 1;
        currentFloor = gameObject.AddComponent<Floor>();
		currentFloor.InitialiseTestFloor();
		Debug.Log("Before");
		AscentSoundManager soundMan = GameObject.Find("SoundManager").GetComponent<AscentSoundManager>();
		soundMan.SetSound("Sounds/music/tower");
		soundMan.PlaySound();
		Debug.Log("After");
    }

	public void InitialiseFloor()
	{
        currentFloorNumber = 1;
        currentFloor = gameObject.AddComponent<Floor>();
        currentFloor.InitialiseRandomFloor();
	}

    protected float experienceGainBonus;
    public float ExperienceGainBonus
    {
        get { return experienceGainBonus; }
    }

    protected float goldGainBonus;
    public float GoldGainBonus
    {
        get { return goldGainBonus; }
    }
}
