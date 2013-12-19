using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Door : MonoBehaviour 
{
	private float standingOnDoorTimer = 0.0f;
	public Floor.TransitionDirection direction;
	public Door targetDoor;
	private bool[] playersLeftDoor;

	private bool startDoor = false;
	public bool StartDoor
	{
		get { return startDoor; }
		set { startDoor = value; }
	}

	bool done;

#if UNITY_EDITOR 
	public void OnDrawGizmos()
	{
		if (targetDoor != null)
		{
			Vector3 dir = (transform.position - targetDoor.transform.position).normalized;

			Vector3 a = transform.position - dir * 7.5f;
			Vector3 b = targetDoor.transform.position + dir * 7.5f;
			
			a.x = a.x - 1.5f;
			a.z = a.z - 1.5f;

			b.x = b.x + 1.5f;
			b.z = b.z + 1.5f;

			a.y = 2.5f;
			b.y = 2.5f;

			Gizmos.DrawLine(a, b);

			a = transform.position;
			a.y = 5.0f;

			Handles.ArrowCap(0, a, Quaternion.LookRotation(FloorCamera.GetDirectionVector(direction), Vector3.up), 1.5f);
		}
	}
#endif

	//public void Update()
	//{
	//    if (targetDoor != null)
	//    {
	//        Debug.DrawLine(transform.position, targetDoor.transform.position);
	//    }
	//}

	public void OnEnable()
	{
		//direction = (Floor.TransitionDirection)Enum.Parse(typeof(Floor.TransitionDirection), gameObject.name);
		done = false;
		playersLeftDoor = new bool[Game.Singleton.Players.Count];
	}

	public void Process()
	{
		if (startDoor)
		{
			// Wait for all players to get out before enabling self
			int countPlayersLeftDoor = 0;
			for (int i = 0; i < playersLeftDoor.Length; ++i)
			{
				if (!playersLeftDoor[i])
				{
					if (!Game.Singleton.Players[i].Hero.collider.bounds.Intersects(collider.bounds))
					{
						playersLeftDoor[i] = true;
						++countPlayersLeftDoor;
					}
				}
				else
				{
					++countPlayersLeftDoor;
				}
			}

			if (countPlayersLeftDoor == playersLeftDoor.Length)
			{
				done = false;
				startDoor = false;
			}
		}

        if (targetDoor != null)
        {

            if (!done)
            {
                int playerCount = Game.Singleton.Players.Count;
                int currentPlayerCount = 0;

                // Check if all heroes are in here.
                foreach (Player p in Game.Singleton.Players)
                {
                    if (p.Hero.collider.bounds.Intersects(collider.bounds))
                    {
                        ++currentPlayerCount;
                    }
                    else
                    {
                        break;
                    }
                }

                if (currentPlayerCount > 0 &&
                    currentPlayerCount == playerCount)
                {
                    standingOnDoorTimer += Time.deltaTime;

                    if (standingOnDoorTimer >= 1.0f)
                    {
                        Game.Singleton.Tower.CurrentFloor.TransitionToRoom(direction, targetDoor);
                        //gameObject.SetActive(false);
                        done = true;
                    }
                }
                else
                {
                    standingOnDoorTimer = 0.0f;
                }
            }
        }
	}

	public void SetAsStartDoor()
	{
		startDoor = true;
		done = true;
	}
	
	//#region Fields
	//public float doorOpenAngle = 90.0f;
	//public float smoothing = 2.0f;
	
	//public bool isDoorOpen = false;
	//public bool isLocked = false;
	
	//private Vector3 defaultRot;
	//private Vector3 openRot;

	//public Vector3 openDirection;
	//private Vector3 defaultPosition;
	
	//#endregion
	
	//#region Properties
	
	//public bool IsOpen 
	//{
	//    get { return isDoorOpen; }
	//    set { isDoorOpen = value; }
	//}
	
	//public bool IsLocked
	//{
	//    get { return isLocked; }
	//    set { isLocked = value; }
	//}
	
	//#endregion
	
	//// Use this for initialization
	//void Start () 
	//{
	//    defaultRot = transform.eulerAngles;
	//    openRot = new Vector3(defaultRot.x + doorOpenAngle, defaultRot.y, defaultRot.z);

	//    defaultPosition = transform.position;
	//}
	
	//// Update is called once per frame
	//void Update () 
	//{
	//    if (IsLocked == false)
	//    {
	//        if (IsOpen)
	//        {
	//            //SwingOpen();
	//            SlideOpen();
	//        }
	//        //else
	//        //{
	//        //    // SwingClose();
	//        //    SlideClose();
	//        //}
	//    }
	//    else
	//    {
	//        //Debug.Log("Door is locked");
	//    }
	//}

	//private void SlideOpen()
	//{
	//    transform.position = Vector3.Lerp(transform.position, defaultPosition + (openDirection * 4.0f), Time.deltaTime * 5.0f);
	//}

	//private void SlideClose()
	//{
	//    transform.position = Vector3.Lerp(transform.position, defaultPosition, Time.deltaTime * 5.0f);
	//}

	//private void SwingOpen()
	//{
	//    transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot,
	//        Time.deltaTime * smoothing);
	//}

	//private void SwingClose()
	//{
	//    transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot,
	//        Time.deltaTime * smoothing);	
	//}
	
	//#region Collision
	
	//void OnCollisionEnter(Collision collision)
	//{
	//    if (collision.transform.tag == "Hero")
	//    {
	//        if (IsOpen == false)
	//        {
	//            Debug.Log("Door opened");
	//            IsOpen = true;
	//        }
	//        else
	//        {
	//            Debug.Log("Door closed");
	//            IsOpen = false;	
	//        }
	//    }		
	//}
	
	//void OnCollisionExit(Collision collisionInfo)
	//{
	//    //IsOpen = true;		
	//}	
	
	//#endregion
}
