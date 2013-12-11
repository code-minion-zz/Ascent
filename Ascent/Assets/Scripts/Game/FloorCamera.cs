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

    private int currentRoom = 0;
    private int lastRoom = 3;
    private bool transition = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float time;

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
            time += Time.deltaTime * 0.50f;

            if(time >= 1.0f)
            {
                time = 1.0f;
                transition = false;
            }
            Vector3 lerpVector = Vector3.Lerp(startPos, targetPos, time);


            transform.position = lerpVector;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ++currentRoom;

                if (currentRoom > lastRoom)
                {
                    currentRoom = 0;
                }

                transition = true;
                time = 0.0f;

                startPos = _transform.position;

                switch(currentRoom)
                {
                    case 0:
                        {
                            targetPos = new Vector3(0.0f, 30.0f, -4.8f);
                        }
                        break;
                    case 1:
                        {
                            targetPos = new Vector3(30.0f, 30.0f, -4.8f);
                        }
                        break;
                    case 2:
                        {
                            targetPos = new Vector3(30.0f, 30.0f, 15.2f);
                        }
                        break;
                    case 3:
                        {
                            targetPos = new Vector3(0.0f, 30.0f, 15.2f);
                        }
                        break;
                }
                
            }
        }
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
}
