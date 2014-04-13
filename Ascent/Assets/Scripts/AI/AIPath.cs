using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPath : MonoBehaviour 
{
	public List<Vector3> nodes = new List<Vector3>();

	public float nodeRadius;
	public List<Vector3> Nodes { get { return nodes; } }

	public void AddNode(Vector3 node)
	{
		nodes.Add(node);
	}
}
