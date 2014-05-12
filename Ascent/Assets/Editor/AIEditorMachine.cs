using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLabels,
    NoSize = ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}

[CustomEditor(typeof(AIMindAgent))]
[CanEditMultipleObjects]
public class AIEditorMachine : Editor
{
    private SerializedObject aiMindAgent;
    private SerializedProperty newBehaviours;

    private static GUIContent
            moveButtonContent = new GUIContent("\u21b4", "move down"),
            duplicateButtonContent = new GUIContent("+", "duplicate"),
            deleteButtonContent = new GUIContent("-", "delete"),
            addButtonContent = new GUIContent("+", "add element");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    void OnEnable()
    {
        aiMindAgent = new SerializedObject(target);
    }

    /*public override void OnInspectorGUI()
    {
        aiMindAgent.Update();
        newBehaviours = aiMindAgent.FindProperty("newBehaviours");

        EditorGUILayout.PropertyField(aiMindAgent.FindProperty("overrideScriptSetups"));
        EditorGUILayout.PropertyField(aiMindAgent.FindProperty("curBehaviour"));

        SerializedProperty aiBehaviours = newBehaviours.FindPropertyRelative("valuesList");

        Show(aiBehaviours, EditorListOption.NoSize);

        aiMindAgent.ApplyModifiedProperties();
    }*/

    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        bool showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0;
        bool showButtons = true;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list, new GUIContent("AIBehaviours"));
            EditorGUI.indentLevel += 1;
        }

        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");

            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }

            for (int i = 0; i < list.arraySize; ++i)
            {
                SerializedProperty triggersList = list.GetArrayElementAtIndex(i).FindPropertyRelative("triggers");
                SerializedProperty type = list.GetArrayElementAtIndex(i).FindPropertyRelative("type");

                AIMindAgent.EBehaviour typeName = (AIMindAgent.EBehaviour)type.enumValueIndex;

                EditorGUILayout.PropertyField(triggersList, new GUIContent(typeName.ToString()));

                // Show child elements of each trigger list.
                ShowElements(triggersList, EditorListOption.ElementLabels | EditorListOption.Buttons);

                // Show the butotns but only if the list is not expanded
                if (showButtons && !triggersList.isExpanded)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Separator();
                    ShowButtons(list, i);
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
            }
        }

        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    public static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        bool showElementLables = (options & EditorListOption.ElementLabels) != 0,
            showButtons = (options & EditorListOption.Buttons) != 0;

        if (showElementLables)
        {
            EditorGUI.indentLevel += 1;
        }

        if (!showElementLables || list.isExpanded)
        {
            EditorGUI.indentLevel += 1;

            for (int i = 0; i < list.arraySize; ++i)
            {
                SerializedProperty prop = list.GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(prop, new GUIContent("Trigger"), true);

                if (showButtons)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Separator();
                    ShowButtons(list, i);
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
            }
        }

        if (showElementLables)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    //}

    //void AddNewBehaviour()
    //{
    //    AIBehaviour behaviour = null;

    //    Character character = agent.gameObject.GetComponent<Character>();

    //    // Aggressive
    //    int newKey = agent.newBehaviours.KeysList.Count + 1;
    //    Debug.Log(newKey);
    //    behaviour = agent.AddBehaviour(newKey, new AIBehaviour(AIMindAgent.EBehaviour.Defensive));
    //}

    void ProgressBar (float value, string label) 
    {
		// Get a rect for the progress bar using the same margins as a textfield:
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar(rect, value, label);
		EditorGUILayout.Space();

	}
}
