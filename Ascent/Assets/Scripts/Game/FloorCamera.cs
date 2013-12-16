using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorCamera : MonoBehaviour
{
    private List<Player> players;
    private Transform _transform;
    private Camera floorCamera;
    private Plane[] cameraFrustPlanes;
    private const float cameraOffset = 5.0f;

    private bool transition = false;
	private float waitTranisition = 0.0f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float time;

	// Default camera is: XYX: 0, 30, -4.8. R: 80x. FOV: 30
	private const float verticalIncrement = 15.2f;
	private const float horizontalIncrement = 30.0f;


    public Camera Camera
    {
        get { return floorCamera; }
    }

	public void Initialise()
	{
		players = Game.Singleton.Players;
		_transform = transform;
		floorCamera = GetComponent<Camera>();
	}

    public void Update()
    {
        //UpdateCameraPosition();
        if (transition)
        {

			if (waitTranisition < 0.5f)
			{
				waitTranisition += Time.deltaTime;
			}
			else
			{
				time += Time.deltaTime * 0.50f;

				if (time >= 1.0f)
				{
					time = 1.0f;
				}
				Vector3 lerpVector = Vector3.Lerp(startPos, targetPos, time);

				transform.position = lerpVector;
			}
			if(time == 1.0f)
			{
				transition = false;
			}
        }
		//else
		//{
		//    if (Input.GetKeyUp(KeyCode.Space))
		//    {
		//        ++currentRoom;

		//        if (currentRoom > lastRoom)
		//        {
		//            currentRoom = 0;
		//        }

		//        transition = true;
		//        time = 0.0f;

		//        startPos = _transform.position;

		//        switch(currentRoom)
		//        {
		//            case 0:
		//                {
		//                    targetPos = new Vector3(0.0f, 30.0f, -4.8f);
		//                }
		//                break;
		//            case 1:
		//                {
		//                    targetPos = new Vector3(30.0f, 30.0f, -4.8f);
		//                }
		//                break;
		//            case 2:
		//                {
		//                    targetPos = new Vector3(30.0f, 30.0f, 15.2f);
		//                }
		//                break;
		//            case 3:
		//                {
		//                    targetPos = new Vector3(0.0f, 30.0f, 15.2f);
		//                }
		//                break;
		//        }
                
		//    }
		//}
    }

    void UpdateCameraPosition()
    {
        if (players.Count > 0)
        {
            // Ulter position of the camera to center on the players
            Vector3 totalVector = Vector3.zero;

            // Add up all the vectors
            foreach (Player player in players)
            {
                if (player != null)
                {
                    totalVector += player.Hero.transform.position;
                }
            }

            // Calculate camera position based off players
            float x = totalVector.x / players.Count;
            float y = _transform.position.y;
            float z = (totalVector.z / players.Count) - cameraOffset;

            Vector3 newVector = new Vector3(x, y, z);
            Vector3 lerpVector = Vector3.Lerp(_transform.position, newVector, 2.0f * Time.deltaTime);

            // Set the position of our camera.
            _transform.position = lerpVector;
        }
    }

    private void CalculateCameraFrustum()
    {
        cameraFrustPlanes = GeometryUtility.CalculateFrustumPlanes(floorCamera);

        int count = 0;
        foreach (Plane plane in cameraFrustPlanes)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + count.ToString();
            p.transform.position = -plane.normal * plane.distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, plane.normal);
            count++;
        }
    }

	public void TransitionToRoom(Floor.TransitionDirection direction)
	{
		startPos = transform.position;
		targetPos = transform.position;

		switch (direction)
		{
			case Floor.TransitionDirection.North:
				{
					targetPos += new Vector3(0.0f, 0.0f, verticalIncrement);
				}
				break;
			case Floor.TransitionDirection.South:
				{
					targetPos += new Vector3(0.0f, 0.0f, -verticalIncrement);
				}
				break;
			case Floor.TransitionDirection.East:
				{
					targetPos += new Vector3(horizontalIncrement, 0.0f, 0.0f);
				}
				break;
			case Floor.TransitionDirection.West:
				{
					targetPos += new Vector3(-horizontalIncrement, 0.0f, 0.0f);
				}
				break;
		}

		waitTranisition = 0.0f;
		time = 0.0f;
		transition = true;
	}
}
