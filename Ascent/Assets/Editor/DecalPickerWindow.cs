using UnityEngine;
using System.Collections;
using UnityEditor;

public class DecalPickerWindow : EditorWindow
{
    private bool isSetup = false;
    private Vector2 scrollPosition = Vector2.zero;
    private const string decalFolder = "Prefabs/RoomPieces/Surface Decals";
    private UnityEngine.Object[] decalObjects;

    public delegate void DecalSelectedHandler(UnityEngine.Object decalObject);
    private DecalSelectedHandler handler;

    void Initialise(DecalSelectedHandler handler)
    {
        isSetup = true;
        decalObjects = Resources.LoadAll(decalFolder);
        this.handler = handler;
    }

    void Update()
    {
    }

    void OnGUI()
    {
        if (isSetup == false)
        {
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        int counter = 0;
        foreach (UnityEngine.Object obj in decalObjects)
        {
            if (counter % 4 == 0 || counter == 0)
            {
                EditorGUILayout.BeginHorizontal();
            }

            ++counter;

            Texture2D texture = AssetPreview.GetAssetPreview(obj);

            if (GUILayout.Button(texture, GUILayout.Height(texture.width), GUILayout.Width(texture.height)))
            {
                handler(obj);
                this.Close();
            }

            if (counter % 4 == 0)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
