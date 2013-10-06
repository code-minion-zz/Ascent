using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[Serializable]
public class LevelDesigner : EditorWindow
{
	#region Fields
	
	int buttonWidth = 125;
	bool snapToBounds = true;
	GameObject grid;
	
	// Creation settings
    Vector2 scrollPosition;
    ObjectCreationGUI objCreationGUI;

    [SerializeField]
    Vector3 snapAmount = new Vector3(0.5f, 0.5f, 0.5f);
	
	#endregion

    [MenuItem("Ascent/Level Designer... %g")]
    private static void showEditor()
    {
        EditorWindow.GetWindow<LevelDesigner>(false, "Level Designer");
    }

    [MenuItem("Ascent/Level Designer... %g", true)]
    private static bool showEditorValidtator()
    {
        return true;
    }
	
	void Awake()
	{
        // Anything that happens here will only happen once. 
	}

    void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
        if (objCreationGUI == null)
            objCreationGUI = new ObjectCreationGUI();

        objCreationGUI.OnEnable();

        grid = GetGrid();
    }

    void OnDisable()
    {
        objCreationGUI.OnDisable();
    }

    // Update is called once per frame
    void Update () 
    {
        if (EditorApplication.isPlaying)
            return;
		
        if (snapToBounds)
            SnapTo();	
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Separator();

                if (GUILayout.Button("Show/Hide Grid", GUILayout.Width(buttonWidth)))
                {
                    ShowHideGrid();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                ToggleBounds(EditorGUILayout.Toggle(snapToBounds, GUILayout.Width(10)));
                EditorGUILayout.PrefixLabel("Snap Movement");
                EditorGUILayout.EndHorizontal();

                // Set increment amount
                snapAmount = EditorGUILayout.Vector3Field("Movement Increment Amount:", snapAmount);

                objCreationGUI.OnGUI();

            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
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
	
	private void ToggleCenter(bool state)
	{
		if (state)
		{
			ToggleBounds(false);	
		}
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

            float gridx = snapAmount.x;
            float gridy = snapAmount.y;
            float gridz = snapAmount.z;
        
        	foreach (Transform transform in transforms)
        	{
				// Make sure our objects move to increment.
            	Vector3 newPosition = transform.position;
            	newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            	newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            	newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            	transform.position = newPosition;
				
				// Make sure our objects scale to increment.
                // We will clamp to infinity as we do not know how big the maximum scale of an object will be.
				Vector3 newScale = transform.localScale;
				newScale.x = Mathf.Clamp(Mathf.Round(newScale.x / gridx) * gridx, gridx, Mathf.Infinity);
                newScale.y = Mathf.Clamp(Mathf.Round(newScale.y / gridy) * gridy, gridy, Mathf.Infinity);
                newScale.z = Mathf.Clamp(Mathf.Round(newScale.z / gridz) * gridz, gridz, Mathf.Infinity);
				transform.localScale = newScale;			
        	}
		}	
	}	
	
	// Find the grid we will use to help align objects.
	private GameObject GetGrid()
	{
		// Find the object with tag grid.
		GameObject grid = GameObject.FindWithTag("GridHelper");
		
		if (grid != null)
			return grid;
		
		return (null);
	}
	
	#endregion
}