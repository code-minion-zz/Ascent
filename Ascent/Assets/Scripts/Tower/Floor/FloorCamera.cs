using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorCamera : MonoBehaviour
{
    public static List<Hero> Heroes;
    private Transform myTransform;
    private Camera mainCamera;

    private bool transition = false;
    private Vector3 transitionStartPos;
    private Vector3 transitionTargetPos;
    private float transitionTimeElapsed;

	private float roomTransitionTime;

	// Default camera is: XYX: 0, 30, -4.8. R: 80x. FOV: 30
	private const float verticalIncrement = 25.0f;
	private const float horizontalIncrement = 25.0f;

    private CameraShake cameraShake;

	public Vector3 minCamera;
	public Vector3 maxCamera; // Rightside and Bottom

	private float offsetZ = -5.25f;
	public float OffsetZ
	{
		get { return offsetZ; }
	}

    public Camera MainCamera
    {
        get { return mainCamera; }
    }

	public void Initialise()
	{
		Heroes = Game.Singleton.Tower.CurrentFloor.Heroes;
		myTransform = transform;
		mainCamera = GetComponent<Camera>();
        cameraShake = GetComponent<CameraShake>();
	}

    public void Update()
    {
		if (transition)
		{
			transitionTimeElapsed += Time.deltaTime;

			if (transitionTimeElapsed >= roomTransitionTime)
			{
				transitionTimeElapsed = roomTransitionTime;
			}
			Vector3 lerpVector = Vector3.Lerp(transitionStartPos, transitionTargetPos, transitionTimeElapsed / roomTransitionTime);

			transform.position = lerpVector;

			if (transitionTimeElapsed == roomTransitionTime)
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
        Vector3 newVector = CalculateAverageHeroPosition();
        if (newVector == Vector3.zero)
        {
            newVector = transform.position;
        }
        newVector.y = transform.position.y;
		newVector.z += offsetZ;

        Vector3 lerpVector = Vector3.Lerp(myTransform.position, newVector, Time.deltaTime * 2.0f);

		// Set the position of our camera.
		myTransform.position = ClampPositionIntoBounds(lerpVector);
    }

	private Vector3 ClampPositionIntoBounds(Vector3 pos)
	{
        return new Vector3(
        Mathf.Clamp(pos.x, minCamera.x, maxCamera.x),
        22.0f,
		Mathf.Clamp(pos.z, minCamera.z, maxCamera.z));
	}

	public static Vector3 CalculateAverageHeroPosition()
	{
		if (Heroes.Count > 0)
		{
			// Ulter position of the camera to center on the Heros
			Vector3 totalVector = Vector3.zero;

			// Add up all the vectors
			int heroCount = 0;
			foreach (Hero hero in Heroes)
			{
				if (hero != null)
				{
					if (!hero.IsDead)
					{
						totalVector += hero.transform.position;
						++heroCount;
					}
				}
			}

			// Calculate camera position based off Heros
			if (heroCount != 0)
			{
				float x = totalVector.x / Heroes.Count;
				float z = (totalVector.z / (float)heroCount);

				return new Vector3(x, 0.0f, z);
			}
		}

		return Vector3.zero;
	}

	public void TransitionToRoom(Floor.TransitionDirection direction, float roomTransitionTime)
	{
		transitionStartPos = transform.position;
		transitionTargetPos = CalculateAverageHeroPosition();

		transitionTargetPos.z += offsetZ;
		transitionTargetPos = ClampPositionIntoBounds(transitionTargetPos);

		this.roomTransitionTime = roomTransitionTime;
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
