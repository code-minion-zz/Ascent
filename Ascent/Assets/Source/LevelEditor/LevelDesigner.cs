using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelDesigner : EditorWindow 
{
	#region Fields
	
	private int buttonWidth = 125;
	private bool snapToCenter = false;
	private bool snapToBounds = false;
	private GameObject grid;
	
	// Creation settings
	private string[] options = new string[] { "Monster", "Player", "Horizontal Wall", "Vertical Wall" };
	private string createObjects = "Create Objects";
	private bool showFoldOutCreateObj = true;
	private int index = 0;
	
	private GameObject gameObject = null;
	private string objectName = "";
	private Vector3 incrementAmount = new Vector3(0.5f, 0.5f, 0.5f);
	
	#endregion
	
	// Initialization of the level designer.
	[MenuItem ("Ascent/Tools/Level Designer")]
	private static void Init()
	{
		// Create and position the window
        LevelDesigner designerWindow = (LevelDesigner)EditorWindow.GetWindow(typeof(LevelDesigner));
        designerWindow.position = new Rect(20, 150, 300, 350);
		
	}
	
	void Awake()
	{
		if (grid == null)
			grid = GetGrid();
	}
	
	private void OnGUI()
	{
		// Begin vertical layout
		EditorGUILayout.BeginVertical();
		
		EditorGUILayout.Separator();
		
		if (GUILayout.Button("Show/Hide Grid", GUILayout.Width(buttonWidth)))
		{
			ShowHideGrid();
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		ToggleBounds(EditorGUILayout.Toggle(snapToBounds, GUILayout.Width(10)));
		EditorGUILayout.PrefixLabel("1/2 Increment");
		EditorGUILayout.EndHorizontal();
		
		// Change tags
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		

		// Begin foldout for creating objects
		showFoldOutCreateObj = EditorGUILayout.Foldout(showFoldOutCreateObj, createObjects);
		
		if (showFoldOutCreateObj)
		{
			EditorGUILayout.BeginVertical();
			
			// Select the object to create
			index = EditorGUILayout.Popup(index, options);
			
			switch (index)
			{
			case 1:
				break;
				
				// Horizontal Wall
			case 2:
				// Assign selected game object prefab
				gameObject = (GameObject)EditorGUILayout.ObjectField(
					"Game Object", gameObject, typeof(GameObject), true);
				
				if (objectName == "")
					objectName = gameObject.name;
				
				// Give the object a name
				objectName = EditorGUILayout.TextField("Name", objectName);
				
				// Set increment amount
				incrementAmount = EditorGUILayout.Vector3Field("Movement Increment Amount:", incrementAmount);				
				
				// Make child of parent				
				break;
				
			case 3:
				break;
				
			default:
				break;
			}
					
			if (GUILayout.Button("Create", GUILayout.Width(buttonWidth)))
				CreateLevelObject();
			
			EditorGUILayout.EndVertical();
		}
		
		// End vertical layout
		EditorGUILayout.EndVertical();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (EditorApplication.isPlaying)
			return;
		
		if (snapToBounds)
			SnapTo();		
	}
	
	#region Designer Functions
			
	private void CreateLevelObject()
	{
		// Instantiate the selected game object.
		GameObject newObj = (GameObject)GameObject.Instantiate(gameObject);
		newObj.name = objectName;
	}

	private void ShowHideGrid()
	{
		if (grid == null)
		{
			Debug.Log("No grid exists please create a grid using the designer.");
			return;
		}
		
		// Enable or disable the parent
		if (grid.activeInHierarchy == true)
		{
			grid.SetActive(false);
		}
		else
		{
			grid.SetActive(true);
		}
	}
	
	private void ToggleCenter(bool state)
	{
		if (state)
		{
			ToggleBounds(false);	
		}
		
		snapToCenter = state;
	}	
	
	private void ToggleBounds(bool state)
	{
		if (state)
		{
			ToggleCenter(false);	
		}
		
		snapToBounds = state;
	}	
	
	private void SnapTo()
	{
		if (snapToBounds)
		{
			Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        	float gridx = incrementAmount.x;
        	float gridy = incrementAmount.y;
        	float gridz = incrementAmount.z;
        
        	foreach (Transform transform in transforms)
        	{
				// Make sure our objects move to increment.
            	Vector3 newPosition = transform.position;
            	newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            	newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            	newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            	transform.position = newPosition;
				
				// Make sure our objects scale to increment.
				Vector3 newScale = transform.localScale;
				
				if (transform.localScale.x > gridx)
					newScale.x = Mathf.Round(newScale.x / gridx) * gridx;
				else
					newScale.x = gridx;
				
				if (transform.localScale.y > gridy)
					newScale.y = Mathf.Round(newScale.y / gridy) * gridy;
				else
					newScale.y = gridy;
				
				if (transform.localScale.z > gridz)
					newScale.z = Mathf.Round(newScale.z / gridz) * gridz;
				else
					newScale.z = gridz;
				
				transform.localScale = newScale;			
        	}
		}	
	}	
	
	// Find the grid we will use to help align objects.
	private GameObject GetGrid()
	{
		// Find the object with tag grid.
		GameObject grid = GameObject.FindWithTag("Grid");
		
		if (grid != null)
			return grid;
		
		return (null);
	}
	
	#endregion
}
