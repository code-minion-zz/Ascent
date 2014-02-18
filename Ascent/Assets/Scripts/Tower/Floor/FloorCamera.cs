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

	private float fadeToBlackTime = 0.5f;
	public float transitionTime = 0.5f;

	// Default camera is: XYX: 0, 30, -4.8. R: 80x. FOV: 30
	private const float verticalIncrement = 25.0f;
	private const float horizontalIncrement = 25.0f;

    private CameraShake cameraShake;

	public bool clampToRoom = true;

	public Vector3 minCamera;
	public Vector3 maxCamera;

	private float oldHeight;
	private float cameraHeight = 24.0f;
	public float CameraHeight
	{
		get { return cameraHeight; }
		set 
		{
			oldHeight = cameraHeight;
			//cameraHeight = value;
		}
	}

#if UNITY_WEBPLAYER
	private float offsetZ = -5.35f;
#else
	private float offsetZ = -0.35f;
#endif
	public float OffsetZ
	{
		get { return offsetZ; }
		set 
		{ 
			//offsetZ = value; 
		}
	}


    public Camera Camera
    {
        get { return floorCamera; }
    }

	public void Initialise()
	{
		players = Game.Singleton.Players;
		_transform = transform;
		floorCamera = GetComponent<Camera>();
        cameraShake = GetComponent<CameraShake>();
	}

    public void Update()
    {

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
			UpdateCameraPosition();
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
#if UNITY_WEBPLAYER
		if (players.Count > 0)
		{
			// Calculate camera position based off players
			Vector3 playerPos = players[0].Hero.transform.position;

			Vector3 lerpVector = Vector3.Lerp(_transform.position, new Vector3(playerPos.x, _transform.position.y, playerPos.z + offsetZ), 2.0f * Time.deltaTime);
			_transform.position = lerpVector;
		}
#else
        Vector3 newVector = CalculateAveragePlayerPosition();
        Vector3 lerpVector = Vector3.Lerp(_transform.position, newVector, 2.0f * Time.deltaTime);
		lerpVector.z += offsetZ;
		// Set the position of our camera.

		if (clampToRoom)
		{
			_transform.position = ClampPositionIntoBounds(lerpVector);
		}
		else
		{
			
			_transform.position = lerpVector;
		}
#endif
    }

	private Vector3 ClampPositionIntoBounds(Vector3 pos)
	{
        return new Vector3(
        Mathf.Clamp(pos.x, minCamera.x, maxCamera.x),
       // Mathf.Clamp(pos.y, minCamera.y, maxCamera.y),
	   cameraHeight,
		Mathf.Clamp(pos.z, minCamera.z, maxCamera.z));
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

	private Vector3 CalculateAveragePlayerPosition()
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

			return new Vector3(x, y, z);
		}

		return _transform.position;
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
		startPos.y = oldHeight;
		targetPos = CalculateAveragePlayerPosition();
		targetPos.y = cameraHeight;
		targetPos = Vector3.Lerp(_transform.position, targetPos, 2.0f * Time.deltaTime);
		targetPos.z += offsetZ;
		targetPos = ClampPositionIntoBounds(targetPos);
		

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

    public void ShakeCamera(float intensity, float decay)
    {
        cameraShake.DoShake(intensity, decay);
    }
}
