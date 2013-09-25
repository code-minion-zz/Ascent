using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GridEditor : EditorWindow {
	
	private Material material1;
	private Material material2;
	
	public Color matColor1 = Color.red;
	public Color matColor2 = Color.blue;
	
	public float matColorAlpha1 = 1f;
	public float matColorAlpha2 = 1f;
	
	[MenuItem ("GameObject/UniVenture/Grid Editor")]
	private static void Init()
    {
        GridEditor uniVentureGridEditorWindow = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
        uniVentureGridEditorWindow.position = new Rect(100, 150, 250, 135);
    }
	
	void ChangeColor()
	{
		material1 = AssetDatabase.LoadMainAssetAtPath("Assets/UniVenture/Materials/gridMat1.mat") as Material;
		material2 = AssetDatabase.LoadMainAssetAtPath("Assets/UniVenture/Materials/gridMat2.mat") as Material;
		matColor1.a = matColorAlpha1;
		material1.SetColor("_TintColor", matColor1);
		matColor2.a = matColorAlpha2;
		material2.SetColor("_TintColor", matColor2);
	}
	
	void RevertColor()
	{
		material1 = AssetDatabase.LoadMainAssetAtPath("Assets/UniVenture/Materials/gridMat1.mat") as Material;
		material2 = AssetDatabase.LoadMainAssetAtPath("Assets/UniVenture/Materials/gridMat2.mat") as Material;
		matColor1 = Color.red;
		matColor1.a = matColorAlpha1;
		material1.SetColor("_TintColor", matColor1);
		
		matColor2 = Color.white;
		matColor2.a = matColorAlpha2;
		material2.SetColor("_TintColor", matColor2);
	}
	
	private void OnGUI()
	{
		GUILayout.BeginHorizontal();
		matColor1 = EditorGUILayout.ColorField(" Grid Color 1: ", matColor1);
		matColorAlpha1 = EditorGUI.Slider(new Rect(10,20,position.width, 20), " Alpha: ", matColorAlpha1, 0f, 1f);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		GUILayout.BeginHorizontal();
		matColor2 = EditorGUILayout.ColorField(" Grid Color 2: ", matColor2);
		matColorAlpha2 = EditorGUI.Slider(new Rect(10,65, position.width, 20), " Alpha: ", matColorAlpha2, 0f, 1f);
		GUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(" Apply Changes "))
		{
			ChangeColor();	
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(" Default "))
		{
			RevertColor();	
		}
		GUILayout.EndHorizontal();
	}
}
