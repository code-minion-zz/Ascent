using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(AISteeringAgent))]
public class AISteeringAgentPropertyDrawer : PropertyDrawer
{
	public override void  OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
 		base.OnGUI(position, property, label);
	}
}

//class SimpleMaskUsage extends EditorWindow 
//{
//        @MenuItem("Examples/Mask Field Usage")
//        static function Init() {
//            var window = GetWindow(SimpleMaskUsage);
//            window.Show();
//        }
		
//        var flags : int = 0;
//        var options : String[] = ["CanJump", "CanShoot", "CanSwim"];
//        function OnGUI() {
//            flags = EditorGUILayout.MaskField ("Player Flags", flags, options);
//        }
//    }