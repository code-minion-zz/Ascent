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
			Vector3 dir = (targetDoor.transform.position - transform.position).normalized;

			Vector3 a = transform.position + dir * 7.5f;
			Vector3 b = targetDoor.transform.position - dir * 9.0f;

			if (direction == Floor.TransitionDirection.North)
			{
				a.x = a.x - 1.5f;
				b.x = b.x - 1.5f;
			}
			else if (direction == Floor.TransitionDirection.South)
			{
				a.x = a.x + 1.5f;
				b.x = b.x + 1.5f; 
			}
			else if (direction == Floor.TransitionDirection.East)
			{
				a.z = a.z - 1.5f;
				b.z = b.z - 1.5f;
			}
			else if (direction == Floor.TransitionDirection.West)
			{
				a.z = a.z + 1.5f;
				b.z = b.z + 1.5f;
			}

			a.y = 2.5f;
			b.y = 2.5f;

			Gizmos.DrawLine(a, b);
			Handles.ArrowCap(0, a, Quaternion.LookRotation(dir, Vector3.up), 1.5f);

			a = transform.position;
			a.y = 5.0f;
			Handles.ArrowCap(0, a, Quaternion.LookRotation(FloorCamera.GetDirectionVector(direction), Vector3.up), 1.5f);
		}
	}
#endif

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

                Debug.Log("Players are out");
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
}
