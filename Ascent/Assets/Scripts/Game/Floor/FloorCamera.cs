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

	private float fadeToBlackTime = 0.75f;
	private float transitionTime = 2.0f;

	// Default camera is: XYX: 0, 30, -4.8. R: 80x. FOV: 30
	private const float verticalIncrement = 20.0f;
	private const float horizontalIncrement = 30.0f;

	private Vector3 minCamera;
	private Vector3 maxCamera;


    public Camera Camera
    {
        get { return floorCamera; }
    }

	public void Initialise()
	{
		players = Game.Singleton.Players;
		_transform = transform;
		floorCamera = GetComponent<Camera>();

		minCamera = new Vector3(-2.0f, 25.0f, -2.0f);
		maxCamera = new Vector3(2.0f, 25.0f, 2.0f);
	}

    public void Update()
    {
		UpdateCameraPosition();
		transform.position = new Vector3(
		    Mathf.Clamp(transform.position.x, minCamera.x, maxCamera.x),

		    Mathf.Clamp(transform.position.y, minCamera.y, maxCamera.y),

		    Mathf.Clamp(transform.position.z, minCamera.z, maxCamera.z));


		//UpdateCameraRotation();

        //UpdateCameraPosition();
		if (transition)
		{
			if (waitTranisition < fadeToBlackTime)
			{
				waitTranisition += Time.deltaTime;
			}
			else
			{
				time += Time.deltaTime;

				if (time >= transitionTime)
				{
					time = transitionTime;
				}
				Vector3 lerpVector = Vector3.Lerp(startPos, targetPos, time / transitionTime);

				transform.position = lerpVector;
			}
			if (time == transitionTime)
			{
				transition = false;
			}
		}
		else
		{

		}

		if(Input.GetKeyUp(KeyCode.Keypad5))
		{
			transitionTime = 0.25f;
			fadeToBlackTime = 0.0f;
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
            float z = (totalVector.z / players.Count);

            Vector3 newVector = new Vector3(x, y, z);
            Vector3 lerpVector = Vector3.Lerp(_transform.position, newVector, 2.0f * Time.deltaTime);

            // Set the position of our camera.
            _transform.position = lerpVector;
        }
    }

	void UpdateCameraRotation()
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
			float y = totalVector.y;
			float z = (totalVector.z / players.Count);

			Vector3 newVector = new Vector3(x, y, z);
			Debug.Log(newVector);
			transform.LookAt(newVector, Vector3.up);
			//Vector3 lerpVector = Vector3.Lerp(_transform.position, newVector, 2.0f * Time.deltaTime);

			// Set the position of our camera.
			//_transform.position = lerpVector;
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

		targetPos += GetDirectionVector(direction);

		waitTranisition = 0.0f;
		time = 0.0f;
		transition = true;
	}

	static public Vector3 GetDirectionVector(Floor.TransitionDirection direction)
	{
		Vector3 vec = Vector3.zero;

		switch (direction)
		{
			case Floor.TransitionDirection.North:
				{
					vec = new Vector3(0.0f, 0.0f, verticalIncrement);
				}
				break;
			case Floor.TransitionDirection.South:
				{
					vec = new Vector3(0.0f, 0.0f, -verticalIncrement);
				}
				break;
			case Floor.TransitionDirection.East:
				{
					vec = new Vector3(horizontalIncrement, 0.0f, 0.0f);
				}
				break;
			case Floor.TransitionDirection.West:
				{
					vec = new Vector3(-horizontalIncrement, 0.0f, 0.0f);
				}
				break;
		}

		return vec;
	}
}
