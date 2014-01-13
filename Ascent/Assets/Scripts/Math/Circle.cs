using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Circle : Shape2D 
{
    public float radius;

    public Transform transform;

	public Circle()
	{
		type = Shape2D.EType.Circle;
	}

    public void DebugDraw()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, radius);
    }
}
