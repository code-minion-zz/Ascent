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

    public void LateUpdate()
    {
        UpdateCameraPosition();
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
