using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Toolset : EditorWindow {

	private bool snapToCenter = false;
	private bool snapToBounds = false;
	
	private Vector3 newPosition;
	private Vector3 startPosition;
	
	private Vector3 lowBound, highBound;
		
	private string parentName = "Parent Name";
	
	private GameObject camera;
	private Transform[] cameraChildren;
	
	private GameObject helperGrid;
	private Transform[] helperGridChildren;
	
	private bool showLayers = false;
	private bool showTags = false;
	private bool freezeGrid = false;
	private bool showMakeChild = false;
	private bool showGridSnaps = false;
	
	private Vector2 scrollPos;
	private int buttonWidth = 125;
		
	[MenuItem ("GameObject/UniVenture/Toolset")]
	private static void Init()
    {
        Toolset uniVentureToolsetWindow = (Toolset)EditorWindow.GetWindow(typeof(Toolset));
        uniVentureToolsetWindow.position = new Rect(20, 150, 150, 350);
    }
	
	void Awake()
	{
		FindCameraAndChildren();
		GetHelperGrid();
	}
	
	void Update()
	{
		if(EditorApplication.isPlaying)
			return;
		
		if(snapToCenter || snapToBounds)
			SnapTo();
		
		if(helperGrid != null)
		{
			if(freezeGrid)
			{
				foreach(Transform t in helperGridChildren)
				{
					if(Selection.activeTransform == t.transform)
					   Selection.activeTransform = null;
				}
			}
		}
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		EditorGUILayout.Separator();
		if(GUILayout.Button("Show/Hide Grid", GUILayout.Width(buttonWidth)))
		{
			ShowHideGrid();
		}
		
		EditorGUILayout.BeginHorizontal();
		freezeGrid = EditorGUILayout.Toggle(freezeGrid, GUILayout.Width(10));
		EditorGUILayout.PrefixLabel("Freeze Grid");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Move Snap To:");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		ToggleCenter(EditorGUILayout.Toggle(snapToCenter, GUILayout.Width(10)));
		EditorGUILayout.PrefixLabel("Full Increment");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		ToggleBounds(EditorGUILayout.Toggle(snapToBounds, GUILayout.Width(10)));
		EditorGUILayout.PrefixLabel("1/2 Increment");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		showGridSnaps = EditorGUILayout.Foldout(showGridSnaps, "Snap to Grid");
		if(showGridSnaps)
		{
			if(GUILayout.Button("All Objects", GUILayout.Width(buttonWidth)))
			{
				if(EditorUtility.DisplayDialog("Are you sure?",
						"This will snap all objects in the scene to the grid." + "\nThis action cannot be undone.",
						"Yes", "No"))
				{
					SnapAllObjectsToGrid();	
				}
			}
			if(GUILayout.Button("Selected Objects", GUILayout.Width(buttonWidth)))
			{
				SnapSelectedObjectsToGrid();
			}
		}
		
		EditorGUILayout.Separator();
		
		//EditorGUILayout.BeginHorizontal();
		showMakeChild = EditorGUILayout.Foldout(showMakeChild, "Make Child");
		if(showMakeChild)
		{
			parentName = EditorGUILayout.TextField(parentName, GUILayout.Width(buttonWidth));
			
			if(GUILayout.Button("Make Child", GUILayout.Width(buttonWidth)))
			{
				MakeSelectionChild(parentName);
			}
		}
		
		
		EditorGUILayout.Separator();
	
		
		showLayers = EditorGUILayout.Foldout(showLayers, "Layers");
		if(showLayers)
		{
			GUILayout.Label("Change Layer To:", GUILayout.Width(buttonWidth));
			if(GUILayout.Button("Wall", GUILayout.Width(buttonWidth)))
				ChangeLayerToWall();
			if(GUILayout.Button("Invisible Bound", GUILayout.Width(buttonWidth)))
		   		ChangeLayerToInvisibleBound();
			if(GUILayout.Button("Default", GUILayout.Width(buttonWidth)))
				ChangeLayerToDefault();
		}
		EditorGUILayout.Separator();
		showTags = EditorGUILayout.Foldout(showTags, "Tags");
		if(showTags)
		{
			GUILayout.Label("Change Tag To:", GUILayout.Width(buttonWidth));
			if(GUILayout.Button("HorizontalWall", GUILayout.Width(buttonWidth)))
				ChangeTagToHorWall();
			if(GUILayout.Button("VerticalWall", GUILayout.Width(buttonWidth)))
				ChangeTagToVertWall();
		}
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
	
	private void ShowHideGrid()
	{
		GameObject go = helperGrid;
		Transform[] children = helperGridChildren;
		
		if(go == null)
			EditorUtility.DisplayDialog("Notice", "Could not find HelperGrid. Make sure one exists in scene and re-open toolset.", "Okay");
		else
		{
			
			if(go.active == true)
			{
				foreach(Transform child in children)
				{
					child.gameObject.active = false;
				}
				go.active = false;
			}
			else
			{
				go.active = true;
				foreach(Transform child in children)
				{
					child.gameObject.active = true;
				}
				
			}
		}
	}
	
	private void GetHelperGrid()
	{
		helperGrid = GameObject.Find("HelperGrid");
		if(helperGrid)
			helperGridChildren = helperGrid.GetComponentsInChildren<Transform>();
	}
	
	private void FindCameraAndChildren()
	{
		camera = GameObject.FindWithTag("MainCamera") as GameObject;
		cameraChildren = camera.GetComponentsInChildren<Transform>();
	}
	
	private void ToggleCenter(bool state)
	{
		if(state)
		{
			ToggleBounds(false);	
		}
		snapToCenter = state;
	}
	
	private void ToggleBounds(bool state)
	{
		if(state)
		{
			ToggleCenter(false);	
		}
		snapToBounds = state;
	}
	
	private void SnapTo()
	{
		if(snapToCenter)
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        	float gridx = 1.0f;
        	float gridy = 1.0f;
        	float gridz = 1.0f;
        
        	foreach (Transform transform in transforms)
        	{
				if(transform == camera.transform)
					return;
				
				foreach(Transform t in cameraChildren)
				{
					if(transform == t)
						return;
				}
				
            	newPosition = transform.position;
            	newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            	newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            	newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            	transform.position = newPosition;
        	}
		}
		else if(snapToBounds)
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        	float gridx = 0.5f;
        	float gridy = 0.5f;
        	float gridz = 0.5f;
        
        	foreach (Transform transform in transforms)
        	{
				if(transform == camera.transform)
					return;
				
				foreach(Transform t in cameraChildren)
				{
					if(transform == t)
						return;
				}
				
            	newPosition = transform.position;
            	newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            	newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            	newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            	transform.position = newPosition;
        	}
		}	
	}
	
	private void SnapAllObjectsToGrid()
	{
		Transform[] transforms = FindObjectsOfType(typeof(Transform)) as Transform[];
        
        float gridx, gridy, gridz;

        foreach (Transform transform in transforms)
        {
        	if(transform == camera.transform)
        		return;
        	
			foreach(Transform t in cameraChildren)
			{
				if(transform == t)
					return;
			}
			
        	lowBound = transform.position + new Vector3((float)(1/3), (float)(1/3), (float)(1/3));
        	highBound = transform.position + new Vector3((float)(2/3), (float)(2/3), (float)(2/3));
        	
        	if(transform.position.x > lowBound.x || transform.position.x < highBound.x)
        		gridx = 1.0f;
        	else gridx = 0.5f;
        	
        	if(transform.position.y > lowBound.y || transform.position.y < highBound.y)
        		gridy = 1.0f;
        	else gridy = 0.5f;
        	
        	if(transform.position.z > lowBound.z || transform.position.z < highBound.z)
        		gridz = 1.0f;
        	else gridz = 0.5f;
        	
            newPosition = transform.position;
            newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            transform.position = newPosition;
        }
	}
	
	private void SnapSelectedObjectsToGrid()
	{
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        float gridx, gridy, gridz;
        
        foreach (Transform transform in transforms)
        {
        	if(transform == camera.transform)
        		return;
        	
			foreach(Transform t in cameraChildren)
			{
				if(transform == t)
					return;
			}
			
        	lowBound = transform.position + new Vector3((float)(1/3), (float)(1/3), (float)(1/3));
        	highBound = transform.position + new Vector3((float)(2/3), (float)(2/3), (float)(2/3));
        	
        	if(transform.position.x > lowBound.x || transform.position.x < highBound.x)
        		gridx = 1.0f;
        	else gridx = 0.5f;
        	
        	if(transform.position.y > lowBound.y || transform.position.y < highBound.y)
        		gridy = 1.0f;
        	else gridy = 0.5f;
        	
        	if(transform.position.z > lowBound.z || transform.position.z < highBound.z)
        		gridz = 1.0f;
        	else gridz = 0.5f;
        	
            newPosition = transform.position;
            newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            transform.position = newPosition;
        }
	}
	
	private void MakeSelectionChild(string parentName)
	{
		GameObject parent = new GameObject(parentName);
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			gameObject.transform.parent = parent.transform;
		}	
	}
	
	private void ChangeLayerToWall()
	{
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			if(gameObject.layer != LayerMask.NameToLayer("Wall")) gameObject.layer = LayerMask.NameToLayer("Wall");
		}
	}
	
	private void ChangeLayerToInvisibleBound()
	{
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			if(gameObject.layer != LayerMask.NameToLayer("InvisibleBound")) gameObject.layer = LayerMask.NameToLayer("InvisibleBound");
		}
	}
	
	private void ChangeLayerToDefault()
	{
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			if(gameObject.layer != LayerMask.NameToLayer("Default")) gameObject.layer = LayerMask.NameToLayer("Default");
		}	
	}
	
	private void ChangeTagToHorWall()
	{
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			if(gameObject.tag != "HorizontalWall") gameObject.tag = "HorizontalWall";
		}	
	}
	private void ChangeTagToVertWall()
	{
		Object[] gameObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
		
		foreach(GameObject gameObject in gameObjects)
		{
			if(gameObject.tag != "VerticalWall") gameObject.tag = "VerticalWall";
		}	
	}
}