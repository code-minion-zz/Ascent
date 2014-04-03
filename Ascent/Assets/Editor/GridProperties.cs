#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using Ascent;

// Disables warning: The levelGrid is not used (but it is!?)
#pragma warning disable 0414

[Serializable]
public class GridProperties : EditorWindow
{
	#region Fields

	private int buttonWidth = 125;
	private bool snapToBounds = true;
	
	// Creation settings
    [SerializeField]
    private Vector2 scrollPosition;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Vector3 snapAmount = new Vector3(0.5f, 0.5f, 0.5f);
	
	#endregion
	
	void Awake()
	{
        // Anything that happens here will only happen once. 
	}

    public void Init(Grid grid)
    {
        this.grid = grid;
    }

    void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
    }

    void OnDisable()
    {

    }

    // Update is called once per frame
    void Update () 
    {
        if (grid == null)
            return;

        if (EditorApplication.isPlaying)
            return;
		
        if (snapToBounds)
            SnapTo();	
    }

    void OnGUI()
    {
        if (grid == null)
            return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(" Spacing width ");
                    grid.tileWidth = EditorGUILayout.FloatField(grid.tileWidth, GUILayout.Width(50));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" Spacing length ");
                    grid.tileLength = EditorGUILayout.FloatField(grid.tileLength, GUILayout.Width(50));
                    GUILayout.EndHorizontal();

                    grid.tileWidth = Mathf.Clamp(grid.tileWidth, 0.1f, Mathf.Infinity);
                    grid.tileLength = Mathf.Clamp(grid.tileLength, 0.1f, Mathf.Infinity);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" Grid Width ");
					grid.gridLength = EditorGUILayout.FloatField(grid.gridLength, GUILayout.Width(50));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" Grid Length ");
					grid.gridWidth = EditorGUILayout.FloatField(grid.gridWidth, GUILayout.Width(50));

                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200));
                }
                GUILayout.EndHorizontal();

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

            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
    }
	
	#region Designer Functions

	private void ShowHideGrid()
	{
        if (grid.showGrid == true)
        {
            grid.showGrid = false;
        }
        else
        {
            grid.showGrid = true;
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
                //Vector3 newScale = transform.localScale;
                //newScale.x = Mathf.Clamp(Mathf.Round(newScale.x / gridx) * gridx, gridx, Mathf.Infinity);
                //newScale.y = Mathf.Clamp(Mathf.Round(newScale.y / gridy) * gridy, gridy, Mathf.Infinity);
                //newScale.z = Mathf.Clamp(Mathf.Round(newScale.z / gridz) * gridz, gridz, Mathf.Infinity);
                //transform.localScale = newScale;			
        	}
		}	
	}	
	
	#endregion
}
#endif