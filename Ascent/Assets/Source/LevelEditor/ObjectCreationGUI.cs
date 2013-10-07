using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ObjectCreationGUI
{
    [SerializeField]
    GameObject objectToCreate;
    [SerializeField]
    bool showFoldOut;
    [SerializeField]
    string objectName;

    const string createObjects = "Create Objects";
    int buttonWidth = 125;

    public ObjectCreationGUI()
    {
        showFoldOut = false;
        objectToCreate = null;
        objectName = "";
    }

    public void OnEnable()
    {
        // Load up editor prefs
        showFoldOut = EditorPrefs.GetBool("showFoldOut");
    }

    public void OnGUI()
    {
        // Begin foldout for creating objects
        showFoldOut = EditorGUILayout.Foldout(showFoldOut, 
            createObjects);

        if (showFoldOut)
        {
            EditorGUILayout.BeginVertical();

            // Assign selected game object prefab
            objectToCreate = EditorGUILayout.ObjectField("Game Object",
                objectToCreate, typeof(GameObject), true) as GameObject;

            if (objectToCreate != null)
            {
                if (objectName == "")
                    objectName = objectToCreate.name;

                // Give the object a name
                objectName = EditorGUILayout.TextField("Name", objectName);

                if (GUILayout.Button("Create", GUILayout.Width(buttonWidth)))
                    CreateObject();
            }

            EditorGUILayout.EndVertical();
        }
    }

    public void OnDisable()
    {
        // Save editor prefs
        EditorPrefs.SetBool("showFoldOut", showFoldOut);
    }

    void CreateObject()
    {
        // Instantiate the selected game object.
        GameObject newObj = GameObject.Instantiate(objectToCreate) as GameObject;
        newObj.name = objectName;

        GameObject[] objects = new GameObject[] { newObj };
        Selection.objects = objects;

        // Tell the scene view to focus on the selected objects.
        SceneView sceneView = SceneView.lastActiveSceneView;
        sceneView.Focus();
        sceneView.FrameSelected();
    }
}
