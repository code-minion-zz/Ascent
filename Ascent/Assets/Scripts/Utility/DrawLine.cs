using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour 
{
	public GameObject go1;
	public GameObject go2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (go1 != null && go2 != null)
		{
			Debug.DrawLine(go1.transform.position, go2.transform.position);
		}
	}
}
