using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CameraData : ScriptableObject
{
	public Vector2 roomSize;
	public float cameraHeight;
	public bool perspectiveProjection;
}