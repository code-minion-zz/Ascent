using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelDesigner : EditorWindow 
{
	#region Fields
	
	private int buttonWidth = 125;
	
	private GameObject grid;
	
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
		
		// End vertical layout
		EditorGUILayout.EndVertical();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (EditorApplication.isPlaying)
			return;	
	}
	
	#region Designer Functions

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
