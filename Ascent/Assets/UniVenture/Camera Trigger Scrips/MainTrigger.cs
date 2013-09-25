using UnityEngine;
using System.Collections;

public class MainTrigger : MonoBehaviour {
	
	public CameraData cameraData;
	public MonoBehaviour playerScript;
	
	public CamMode cameraMode;
	public enum CamMode 
	{ 
		Dungeon,
		Overworld
	}
	
	public CamStyle cameraStyle;
	public enum CamStyle 
	{ 
		Modern,
		Classic
	}
	
	public MoveCameraDirection lastMovedDirection {get; set;}
	public enum MoveCameraDirection 
	{
		North, 
		South, 
		East, 
		West
	}
	
	private RoomDimensions roomDimension;
	private enum RoomDimensions 
	{
		SMALLER_X_SMALLER_Y, 
		LARGER_X_LARGER_Y, 
		SMALLER_X_LARGER_Y, 
		LARGER_X_SMALLER_Y
	}
	
	public float cameraMoveTime = 0.5f;
	public bool drawDebugLines = false;

	private float playerColliderSize = 1.0f; //make public?
	
	[HideInInspector]
	public bool northLock, southLock, eastLock, westLock;
	
	/*-----Private Fields-----*/
	/*-----References-----*/
	private nTrigger nPreScript;
	private sTrigger sPreScript;
	private eTrigger ePreScript;
	private wTrigger wPreScript;
	
	/*-----Game Objects-----*/
	private GameObject camObj;
	private GameObject player;
	
	/*-----Vectors-----*/
	private Vector3 playerVect;
	private Vector3 northLockVect, southLockVect, eastLockVect, westLockVect;
	private Vector3 pPos;
	private Vector3 cPos;
	private Vector3 debugLine1, debugLine2, debugLine3, debugLine4, debugLine5, debugLine6, debugLine7, debugLine8;
	private Vector2 roomDim;
	
	/*-----Floats-----*/
	private float cPosX, cPosY, cPosZ;
	private float eastWestMidpoint, northSouthMidpoint;
	private float northDifference, southDifference, eastDifference, westDifference;
	
	/*-----Booleans-----*/
	private bool inTransition = false;
	private bool isPerspectiveMode;
	
	/*-----Layer Masks-----*/
	private LayerMask layerMask1;
	private LayerMask layerMask2;
	private LayerMask layerMask;
	
	void Awake()
	{
		nPreScript = GameObject.Find("NorthTrigger").GetComponent<nTrigger>();
		sPreScript = GameObject.Find("SouthTrigger").GetComponent<sTrigger>();
		ePreScript = GameObject.Find("EastTrigger").GetComponent<eTrigger>();
		wPreScript = GameObject.Find("WestTrigger").GetComponent<wTrigger>();
		
		camObj = GameObject.FindWithTag("MainCamera");
		player = GameObject.FindWithTag("Player");
		
		layerMask1 = 1 << LayerMask.NameToLayer("Wall");
		layerMask2 = 1 << LayerMask.NameToLayer("InvisibleBound");
		layerMask = (layerMask1 | layerMask2);
	}
	
