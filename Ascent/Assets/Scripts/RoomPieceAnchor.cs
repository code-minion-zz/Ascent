using UnityEngine;
using System.Collections;

public class RoomPieceAnchor : MonoBehaviour 
{
	public enum EPieceType
	{
		Wall,
		Door,
		Window,
		Arch,
		Corner,
		Ground
	}

	public EPieceType type;
	public Floor.TransitionDirection direction;

	void Start () 
	{
		GameObject go = null;

		switch (type)
		{
			case EPieceType.Wall:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.wallPrefab) as GameObject;
				}
				break;
			case EPieceType.Door:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.doorPrefab) as GameObject;
					Game.Singleton.Tower.CurrentFloor.CurrentRoom.Doors.doors[(int)direction] = go.GetComponent<Door>();
				}
				break;
			case EPieceType.Window:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.wallWindowPrefab) as GameObject;
				}
				break;
			case EPieceType.Arch:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.archPrefab) as GameObject;
				}
				break;
			case EPieceType.Corner:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.cornerPrefab) as GameObject;
				}
				break;
			case EPieceType.Ground:
				{
					go = Instantiate(Game.Singleton.Tower.CurrentFloor.groundPrefab) as GameObject;
				}
				break;
		}

		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;
		go.transform.parent	= transform.parent;

		GameObject.Destroy(gameObject);
	}
}
