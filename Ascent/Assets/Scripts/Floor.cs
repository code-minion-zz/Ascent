using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour 
{
	private List<Player> players;
	private GameObject[] startPoints;
	private GameObject CameraPrefab;
	private Plane[] cameraFrustPlanes;

	// Camera offset
	private const float cameraOffset = 15.0f;
	public bool orthographicCamera = false;


	public Camera MainCamera
	{
		get { return CameraPrefab.camera; }
	}

	public GameObject[] StartPoints
	{
		get { return startPoints; }
	}

	private List<Enemy> enemies;
	public List<Enemy> Enemies
	{
		get { return enemies; }
	}

	//private FloorRecordKeeper recordKeeper;
	//public FloorRecordKeeper Records
	//{
	//    get { return recordKeeper; }
	//}

	// Use this for initialization
	void Awake()
	{
		enemies = new List<Enemy>();
	}

	void Start()
	{

		//Resources.Load("Prefabs/Level" + Game.Singleton.GetChosenLevel);
		// Create the camera

		GameObject go = null;

		if (orthographicCamera)
		{
			go = Resources.Load("Prefabs/GameCameraOrtho") as GameObject;
		}
		else
		{
			go = Resources.Load("Prefabs/GameCamera") as GameObject;
		}

		startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

		players = Game.Singleton.Players;

		for (int i = 0; i < players.Count; ++i)
		{
			Vector3 pos = startPoints[i].transform.position;
			//players[i].Hero.transform.position = pos;
			//players[i].Hero.transform.rotation = Quaternion.identity;
			players[i].transform.position = pos;
			players[i].transform.rotation = Quaternion.identity;
		}

		CameraPrefab = Instantiate(go) as GameObject;

		//CalculateCameraFrustum();
	}

	public void AddEnemy(Enemy _enemy)
	{
		enemies.Add(_enemy);
	}

	#region Update

	// Update is called once per frame
	void Update()
	{
		// Update Camera
		UpdateCamPos();
		HandleDeadHeroes();

		if (Input.GetKeyUp(KeyCode.F1))
		{
			EndFloor();
		}
	}

	void UpdateCamPos()
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
		float y = CameraPrefab.transform.position.y;
		float z = (totalVector.z / players.Count) - cameraOffset;

		Vector3 newVector = new Vector3(x, y, z);
		Vector3 lerpVector = Vector3.Lerp(CameraPrefab.transform.position, newVector, 2.0f * Time.deltaTime);

		// Set the position of our camera.
		CameraPrefab.transform.position = lerpVector;

		//GameObject.Find("CameraBlur").transform.position = lerpVector;
	}

	private void CalculateCameraFrustum()
	{
		Camera cam = CameraPrefab.GetComponent<Camera>();
		cameraFrustPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

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

	void HandleDeadHeroes()
	{
		foreach (Player player in players)
		{
			Hero hero = player.Hero.GetComponent<Hero>();

			if (hero.IsDead)
			{
				hero.Respawn(startPoints[0].transform.position);
			}
		}
	}

	void EndFloor()
	{
		// Disable the whole floor( audio listener from the camera )
		enabled = false;
		CameraPrefab.SetActive(false);

		// Disable input on all heroes
		foreach (Player player in players)
		{
			player.Hero.GetComponent<Hero>().HeroController.DisableInput();
			player.Hero.SetActive(false);
		}

		// Show summary screen
		Instantiate(Resources.Load("Prefabs/FloorSummary"));

		// Enable input on summary screen
	}

	#endregion
}