	void Start ()
	{	
		//if using player with character controller delete comment and replace "MovementScriptNameHere"
		//with the name of your player movement script.
		//playerScript = GameObject.FindWithTag("Player").GetComponent<MovementScriptNameHere>();
		
		if(!playerScript)
			Debug.LogError("playerScript variable is null \nPlease drag player into ObjectField");
		
		if(!cameraData)
			Debug.LogError("cameraData variable is null \nPlease drag camdata asset from Project folder into ObjectField");
		else
		{
			isPerspectiveMode = cameraData.perspectiveProjection;
			roomDim = new Vector2(cameraData.roomSize.x, cameraData.roomSize.y);
		}
		
		northLockVect = new Vector3(0,0,1);
		southLockVect = new Vector3(0,0,-1);
		eastLockVect = new Vector3(1,0,0);
		westLockVect = new Vector3(-1,0,0);
		
		cPos.y = camObj.transform.position.y;
		
		if(cameraMoveTime < 0.0f)
			cameraMoveTime = 0.001f;
		
		if(cameraStyle == MainTrigger.CamStyle.Classic)
			cameraMoveTime = 0.01f;
		
		CheckNextRoomEW(new Vector3(camObj.transform.position.x, 0.5f, camObj.transform.position.z - (roomDim.y / 2f) + 1.5f), new Vector3(1, 0, 0));
		CheckNextRoomNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2f) - 1.5f, 0.5f, camObj.transform.position.z), new Vector3(0, 0, 1));
		
		GetRoomDimensions();
	}
	
	void Update()
	{	
		if(cameraMode == CamMode.Overworld)
			return;
		
		if(drawDebugLines)
		{
			Debug.DrawLine(debugLine1, debugLine2, Color.blue);
			Debug.DrawLine(debugLine3, debugLine4, Color.red);
		
			Debug.DrawLine(debugLine5, debugLine6, Color.blue);
			Debug.DrawLine(debugLine7, debugLine8, Color.red);
		}
	}
	
	void LateUpdate()
	{
		if(cameraMode == CamMode.Overworld) 
			return;
		
		playerVect = player.transform.position - camObj.transform.position;
		pPos = player.transform.position;

		cPos = camObj.transform.position;
		cPosX = camObj.transform.position.x;
		cPosZ = camObj.transform.position.z;
		
		if(!inTransition)
		{	
			if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_LARGER_Y)
			{	
				if(eastLock && Vector3.Dot(eastLockVect, playerVect) < 0)
					eastLock = false;
				if(westLock && Vector3.Dot(westLockVect, playerVect) < 0)
					westLock = false;
				if(northLock && Vector3.Dot(northLockVect, playerVect) < 0)
					northLock = false;
				if(southLock && Vector3.Dot(southLockVect, playerVect) < 0)
					southLock = false;

				if(eastLock && westLock)
				{
					cPosX = cPos.x;
				}
				else if(!eastLock && westLock)
				{
					if(pPos.x < cPos.x)
						cPosX = cPos.x;
					else
						cPosX = pPos.x;
				}
				else if (eastLock && !westLock)
				{
					if(pPos.x > cPos.x)
						cPosX = cPos.x;
					else
						cPosX = pPos.x;
				}
				else
					cPosX = pPos.x;

				if(northLock && southLock)
				{
					cPosZ = cPos.z;	
				}
				else if(!northLock && southLock)
				{
					if(pPos.z < cPos.z)
						cPosZ = cPos.z;
					else
						cPosZ = pPos.z;
				}
				else if (northLock && !southLock)
				{
					if(pPos.z > cPos.z)
						cPosZ = cPos.z;
					else
						cPosZ = pPos.z;
				}
				else
					cPosZ = pPos.z;
			}
			else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y)
			{
				if(northLock && Vector3.Dot(northLockVect, playerVect) < 0)
					northLock = false;
				if(southLock && Vector3.Dot(southLockVect, playerVect) < 0)
					southLock = false;
				
				if(northLock && southLock)
				{
					cPosZ = cPos.z;	
				}
				else if(!northLock && southLock)
				{
					if(pPos.z < cPos.z)
						cPosZ = cPos.z;
					else
						cPosZ = pPos.z;
				}
				else if (northLock && !southLock)
				{
					if(pPos.z > cPos.z)
						cPosZ = cPos.z;
					else
						cPosZ = pPos.z;
				}
				else
					cPosZ = pPos.z;
			}
			else if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y)
			{	
				if(eastLock && Vector3.Dot(eastLockVect, playerVect) < 0)
					eastLock = false;
				if(westLock && Vector3.Dot(westLockVect, playerVect) < 0)
					westLock = false;
				
				if(eastLock && westLock)
				{
					cPosX = cPos.x;
				}
				else if(!eastLock && westLock)
				{
					if(pPos.x < cPos.x)
						cPosX = cPos.x;
					else
						cPosX = pPos.x;
				}
				else if (eastLock && !westLock)
				{
					if(pPos.x > cPos.x)
						cPosX = cPos.x;
					else
						cPosX = pPos.x;
				}
				else
					cPosX = pPos.x;
			}
			else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y)
			{
				//do something like nothing
				northLock = true;
				eastLock = true;
				westLock = true;
				southLock = true;
			}
			
			camObj.transform.position = new Vector3(cPosX, cPos.y, cPosZ);
		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col == nPreScript.exitCol)
		{
			nPreScript.exitCol = null;
		}
		if (col == sPreScript.exitCol)
		{
			sPreScript.exitCol = null;
		}
		if (col == ePreScript.exitCol)
		{
			ePreScript.exitCol = null;
		}
		if (col == wPreScript.exitCol)
		{
			wPreScript.exitCol = null;
		}
	}

	public void TranslateCameraUp()
	{
		CheckNextRoomNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) - 1.5f, 0.5f, camObj.transform.position.z + roomDim.y), new Vector3(0,0,1));
		CheckNextRoomEW(new Vector3(camObj.transform.position.x, 0.5f, camObj.transform.position.z + (roomDim.y / 2) + 1.5f), new Vector3(1, 0,0));
		GetRoomDimensions();
		StartCoroutine(MoveCamera(MoveCameraDirection.North));
	}
	public void TranslateCameraDown()
	{
		CheckNextRoomNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) - 1.5f, 0.5f, camObj.transform.position.z - roomDim.y), new Vector3(0,0,1));
		CheckNextRoomEW(new Vector3(camObj.transform.position.x, 0.5f, camObj.transform.position.z - (roomDim.y / 2) - 1.5f), new Vector3(1,0,0));
		GetRoomDimensions();
		StartCoroutine(MoveCamera(MoveCameraDirection.South));
	}
	public void TranslateCameraRight()
	{
		CheckNextRoomNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) + 1.5f, 0.5f, camObj.transform.position.z), new Vector3(0,0,1));
		CheckNextRoomEW(new Vector3(camObj.transform.position.x + roomDim.x, 0.5f, camObj.transform.position.z - (roomDim.y / 2) + 1.5f), new Vector3(1,0,0));
		GetRoomDimensions();
		StartCoroutine(MoveCamera(MoveCameraDirection.East));
	}
	public void TranslateCameraLeft()
	{
		CheckNextRoomNS(new Vector3(camObj.transform.position.x - (roomDim.x / 2) - 1.5f, 0.5f, camObj.transform.position.z), new Vector3(0,0,1));
		CheckNextRoomEW(new Vector3(camObj.transform.position.x - roomDim.x, 0.5f, camObj.transform.position.z - (roomDim.y / 2) + 1.5f), new Vector3(1,0,0));
		GetRoomDimensions();
		StartCoroutine(MoveCamera(MoveCameraDirection.West));
	}
	
	private IEnumerator MoveCamera(MoveCameraDirection moveCamera)
	{
		
		if(cameraMode == MainTrigger.CamMode.Dungeon)
		{
			StartCoroutine(Transition(cameraMoveTime));
			StartCoroutine(FreezePlayer(cameraMoveTime));
			Vector3 start = camObj.transform.position;
			Vector3 end = Vector3.zero;
			Vector3 playerStart = player.transform.position;
			Vector3 playerEnd = Vector3.zero;
			float fPositionX = 0; 
			float fPositionZ = 0;
			
			//Determine player and camera end points based on movement direction
			////*******************   North   ******************////
			if(moveCamera == MoveCameraDirection.North)
			{
				if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_LARGER_Y)
				{
					if(eastLockCheck)
					{
						if(eastLock)
						{
							if(player.transform.position.x > camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x < camObj.transform.position.x - eastDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x - eastDifference;
						}
					}
					if(westLockCheck)
					{
						if(westLock)
						{
							if(player.transform.position.x < camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x > camObj.transform.position.x + westDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x + westDifference;
						}
					}
					if(!eastLockCheck && !westLockCheck)
						fPositionX = player.transform.position.x;
					
					fPositionZ = camObj.transform.position.z + roomDim.y;
					fPositionZ = RoundToTens(fPositionZ);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y)
				{
					fPositionZ = camObj.transform.position.z + roomDim.y;
					fPositionX = eastWestMidpoint;
					
					fPositionZ = RoundToTens(fPositionZ);
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y)
				{
					fPositionZ = camObj.transform.position.z + roomDim.y;
					fPositionX = eastWestMidpoint;
					
					fPositionZ = RoundToTens(fPositionZ);
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y)
				{
					if(eastLockCheck)
					{
						if(eastLock)
						{
							if(player.transform.position.x > camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x < camObj.transform.position.x - eastDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x - eastDifference;	
						}
					}
					if(westLockCheck)
					{
						if(westLock)
						{
							if(player.transform.position.x < camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x > camObj.transform.position.x + westDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x + westDifference;	
						}
					}
					if(!eastLockCheck && !westLockCheck)
						fPositionX = player.transform.position.x;
					
					fPositionZ = camObj.transform.position.z + roomDim.y;
					
					fPositionZ = RoundToTens(fPositionZ);
				}
				
				playerEnd = player.transform.position + new Vector3(0.0f,0.0f,MovePlayer());
			}
			////*******************   South   ******************////
			else if(moveCamera == MoveCameraDirection.South)
			{
				if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_LARGER_Y)
				{
					if(eastLockCheck)
					{
						if(eastLock)
						{
							if(player.transform.position.x > camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x < camObj.transform.position.x - eastDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x - eastDifference;	
						}
					}
					if(westLockCheck)
					{
						if(westLock)
						{
							if(player.transform.position.x < camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = player.transform.position.x;
						}
						else
						{
							if(player.transform.position.x > camObj.transform.position.x + westDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x + westDifference;
						}
					}
					if(!eastLockCheck && !westLockCheck)
						fPositionX = player.transform.position.x;
					
					fPositionZ = camObj.transform.position.z - roomDim.y;
					
					fPositionZ = RoundToTens(fPositionZ);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y)
				{
					fPositionZ = camObj.transform.position.z - roomDim.y;
					fPositionX = eastWestMidpoint;
					
					fPositionZ = RoundToTens(fPositionZ);
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y)
				{
					fPositionZ = camObj.transform.position.z - roomDim.y;
					fPositionX = eastWestMidpoint;
					
					fPositionZ = RoundToTens(fPositionZ);
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y)
				{
					if(eastLockCheck)
					{
						if(eastLock)
						{
							if(player.transform.position.x > camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
							{
								fPositionX = player.transform.position.x;
							}
						}
						else
						{
							if(player.transform.position.x > camObj.transform.position.x)
								fPositionX = camObj.transform.position.x - eastDifference;
							else
							{
								if(player.transform.position.x < camObj.transform.position.x - eastDifference)
									fPositionX = player.transform.position.x;
								else
									fPositionX = camObj.transform.position.x - eastDifference;
							}
						}
					}
					if(westLockCheck)
					{
						if(westLock)
						{
							if(player.transform.position.x < camObj.transform.position.x)
								fPositionX = camObj.transform.position.x;
							else
								fPositionX = camObj.transform.position.x;
						}
						else
						{
							if(player.transform.position.x > camObj.transform.position.x + westDifference)
								fPositionX = player.transform.position.x;
							else
								fPositionX = camObj.transform.position.x + westDifference;	
						}
					}
					if(!eastLockCheck && !westLockCheck)
						fPositionX = player.transform.position.x;
					
					fPositionZ = camObj.transform.position.z - roomDim.y;
					
					fPositionZ = RoundToTens(fPositionZ);
				}
				
				playerEnd = player.transform.position + new Vector3(0.0f,0.0f,-MovePlayer());
			}
			////*******************   East   ******************////
			else if(moveCamera == MoveCameraDirection.East)
			{
				if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_LARGER_Y)
				{
					if(northLockCheck)
					{
						if(northLock)
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z - northDifference;
							else
								fPositionZ = camObj.transform.position.z - northDifference;
						}
					}
					if(southLockCheck)
					{
						if(southLock)
						{
							if(player.transform.position.z < camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z < camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z + southDifference;
							else
								fPositionZ = camObj.transform.position.z + southDifference;
						}
					}
					if(!northLockCheck && !southLockCheck)
						fPositionZ = player.transform.position.z;
					
					fPositionX = camObj.transform.position.x + roomDim.x;
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y)
				{
					fPositionX = camObj.transform.position.x + roomDim.x;
					fPositionZ = northSouthMidpoint;
					
					fPositionX = RoundToTens(fPositionX);
					fPositionZ = RoundToTens(fPositionZ);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y)
				{
					if(northLockCheck)
					{
						if(northLock)
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z < camObj.transform.position.z - northDifference)
								fPositionZ = player.transform.position.z;
							else
								fPositionZ = camObj.transform.position.z - northDifference;
						}
					}
					if(southLockCheck)
					{
						if(southLock)
						{
							if(player.transform.position.z < camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z > camObj.transform.position.z + southDifference)
								fPositionZ = player.transform.position.z;
							else
								fPositionZ = camObj.transform.position.z + southDifference;
						}
					}
					if(!northLockCheck && !southLockCheck)
						fPositionZ = player.transform.position.z;
					
					fPositionX = camObj.transform.position.x + roomDim.x;
					
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y)
				{
					fPositionX = camObj.transform.position.x + roomDim.x;
					fPositionZ = northSouthMidpoint;
					
					fPositionX = RoundToTens(fPositionX);
					fPositionZ = RoundToTens(fPositionZ);
				}
				
				playerEnd = player.transform.position + new Vector3(MovePlayer(),0.0f,0.0f);
			}
			////*******************   West   ******************////
			else if(moveCamera == MoveCameraDirection.West)
			{
				if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_LARGER_Y)
				{
					if(northLockCheck)
					{
						if(northLock)
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z - northDifference;
							else
								fPositionZ = camObj.transform.position.z - northDifference;
						}
					}
					if(southLockCheck)
					{
						if(southLock)
						{
							if(player.transform.position.z < camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z < camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z + southDifference;
							else
								fPositionZ = camObj.transform.position.z + southDifference;
						}
					}
					if(!northLockCheck && !southLockCheck)
						fPositionZ = player.transform.position.z;
					
					fPositionX = camObj.transform.position.x - roomDim.x;
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y)
				{
					fPositionX = camObj.transform.position.x - roomDim.x;
					fPositionZ = northSouthMidpoint;
					
					fPositionX = RoundToTens(fPositionX);
					fPositionZ = RoundToTens(fPositionZ);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y)
				{
					if(northLockCheck)
					{
						if(northLock)
						{
							if(player.transform.position.z > camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z < camObj.transform.position.z - northDifference)
								fPositionZ = player.transform.position.z;
							else
								fPositionZ = camObj.transform.position.z - northDifference;
						}
					}
					if(southLockCheck)
					{
						if(southLock)
						{
							if(player.transform.position.z <= camObj.transform.position.z)
								fPositionZ = camObj.transform.position.z;
							else
								fPositionZ = player.transform.position.z;
						}
						else
						{
							if(player.transform.position.z > camObj.transform.position.z + southDifference)
								fPositionZ = player.transform.position.z;
							else
								fPositionZ = camObj.transform.position.z + southDifference;
						}
					}
					if(!northLockCheck && !southLockCheck)
						fPositionZ = player.transform.position.z;
					
					fPositionX = camObj.transform.position.x - roomDim.x;
					fPositionX = RoundToTens(fPositionX);
				}
				else if(roomDimension == MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y)
				{
					fPositionX = camObj.transform.position.x - roomDim.x;
					fPositionZ = northSouthMidpoint;
					
					fPositionX = RoundToTens(fPositionX);
					fPositionZ = RoundToTens(fPositionZ);
				}
				
				playerEnd = player.transform.position + new Vector3(-MovePlayer(),0.0f,0.0f);
			}
			
			end = new Vector3(fPositionX, camObj.transform.position.y, fPositionZ);
			
			float rate = 1.0f/cameraMoveTime;
			float t = 0;
			while (t < 1.0f)
			{
				t += Time.deltaTime * rate;
				camObj.transform.position = Vector3.Lerp(start, end, t);
				player.transform.position = Vector3.Lerp(playerStart, playerEnd, t);
				yield return 0;
			}
			
			if(lastMovedDirection == MainTrigger.MoveCameraDirection.North)
			{
				VerifyRoomDimensionsNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) - 1.5f, 0.5f, northSouthMidpoint), new Vector3(0,0,1));
				VerifyRoomDimensionsEW(new Vector3(eastWestMidpoint, 0.5f, camObj.transform.position.z - (roomDim.y / 2f) + 1.5f), new Vector3(1, 0,0));
			}
			else if (lastMovedDirection == MainTrigger.MoveCameraDirection.South)
			{
				VerifyRoomDimensionsNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) - 1.5f, 0.5f, northSouthMidpoint), new Vector3(0,0,1));
				VerifyRoomDimensionsEW(new Vector3(eastWestMidpoint, 0.5f, camObj.transform.position.z + (roomDim.y / 2f) - 1.5f), new Vector3(1, 0,0));
			}
			else
			{
				VerifyRoomDimensionsNS(new Vector3(camObj.transform.position.x + (roomDim.x / 2) - 1.5f, 0.5f, northSouthMidpoint), new Vector3(0,0,1));
				VerifyRoomDimensionsEW(new Vector3(eastWestMidpoint, 0.5f, camObj.transform.position.z + (roomDim.y / 2f) - 1.5f), new Vector3(1, 0,0));
			}
			GetRoomDimensions();
		}
		else
		{
			StartCoroutine(Transition(cameraMoveTime));
			StartCoroutine(FreezePlayer(cameraMoveTime));
			Vector3 start = camObj.transform.position;
			Vector3 end = Vector3.zero;
			Vector3 playerStart = player.transform.position;
			Vector3 playerEnd = Vector3.zero;
			float fPositionX = 0; 
			float fPositionZ = 0;
			
			if(moveCamera == MainTrigger.MoveCameraDirection.North)
			{
				fPositionX = camObj.transform.position.x;
				fPositionZ = camObj.transform.position.z + roomDim.y;
				playerEnd = player.transform.position + new Vector3(0.0f,0.0f,MovePlayer());
			}
			else if(moveCamera == MainTrigger.MoveCameraDirection.South)
			{
				fPositionX = camObj.transform.position.x;
				fPositionZ = camObj.transform.position.z - roomDim.y;
				playerEnd = player.transform.position + new Vector3(0.0f,0.0f,-MovePlayer());
			}
			else if(moveCamera == MainTrigger.MoveCameraDirection.East)
			{
				fPositionX = camObj.transform.position.x + roomDim.x;
				fPositionZ = camObj.transform.position.z;
				playerEnd = player.transform.position + new Vector3(MovePlayer(),0.0f,0.0f);
			}
			else if(moveCamera == MainTrigger.MoveCameraDirection.West)
			{
				fPositionX = camObj.transform.position.x - roomDim.x;
				fPositionZ = camObj.transform.position.z;
				playerEnd = player.transform.position + new Vector3(-MovePlayer(),0.0f,0.0f);
			}
			
			end = new Vector3(fPositionX, camObj.transform.position.y, fPositionZ);
			
			float rate = 1.0f/cameraMoveTime;
			float t = 0;
			while (t < 1.0f)
			{
				t += Time.deltaTime * rate;
				camObj.transform.position = Vector3.Lerp(start, end, t);
				player.transform.position = Vector3.Lerp(playerStart, playerEnd, t);
				yield return 0;
			}
		}
		////////////////////////////////////
		cPos = camObj.transform.position;
		cPosX = camObj.transform.position.x;
		cPosZ = camObj.transform.position.z;
	}

	private IEnumerator FreezePlayer(float delay)
	{			
		playerScript.enabled = false;
		yield return new WaitForSeconds(delay + 0.05f);
		playerScript.enabled = true;
	}
	
	private IEnumerator Transition(float wait)
	{
		inTransition = true;
		yield return new WaitForSeconds(wait + 0.05f);
		cPos = camObj.transform.position;
		inTransition = false;	
	}
	
	private float MovePlayer()
	{
		float moveBy;
		
		if(isPerspectiveMode) moveBy = 2.0f * playerColliderSize + 0.05f;
		else moveBy = playerColliderSize + 0.05f;
		
		return moveBy;
	}
	
	private bool northLockCheck, southLockCheck, smallerY;
	private void CheckNextRoomNS(Vector3 origin, Vector3 direction)
	{
		bool vCheck1 = false;
		bool vCheck2 = false;
		RaycastHit hit1, hit2, hit3, hit4;
		if(Physics.Raycast(origin, direction, out hit1, Mathf.Infinity, layerMask))
		{	 
			if(hit1.distance + 1f <= roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				northLockCheck = true;
				northDifference = ((roomDim.y/2f) - (hit1.distance + 1f));
			}
			else
			{
				//room is larger than bounds in y direction
				northLockCheck = false;
			}
		} debugLine1 = origin; debugLine2 = hit1.point;
		if(Physics.Raycast(origin, -direction, out hit2, Mathf.Infinity, layerMask))
		{	 
			if(hit2.distance + 1f <= roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				southLockCheck = true;
				southDifference = ((roomDim.y/2f) - (hit2.distance + 1f));
			}
			else
			{
				//room is larger than bounds in y direction
				southLockCheck = false;
			}
		} debugLine3 = origin; debugLine4 = hit2.point;
		
		northSouthMidpoint = (hit1.point.z + hit2.point.z) / 2f;
		
		
		if(Physics.Raycast(new Vector3(origin.x, origin.y, northSouthMidpoint), direction, out hit3, Mathf.Infinity, layerMask))
		{	 
			if(hit3.distance < roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				northLock = true;
				vCheck1 = true;
			}
			else
			{
				//room is larger than bounds in y direction
				northLock = false;
				vCheck1 = false;
			}
		}
		if(Physics.Raycast(new Vector3(origin.x, origin.y, northSouthMidpoint), -direction, out hit4, Mathf.Infinity, layerMask))
		{	 
			if(hit4.distance < roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				southLock = true;
				vCheck2 = true;
			}
			else
			{
				//room is larger than bounds in y direction
				southLock = false;
				vCheck2 = false;
			}
		}
		if(vCheck1 && vCheck2)
			smallerY = true;
		else
			smallerY = false;
		
		
	}
	private bool eastLockCheck, westLockCheck, smallerX;
	private void CheckNextRoomEW(Vector3 origin, Vector3 direction)
	{
		bool hCheck1 = false;
		bool hCheck2 = false;
		RaycastHit hit1, hit2, hit3, hit4;
		if(Physics.Raycast(origin, direction, out hit1, Mathf.Infinity, layerMask))
		{
			if(hit1.distance + 1f <= roomDim.x / 2f)
			{
				//room is smaller than bounds in x direction
				eastDifference = (roomDim.x / 2f) - (hit1.distance + 1f);
				eastLockCheck = true;
			}
			else
			{
				//room is larger than bounds in x direction
				eastLockCheck = false;
			}
		} debugLine5 = origin; debugLine6 = hit1.point;

		if(Physics.Raycast(origin, -direction, out hit2, Mathf.Infinity, layerMask))
		{
			if(hit2.distance + 1f <= (roomDim.x / 2f))
			{
				//room is smaller than bounds in x direction
				westDifference = (roomDim.x / 2f) - (hit2.distance + 1f);
				westLockCheck = true;
			}
			else
			{
				//room is larger than bounds in x direction
				westLockCheck = false;
			}
		} debugLine7 = origin; debugLine8 = hit2.point;

		eastWestMidpoint = (hit1.point.x + hit2.point.x) / 2f;
		
		if(Physics.Raycast(new Vector3(eastWestMidpoint, origin.y, origin.z), direction, out hit3, Mathf.Infinity, layerMask))
		{
			if(hit3.distance < roomDim.x / 2f)
			{
				//room is smaller than bounds in x direction
				eastLock = true;
				hCheck1 = true;
			}
			else
			{
				//room is larger than bounds in x direction
				eastLock = false;
				hCheck1 = false;
			}
		}
		if(Physics.Raycast(new Vector3(eastWestMidpoint, origin.y, origin.z), -direction, out hit4, Mathf.Infinity, layerMask))
		{
			if(hit4.distance < roomDim.x / 2f)
			{
				//room is smaller than bounds in x direction
				westLock = true;
				hCheck2 = true;
			}
			else
			{
				//room is larger than bounds in x direction
				westLock = false;
				hCheck2 = false;
			}
		}
		if(hCheck1 && hCheck2)
			smallerX = true;
		else
			smallerX = false;	
	}
	
	private void VerifyRoomDimensionsEW(Vector3 origin, Vector3 direction)
	{
		bool hCheck1 = false;
		bool hCheck2 = false;
		RaycastHit hit1, hit2;
		if(Physics.Raycast(origin, direction, out hit1, Mathf.Infinity, layerMask))
		{
			if(hit1.distance + 1f <= roomDim.x / 2f)
			{
				//room is smaller than bounds in x direction
				hCheck1 = true;
			}
			else
			{
				//room is larger than bounds in x direction
				hCheck1 = false;
			}
		} 
		if(Physics.Raycast(origin, -direction, out hit2, Mathf.Infinity, layerMask))
		{
			if(hit2.distance + 1f <= (roomDim.x / 2f))
			{
				//room is smaller than bounds in x direction
				hCheck2 = true;
			}
			else
			{
				//room is larger than bounds in x direction
				hCheck2 = false;
			}
		} 
		if(hCheck1 && hCheck2)
			smallerX = true;
		else
			smallerX = false;
		
		if(drawDebugLines)
		{
			debugLine5 = origin; debugLine6 = hit1.point;
			debugLine7 = origin; debugLine8 = hit2.point;
		}
	}
	
	private void VerifyRoomDimensionsNS(Vector3 origin, Vector3 direction)
	{
		bool vCheck1 = false;
		bool vCheck2 = false;
		RaycastHit hit1, hit2;
		if(Physics.Raycast(origin, direction, out hit1, Mathf.Infinity, layerMask))
		{	 
			if(hit1.distance + 1f <= roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				vCheck1 = true;
			}
			else
			{
				//room is larger than bounds in y direction
				vCheck1 = false;
			}
		}
		if(Physics.Raycast(origin, -direction, out hit2, Mathf.Infinity, layerMask))
		{	 
			if(hit2.distance + 1f <= roomDim.y /2f)
			{
				//room is smaller than bounds in y direction
				vCheck2 = true;
			}
			else
			{
				//room is larger than bounds in y direction
				vCheck2 = false;
			}
		} 
		if(vCheck1 && vCheck2)
			smallerY = true;
		else
			smallerY = false;
		
		if(drawDebugLines)
		{
			debugLine1 = origin; debugLine2 = hit1.point;
			debugLine3 = origin; debugLine4 = hit2.point;
		}
	}
	
	private void GetRoomDimensions()
	{
		if(!smallerX && !smallerY)
			roomDimension = MainTrigger.RoomDimensions.LARGER_X_LARGER_Y;
		else if(smallerX && smallerY)
			roomDimension = MainTrigger.RoomDimensions.SMALLER_X_SMALLER_Y;
		else if(smallerX && !smallerY)
			roomDimension = MainTrigger.RoomDimensions.SMALLER_X_LARGER_Y;
		else if(!smallerX && smallerY)
			roomDimension = MainTrigger.RoomDimensions.LARGER_X_SMALLER_Y;
	}
	
	private int RoundToTens(float a)
	{
		int num = 0;
		
		if(Mathf.Abs(Mathf.Round(a) - a) < 0.5f)
			num = (int)Mathf.Round(a);
		else if(Mathf.Abs(Mathf.Round(a) - a) >= 0.5f)
			num = (int)Mathf.Round(a) + 1;
		
		return num;
	}
	
	public void SwitchMode()
	{
		StartCoroutine(SwitchModes());
	}
	
	private IEnumerator SwitchModes()
	{	
		if(cameraMode == CamMode.Dungeon)
		{
			yield return new WaitForSeconds(cameraMoveTime);
			cameraMode = CamMode.Overworld;
		}
		else
			cameraMode = CamMode.Dungeon;
	}
}