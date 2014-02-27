using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Door : MonoBehaviour 
{
	public Floor.TransitionDirection direction;
	public Door targetDoor;
    public bool isConnected;
    public bool isEntryDoor = false;
    public Collider immediateArea;

    private float standingOnDoorTimer = 0.0f;
	private bool[] playersLeftDoor;
	private bool startDoor = false;

	public bool StartDoor
	{
		get { return startDoor; }
		set { startDoor = value; }
	}

	bool walkedOutOfTheDoor;

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
        if (Game.Singleton.Tower.CurrentFloor != null)
        {
            //direction = (Floor.TransitionDirection)Enum.Parse(typeof(Floor.TransitionDirection), gameObject.name);
            walkedOutOfTheDoor = false;
            playersLeftDoor = new bool[Game.Singleton.Players.Count];
        }
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
				walkedOutOfTheDoor = false;
				startDoor = false;
			}
		}

        if (targetDoor != null)
        {
			int playerCount = Game.Singleton.AlivePlayerCount;

			// If only 1 player then they can quickly transition between rooms
			if (playerCount == 1)
			{
                foreach (Player p in Game.Singleton.Players)
                {
                    if (!p.Hero.IsDead)
                    {
                       if(p.Hero.collider.bounds.Intersects(immediateArea.bounds))
                       {
                           Game.Singleton.Tower.CurrentFloor.TransitionToRoom(direction, targetDoor);
                       }
                    }
                }
			}

            int currentPlayerCount = 0;

            // Check if all heroes are in here.
            foreach (Player p in Game.Singleton.Players)
            {
                if (p.Hero.collider.bounds.Intersects(collider.bounds))
                {
                    ++currentPlayerCount;
                }
            }

            if (currentPlayerCount > 0 &&
                currentPlayerCount == playerCount)
            {
				if (!walkedOutOfTheDoor)
				{
					standingOnDoorTimer += Time.deltaTime;
					if (standingOnDoorTimer >= 0.5f)
					{
						Game.Singleton.Tower.CurrentFloor.TransitionToRoom(direction, targetDoor);
						walkedOutOfTheDoor = true;
					}
				}
				else
				{
					// If one player is standing on the immediate area then a transition can occur immediately.
					foreach (Player p in Game.Singleton.Players)
					{
						if (p.Hero.collider.bounds.Intersects(immediateArea.bounds))
						{
							Debug.Log("inside it!");
							Game.Singleton.Tower.CurrentFloor.TransitionToRoom(direction, targetDoor);
							walkedOutOfTheDoor = true;
						}
					}
				}
            }
            else
            {
                standingOnDoorTimer = 0.0f;
            }
        }
	}

	public void SetAsStartDoor()
	{
		startDoor = true;
		walkedOutOfTheDoor = true;
	}
}
