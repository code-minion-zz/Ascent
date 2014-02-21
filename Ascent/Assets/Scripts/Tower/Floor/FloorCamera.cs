using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorCamera : MonoBehaviour
{
    private List<Player> players;
    private Transform myTransform;
    private Camera mainCamera;

    private bool transition = false;
    private Vector3 transitionStartPos;
    private Vector3 transitionTargetPos;
    private float transitionTimeElapsed;

	private float transitionTime = 0.5f;

	// Default camera is: XYX: 0, 30, -4.8. R: 80x. FOV: 30
	private const float verticalIncrement = 25.0f;
	private const float horizontalIncrement = 25.0f;

    private CameraShake cameraShake;

	private bool clampToRoom = true;

	public Vector3 minCamera;
	public Vector3 maxCamera; // Rightside and Bottom

    //private float oldHeight;
    //private float cameraHeight = 24.0f;
    //public float CameraHeight
    //{
    //    get { return cameraHeight; }
    //    set 
    //    {
    //        oldHeight = cameraHeight;
    //        //cameraHeight = value;
    //    }
    //}

#if UNITY_WEBPLAYER
	private float offsetZ = -5.35f;
#else
    [HideInInspector]
	public float offsetZ = -0.35f;
#endif
	public float OffsetZ
	{
		get { return offsetZ; }
		set 
		{ 
			//offsetZ = value; 
		}
	}


    public Camera MainCamera
    {
        get { return mainCamera; }
    }

	public void Initialise()
	{
		players = Game.Singleton.Players;
		myTransform = transform;
		mainCamera = GetComponent<Camera>();
        cameraShake = GetComponent<CameraShake>();
	}

    public void Update()
    {
		if (transition)
		{
			transitionTimeElapsed += Time.deltaTime;

			if (transitionTimeElapsed >= transitionTime)
			{
				transitionTimeElapsed = transitionTime;
			}
			Vector3 lerpVector = Vector3.Lerp(transitionStartPos, transitionTargetPos, transitionTimeElapsed / transitionTime);

			transform.position = lerpVector;

			if (transitionTimeElapsed == transitionTime)
			{
				transition = false;
				
			}
		}
		else
		{
			UpdateCameraPosition();
		}
    }

    public void UpdateCameraPosition()
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
        Vector3 lerpVector = Vector3.Lerp(myTransform.position, newVector, 2.0f * Time.deltaTime);
		lerpVector.z += offsetZ;

		// Set the position of our camera.
		if (clampToRoom)
		{
			myTransform.position = ClampPositionIntoBounds(lerpVector);
		}
		else
		{
			
			myTransform.position = lerpVector;
		}
#endif
    }

	private Vector3 ClampPositionIntoBounds(Vector3 pos)
	{
        return new Vector3(
        Mathf.Clamp(pos.x, minCamera.x, maxCamera.x),
        22.0f,
		Mathf.Clamp(pos.z, minCamera.z, maxCamera.z));
	}

	public Vector3 CalculateAveragePlayerPosition()
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
			float y = myTransform.position.y;
			float z = (totalVector.z / players.Count);

			return new Vector3(x, y, z);
		}

		return myTransform.position;
	}

	public void TransitionToRoom(Floor.TransitionDirection direction)
	{
		transitionStartPos = transform.position;
		//transitionStartPos.y = oldHeight;
		transitionTargetPos = CalculateAveragePlayerPosition();
		//transitionTargetPos.y = cameraHeight;
		transitionTargetPos = Vector3.Lerp(myTransform.position, transitionTargetPos, 2.0f * Time.deltaTime);
		transitionTargetPos.z += offsetZ;
		transitionTargetPos = ClampPositionIntoBounds(transitionTargetPos);
		
		transitionTimeElapsed = 0.0f;
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
