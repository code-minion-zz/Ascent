#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridDesigner : EditorWindow 
{
	#region enums
	
	public enum C_AspectRatio
	{
		FourThree,
		FiveFour,
		ThreeTwo,
		SixteenTen,
		SixteenNine
	}	
	
	#endregion
	
	#region Fields
	
	private GameObject grid;
	private int aspectRatioNumerator;
	private int aspectRatioDenominator;
	
	private int gridScale = 2;
	private float gridX, gridY;
	private Vector2 gridDimensions;
	
	private C_AspectRatio currentAspectRatio;
	
	const string redMatPath = "Assets/Source/LevelEditor/Materials/gridMat1.mat";
	const string whiteMatPath = "Assets/Source/LevelEditor/Materials/gridMat2.mat";
	
	#endregion
	
	// Initialization of the level designer.
	[MenuItem ("Ascent/Tools/Generate Grid")]
	private static void Init()
	{
		// Create and position the window
        EditorWindow.GetWindow(typeof(GridDesigner), false);
        //designerWindow.position = new Rect(, 150, 300, 350);		
	}
	
	void OnInspectorUpdate()
	{
		Repaint();
		
		gridDimensions = new Vector2(gridX, gridY);
	}
	
	private void OnGUI()
	{
		CurrentAspectRatio();
		
		// Seperate
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Aspect Ratio:", GUILayout.Width(125));
		currentAspectRatio = (C_AspectRatio)EditorGUILayout.EnumPopup(currentAspectRatio);
		EditorGUILayout.EndHorizontal();
		
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
		
		if (GUILayout.Button("Generate Grid"))
			GenerateGrid();
	}
	
	private float CurrentAspectRatio()
	{
		switch (currentAspectRatio)
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
	
	// Generates a grid game object and child tiles.
	private void GenerateGrid()
	{
		// Create the grid GameObject
		grid = new GameObject("GridHelper");
		grid.transform.position = Vector3.zero;
		
		// Assign the tag.
		grid.tag = "Grid";
		
		// Create the child grid tiles
		GameObject[] gameObjects = new GameObject[121];
		
		Vector2 frame = new Vector2(aspectRatioNumerator * gridScale, aspectRatioDenominator * gridScale);
		float gridPosx = 0;
		float gridPosy = 0.01f;
		float gridPosz = 0;		
		
		for (int i = 0; i < gameObjects.Length; i++)	
		{
			// Create the plane
			gameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
			gameObjects[i].name = "Grid " + i.ToString();
			gameObjects[i].transform.localScale = new Vector3((frame.x * 2f) / 10f, 1f, (frame.y * 2f) / 10f);
			
			if (i <= 10)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * i * 2f;
				gridPosz = -5f * frame.y * 2f;
			}
			else if (i <= 21)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 11) * 2f;
				gridPosz = -4f * frame.y * 2f;
			}
			else if (i <= 32)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 22) * 2f;
				gridPosz = -3f * frame.y * 2f;
			}
			else if (i <= 43)
			{
				gridPosx = -5f * frame.x * 2f + frame.x * (i - 33) * 2f;
				gridPosz = -2f * frame.y * 2f;
			}
			else if (i <= 54)
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
			
			Material redMat = AssetDatabase.LoadAssetAtPath(redMatPath, typeof(Material)) as Material;
			Material whiteMat = AssetDatabase.LoadAssetAtPath(whiteMatPath, typeof(Material)) as Material;
			
			//redMat.mainTextureScale = new Vector2(frame.x * 2, frame.y * 2);
			redMat.mainTextureOffset = new Vector2(0.5f, 0.5f);
			//whiteMat.mainTextureScale = new Vector2(frame.x * 2, frame.y * 2);
			whiteMat.mainTextureOffset = new Vector2(0.5f, 0.5f);		
			
			// Every second tileset change material
			if (i % 2 == 0)
				gameObjects[i].renderer.material = redMat;
			else
				gameObjects[i].renderer.material = whiteMat;			
			
			// Move this plane to the right location.
			gameObjects[i].transform.position = new Vector3(gridPosx, gridPosy, gridPosz);
			// Set the parent of this object to the grid helper.
			gameObjects[i].transform.parent = grid.transform;
		}
	}
}

#endif