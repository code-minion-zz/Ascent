using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class Snap : ScriptableObject
{
    [MenuItem ("GameObject/Snap Object %g")]
    static void MenuSnapToGridBounds()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        float gridx = 0.5f;
        float gridy = 0.5f;
        float gridz = 0.5f;
        
        foreach (Transform transform in transforms)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Round(newPosition.x / gridx) * gridx;
            newPosition.y = Mathf.Round(newPosition.y / gridy) * gridy;
            newPosition.z = Mathf.Round(newPosition.z / gridz) * gridz;
            transform.position = newPosition;
        }
    }
}
