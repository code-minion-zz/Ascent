using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UniVentureCam : EditorWindow {

	public enum Projection
	{
		Perspective,
		Orthographic
	}

	public enum C_AspectRatio
	{
		FourThree,
		FiveFour,
		ThreeTwo,
		SixteenTen,
		SixteenNine
	}
	
	private Camera camera;
	private Projection projection;
	
	private float camHeight;
	
	private string camName = "UniVentureCam";
	
	private int aspectRatioNumerator;
	private int aspectRatioDenominator;
	
	private int fov = 60;
	private float perspCamHeight;
	
	private int gridScale = 2;
	private float gridX, gridY;
	private Vector2 gridDimensions;
	
	private GameObject cardinalPoints;
	private GameObject cardinalBounds;
	
	private GameObject mainTrigger;
	private GameObject northTrigger;
	private GameObject southTrigger;
	private GameObject eastTrigger;
	private GameObject westTrigger;
	private GameObject nBound;
	private GameObject sBound;
	private GameObject eBound;
	private GameObject wBound;
	private GameObject lightObj;
	
	private GameObject invisibleBounds;
	private GameObject invisibleBoundNorth;
	private GameObject invisibleBoundSouth;
	private GameObject invisibleBoundEast;
	private GameObject invisibleBoundWest;
	
	private GameObject grid;
	
	private C_AspectRatio currentAspectRatio;
	
	[MenuItem ("GameObject/UniVenture/Create Camera")]
	private static void Init()
    {
        UniVentureCam uniVentureCamToolWindow = (UniVentureCam)EditorWindow.GetWindow(typeof(UniVentureCam));
        uniVentureCamToolWindow.position = new Rect(50, 150, 275, 230);
    }
	
	void OnInspectorUpdate () 
	{
        Repaint ();
		
		gridDimensions = new Vector2(gridX, gridY);
    }
	
	private void OnGUI()
	{
		CurrentAspectRatio();
		
		EditorGUILayout.Separator();
		
		camera = (Camera)EditorGUILayout.ObjectField("Main Camera:", camera, typeof(Camera), true);
		
		EditorGUILayout.BeginHorizontal();
		camName = EditorGUILayout.TextField("Camera Name: ", camName);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Aspect Ratio:", GUILayout.Width(125));
		currentAspectRatio = (C_AspectRatio)EditorGUILayout.EnumPopup(currentAspectRatio);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Projection:", GUILayout.Width(125));
        projection = (Projection)EditorGUILayout.EnumPopup(projection);
        EditorGUILayout.EndHorizontal();
		
		if (projection == Projection.Perspective)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("FOV: ", GUILayout.Width(35));
			fov = EditorGUILayout.IntSlider(fov, 1, 179);
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Grid Scale:", GUILayout.Width(125));
		gridScale = EditorGUILayout.IntSlider(gridScale, 1, 10);
		EditorGUILayout.EndHorizontal();
			
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Grid Size:" , GUILayout.Width(125));
		GUILayout.Label(gridDimensions.x + " X " + gridDimensions.y, GUILayout.Width(125));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Create Camera"))
        {
            if (ValidateCam() && IsMainCamera())
			{
				SetAttributes();
			}
        }
				
		if(GUILayout.Button("Generate Grid"))
			GenerateGrid();
		
		////For debugging erase from final code
		if(GUILayout.Button("Restore Cam Data"))
			StoreData();
	}
	
	private bool ValidateCam()
	{
		if(!camera)
        {
			EditorUtility.DisplayDialog("Notice:",
					"Please select a main camera.",
					"Okay");
            camName = "UniVentureCam";
            return false;
        }
		else
		{
			camera.name = camName;
			return true;
		}
	}
	
	private bool IsMainCamera()
	{
		if(camera.gameObject.CompareTag("MainCamera"))
		{
			return true;
		}
		else
		{
			EditorUtility.DisplayDialog("Notice:",
					"The camera you have selected is not the main camera. Please change the tag to MainCamera.",
					"Okay");
			return false;
		}
	}
	
	private void SetAttributes()
	{
		StoreData();
		SetProjection();
		SetPositionAndRotation();
		CreateTriggers();
	}
	
	private float CalcPerspSize()
	{
		perspCamHeight = (2.0f * (float)aspectRatioDenominator * (float)gridScale) / (2.0f * Mathf.Tan((float)fov * Mathf.Deg2Rad * 0.5f));
		
		return perspCamHeight;
	}
	
	private int CalcOrthoSize()
	{
		int orthoSize;
		orthoSize = aspectRatioDenominator * gridScale;
		
		return orthoSize;
	}
	
	private float CalcTriggerPosNS()
	{
		float trigPos;
		if(projection == Projection.Perspective)
		{
			trigPos = (float)(aspectRatioDenominator * gridScale - 0.5f);
		}
		else
		{
			trigPos = (float)(aspectRatioDenominator * gridScale);
		}
		return trigPos;
	}
	
	private float CalcTriggerPosEW()
	{
		float trigPos;
		if(projection == Projection.Perspective)
		{
			trigPos = (float)(aspectRatioNumerator * gridScale - 0.5f);
		}
		else
		{
			trigPos = (float)(aspectRatioNumerator * gridScale);
		}
		return trigPos;
	}
	
	private float CurrentAspectRatio()
	{
		switch(currentAspectRatio)
			{
				case C_AspectRatio.FiveFour:
				{
					aspectRatioNumerator = 5;
					aspectRatioDenominator = 4;
					break;
				}
				case C_AspectRatio.FourThree:
				{
					aspectRatioNumerator = 4;
					aspectRatioDenominator = 3;
					break;
				}
				case C_AspectRatio.ThreeTwo:
				{
					aspectRatioNumerator = 3;
					aspectRatioDenominator = 2;
					break;
				}
				case C_AspectRatio.SixteenTen:
				{
					aspectRatioNumerator = 16;
					aspectRatioDenominator = 10;
					break;
				}
				case C_AspectRatio.SixteenNine:
				{
					aspectRatioNumerator = 16;
					aspectRatioDenominator = 9;
					break;
				}
			}
		float _aspectRatioNumerator;
		_aspectRatioNumerator = aspectRatioNumerator;

		gridX =  2 * aspectRatioNumerator * gridScale;
		gridY =  2 * aspectRatioDenominator * gridScale;
		
		return _aspectRatioNumerator;
	}
	
	private void SetPositionAndRotation()
	{
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
		camera.transform.rotation = rotation;
		
		if(projection == Projection.Perspective)
		{
			camera.transform.position = new Vector3(0.0f, CalcPerspSize(), 0.0f);
			camera.fieldOfView = fov;
		}
		else if(projection == Projection.Orthographic)
		{
			camera.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
			camera.orthographicSize = CalcOrthoSize();
		}
	}
	
	private void SetProjection()
	{
		switch(projection)
		{
			case Projection.Perspective:
			{
				camera.orthographic = false;
				break;
			}
			case Projection.Orthographic:
			{
				camera.orthographic = true;
				break;
			}
		}
	}
	
	private void CreateTriggers()
	{
		/*-----Cardinal Points Parent-----*/
		if(cardinalPoints) DestroyImmediate(cardinalPoints.gameObject);
		cardinalPoints = new GameObject("Cardinal Points").gameObject;
		cardinalPoints.transform.parent = camera.transform;
		
		/*-----Cardinal Bounds Parent-----*/
		if(cardinalBounds) DestroyImmediate(cardinalBounds.gameObject);
		cardinalBounds = new GameObject("Cardinal Bounds").gameObject;
		cardinalBounds.transform.parent = camera.transform;
		
		/*-----Main Trigger-----*/
		if(mainTrigger) DestroyImmediate(mainTrigger.gameObject);
		mainTrigger = new GameObject("_MainTrigger").gameObject;
		mainTrigger.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
		mainTrigger.transform.parent = camera.transform;
		mainTrigger.AddComponent<BoxCollider>();
		BoxCollider mainTriggerCollider = mainTrigger.GetComponent(typeof(BoxCollider)) as BoxCollider;
		mainTriggerCollider.isTrigger = true;
		mainTriggerCollider.size = new Vector3((float)aspectRatioNumerator * gridScale * 2.0f - 2.0f, 10f, (float)aspectRatioDenominator * gridScale * 2.0f - 2.0f);
		mainTriggerCollider.center = new Vector3(0f, 4.5f, 0f);
		mainTrigger.AddComponent<Rigidbody>();
		mainTrigger.rigidbody.useGravity = false;
		mainTrigger.rigidbody.isKinematic = true;
		mainTrigger.AddComponent<MainTrigger>();
		
		/*-----North Trigger-----*/
		if(northTrigger) DestroyImmediate(northTrigger.gameObject);
		northTrigger = new GameObject("NorthTrigger").gameObject;
		northTrigger.transform.parent = cardinalPoints.transform;
		northTrigger.transform.position = new Vector3(0.0f, 0.5f, 1.0f * CalcTriggerPosNS());
		northTrigger.AddComponent<BoxCollider>();
		BoxCollider northTriggerCollider = northTrigger.GetComponent(typeof(BoxCollider)) as BoxCollider;
		northTriggerCollider.isTrigger = true;
		northTriggerCollider.size = new Vector3((float)aspectRatioNumerator * gridScale * 2.0f, 10f, 0.00001f);
		northTriggerCollider.center = new Vector3(0f, 4.5f, 0f);
		northTrigger.AddComponent<Rigidbody>();
		northTrigger.rigidbody.useGravity = false;
		northTrigger.rigidbody.isKinematic = true;
		northTrigger.AddComponent<nTrigger>();
		
		/*-----South Trigger-----*/
		if(southTrigger) DestroyImmediate(southTrigger.gameObject);
		southTrigger = new GameObject("SouthTrigger").gameObject;
		southTrigger.transform.parent = cardinalPoints.transform;
		southTrigger.transform.position = new Vector3(0.0f, 0.5f, -1.0f * CalcTriggerPosNS());
		southTrigger.AddComponent<BoxCollider>();
		BoxCollider southTriggerCollider = southTrigger.GetComponent(typeof(BoxCollider)) as BoxCollider;
		southTriggerCollider.isTrigger = true;
		southTriggerCollider.size = new Vector3((float)aspectRatioNumerator * gridScale * 2.0f, 10f, 0.00001f);
		southTriggerCollider.center = new Vector3(0f, 4.5f, 0f);
		southTrigger.AddComponent<Rigidbody>();
		southTrigger.rigidbody.useGravity = false;
		southTrigger.rigidbody.isKinematic = true;
		southTrigger.AddComponent<sTrigger>();
		
		/*-----East Trigger-----*/
		if(eastTrigger) DestroyImmediate(eastTrigger.gameObject);
		eastTrigger = new GameObject("EastTrigger").gameObject;
		eastTrigger.transform.parent = cardinalPoints.transform;
		eastTrigger.transform.position = new Vector3(1.0f * CalcTriggerPosEW(), 0.5f, 0.0f);
		eastTrigger.AddComponent<BoxCollider>();
		BoxCollider eastTriggerCollider = eastTrigger.GetComponent(typeof(BoxCollider)) as BoxCollider;
		eastTriggerCollider.isTrigger = true;
		eastTriggerCollider.size = new Vector3(0.00001f, 10f, (float)aspectRatioDenominator * gridScale * 2.0f);
		eastTriggerCollider.center = new Vector3(0f, 4.5f, 0f);
		eastTrigger.AddComponent<Rigidbody>();
		eastTrigger.rigidbody.useGravity = false;
		eastTrigger.rigidbody.isKinematic = true;
		eastTrigger.AddComponent<eTrigger>();
		
		/*-----West Trigger-----*/
		if(westTrigger) DestroyImmediate(westTrigger.gameObject);
		westTrigger = new GameObject("WestTrigger").gameObject;
		westTrigger.transform.parent = cardinalPoints.transform;
		westTrigger.transform.position = new Vector3(-1.0f * CalcTriggerPosEW(), 0.5f, 0.0f);
		westTrigger.AddComponent<BoxCollider>();
		BoxCollider westTriggerCollider = westTrigger.GetComponent(typeof(BoxCollider)) as BoxCollider;
		westTriggerCollider.isTrigger = true;
		westTriggerCollider.size = new Vector3(0.00001f, 10f, (float)aspectRatioDenominator * gridScale * 2.0f);
		westTriggerCollider.center = new Vector3(0f, 4.5f, 0f);
		westTrigger.AddComponent<Rigidbody>();
		westTrigger.rigidbody.useGravity = false;
		westTrigger.rigidbody.isKinematic = true;
		westTrigger.AddComponent<wTrigger>();
		
		/*-----North Bound-----*/
		if(nBound) DestroyImmediate(nBound.gameObject);
		nBound = new GameObject("nBound").gameObject;
		nBound.transform.parent = cardinalBounds.transform;
		nBound.transform.position = new Vector3(0.0f, 0.5f, 1.0f * (float)(aspectRatioDenominator * gridScale - 1.0f));
		nBound.AddComponent<BoxCollider>();
		BoxCollider nBoundCollider = nBound.GetComponent(typeof(BoxCollider)) as BoxCollider;
		nBoundCollider.isTrigger = true;
		nBoundCollider.size = new Vector3((float)aspectRatioNumerator * gridScale * 2.0f - 2.5f, 10f, 0.00001f);
		nBoundCollider.center = new Vector3(0f, 4.5f, 0f);
		nBound.AddComponent<Rigidbody>();
		nBound.rigidbody.useGravity = false;
		nBound.rigidbody.isKinematic = true;
		nBound.AddComponent<nCross>();
		
		/*-----South Bound-----*/
		if(sBound) DestroyImmediate(sBound.gameObject);
		sBound = new GameObject("sBound").gameObject;
		sBound.transform.parent = cardinalBounds.transform;
		sBound.transform.position = new Vector3(0.0f, 0.5f, -1.0f * (float)(aspectRatioDenominator * gridScale - 1.0f));
		sBound.AddComponent<BoxCollider>();
		BoxCollider sBoundCollider = sBound.GetComponent(typeof(BoxCollider)) as BoxCollider;
		sBoundCollider.isTrigger = true;
		sBoundCollider.size = new Vector3((float)aspectRatioNumerator * gridScale * 2.0f - 2.5f, 10f, 0.00001f);
		sBoundCollider.center = new Vector3(0f, 4.5f, 0f);
		sBound.AddComponent<Rigidbody>();
		sBound.rigidbody.useGravity = false;
		sBound.rigidbody.isKinematic = true;
		sBound.AddComponent<sCross>();
		
		/*-----East Bound-----*/
		if(eBound) DestroyImmediate(eBound.gameObject);
		eBound = new GameObject("eBound").gameObject;
		eBound.transform.parent = cardinalBounds.transform;
		eBound.transform.position = new Vector3(1.0f * (float)(aspectRatioNumerator * gridScale - 1.0f), 0.5f, 0.0f);
		eBound.AddComponent<BoxCollider>();
		BoxCollider eBoundCollider = eBound.GetComponent(typeof(BoxCollider)) as BoxCollider;
		eBoundCollider.isTrigger = true;
		eBoundCollider.size = new Vector3(0.00001f, 10f, (float)aspectRatioDenominator * gridScale * 2.0f - 2.5f);
		eBoundCollider.center = new Vector3(0f, 4.5f, 0f);
		eBound.AddComponent<Rigidbody>();
		eBound.rigidbody.useGravity = false;
		eBound.rigidbody.isKinematic = true;
		eBound.AddComponent<eCross>();
		
		/*-----West Bound-----*/
		if(wBound) DestroyImmediate(wBound.gameObject);
		wBound = new GameObject("wBound").gameObject;
		wBound.transform.parent = cardinalBounds.transform;
		wBound.transform.position = new Vector3(-1.0f * (float)(aspectRatioNumerator * gridScale - 1.0f), 0.5f, 0.0f);
		wBound.AddComponent<BoxCollider>();
		BoxCollider wBoundCollider = wBound.GetComponent(typeof(BoxCollider)) as BoxCollider;
		wBoundCollider.isTrigger = true;
		wBoundCollider.size = new Vector3(0.00001f, 10f, (float)aspectRatioDenominator * gridScale * 2.0f - 2.5f);
		wBoundCollider.center = new Vector3(0f, 4.5f, 0f);
		wBound.AddComponent<Rigidbody>();
		wBound.rigidbody.useGravity = false;
		wBound.rigidbody.isKinematic = true;
		wBound.AddComponent<wCross>();
		
		/*-----Default Light-----*/
		if(lightObj) DestroyImmediate(lightObj.gameObject);
		lightObj = new GameObject("pointlight").gameObject;
		lightObj.AddComponent<Light>();
		lightObj.gameObject.light.type = LightType.Point;
		if(projection == Projection.Perspective)
			lightObj.transform.position = new Vector3(0, CalcPerspSize(), 0);
		else if(projection == Projection.Orthographic)
			lightObj.transform.position = new Vector3(0, 10, 0);
		lightObj.gameObject.light.range = 20f;
		lightObj.transform.parent = camera.transform;
		
		/*-----InvisibleBounds-----*/
		if(invisibleBounds) DestroyImmediate(invisibleBounds.gameObject);
		if(invisibleBoundNorth) DestroyImmediate(invisibleBoundNorth.gameObject);
		if(invisibleBoundSouth) DestroyImmediate(invisibleBoundSouth.gameObject);
		if(invisibleBoundEast) DestroyImmediate(invisibleBoundEast.gameObject);
		if(invisibleBoundWest) DestroyImmediate(invisibleBoundWest.gameObject);
		invisibleBounds = new GameObject("Temporary Invisible Bounds").gameObject;
		invisibleBoundNorth = new GameObject("North").gameObject;
		invisibleBoundSouth = new GameObject("South").gameObject;
		invisibleBoundEast = new GameObject("East").gameObject;
		invisibleBoundWest = new GameObject("West").gameObject;
		invisibleBoundNorth.transform.position = new Vector3(0f, 0.5f, 1000f);
		invisibleBoundSouth.transform.position = new Vector3(0f, 0.5f, -1000f);
		invisibleBoundEast.transform.position = new Vector3(1000f, 0.5f, 0f);
		invisibleBoundWest.transform.position = new Vector3(-1000f, 0.5f, 0f);
		invisibleBoundNorth.transform.localScale = new Vector3(2000f, 1f, 1f);
		invisibleBoundSouth.transform.localScale = new Vector3(2000f, 1f, 1f);
		invisibleBoundEast.transform.localScale = new Vector3(1f, 1f, 2000f);
		invisibleBoundWest.transform.localScale = new Vector3(1f, 1f, 2000f);
		invisibleBoundNorth.AddComponent<BoxCollider>();
		invisibleBoundSouth.AddComponent<BoxCollider>();
		invisibleBoundEast.AddComponent<BoxCollider>();
		invisibleBoundWest.AddComponent<BoxCollider>();
		invisibleBoundNorth.gameObject.layer = LayerMask.NameToLayer("InvisibleBound");
		invisibleBoundSouth.gameObject.layer = LayerMask.NameToLayer("InvisibleBound");
		invisibleBoundEast.gameObject.layer = LayerMask.NameToLayer("InvisibleBound");
		invisibleBoundWest.gameObject.layer = LayerMask.NameToLayer("InvisibleBound");
		invisibleBoundNorth.transform.parent = invisibleBounds.transform;
		invisibleBoundSouth.transform.parent = invisibleBounds.transform;
		invisibleBoundEast.transform.parent = invisibleBounds.transform;
		invisibleBoundWest.transform.parent = invisibleBounds.transform;
		
		
		Selection.activeGameObject = camera.gameObject;

		EditorGUIUtility.PingObject(camera);
	}
	
	private void GenerateGrid()
	{
		grid = new GameObject("HelperGrid");
		grid.transform.position = Vector3.zero;
		
		GameObject[] gos = new GameObject[121];
		Vector2 frame = new Vector2(aspectRatioNumerator * gridScale, aspectRatioDenominator * gridScale);
		float gridPosx = 0;
		float gridPosy = 0.01f;
		float gridPosz = 0;
		
		for(int i = 0; i < gos.Length; i++)	
		{
			gos[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
			gos[i].name = "grid " + i.ToString();
			gos[i].transform.localScale = new Vector3((frame.x * 2f) / 10f, 1f, (frame.y * 2f) / 10f);
			
			if(i <= 10)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * i * 2f;
				gridPosz = -5f * frame.y * 2f;
			}
			else if(i <= 21)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 11) * 2f;
				gridPosz = -4f * frame.y * 2f;
			}
			else if(i <= 32)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 22) * 2f;
				gridPosz = -3f * frame.y * 2f;
			}
			else if(i <= 43)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 33) * 2f;
				gridPosz = -2f * frame.y * 2f;
			}
			else if(i <= 54)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 44) * 2f;
				gridPosz = -1f * frame.y * 2f;	
			}
			else if(i <= 65)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 55) * 2f;
				gridPosz = 0f * frame.y * 2f;	
			}
			else if(i <= 76)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 66) * 2f;
				gridPosz = 1f * frame.y * 2f;	
			}
			else if(i <= 87)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 77) * 2f;
				gridPosz = 2f * frame.y * 2f;	
			}
			else if(i <= 98)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 88) * 2f;
				gridPosz = 3f * frame.y * 2f;	
			}
			else if(i <= 109)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 99) * 2f;
				gridPosz = 4f * frame.y * 2f;	
			}
			else if(i <= 120)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 110) * 2f;
				gridPosz = 5f * frame.y * 2f;	
			}			
			
			string redMatPath = "Assets/UniVenture/Materials/gridMat1.mat";
			Material redMat = AssetDatabase.LoadAssetAtPath(redMatPath, typeof(Material)) as Material;
			
			string whiteMatPath = "Assets/UniVenture/Materials/gridMat2.mat";
			Material whiteMat = AssetDatabase.LoadAssetAtPath(whiteMatPath, typeof(Material)) as Material;
			
			redMat.mainTextureScale = new Vector2(frame.x * 2, frame.y * 2);
			redMat.mainTextureOffset = new Vector2(0.5f, 0.5f);
			whiteMat.mainTextureScale = new Vector2(frame.x * 2, frame.y * 2);
			whiteMat.mainTextureOffset = new Vector2(0.5f, 0.5f);
			
			if(i % 2 == 0)
				gos[i].renderer.material = redMat;
			else
				gos[i].renderer.material = whiteMat;
			
			gos[i].transform.position = new Vector3(gridPosx, gridPosy, gridPosz);
			gos[i].transform.parent = grid.transform;
		}
		
	}
	
	private void StoreData()
	{
        CameraData camData = (CameraData)ScriptableObject.CreateInstance("CameraData");
        AssetDatabase.CreateAsset(camData, "Assets/camdata.asset");
        EditorUtility.SetDirty(camData);
		camData.roomSize = new Vector2((float)aspectRatioNumerator * gridScale * 2.0f, (float)aspectRatioDenominator * gridScale * 2.0f);
		if(projection == Projection.Perspective) 
		{
			camData.perspectiveProjection = true;
			camData.cameraHeight = CalcPerspSize();
		}
		else 
		{
			camData.perspectiveProjection = false;
			camData.cameraHeight = 10f;
		} 
		
		//camData.hideFlags = HideFlags.HideAndDontSave;
	}
}