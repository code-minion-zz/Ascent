using UnityEngine;
using System.Collections;

public class nCross : MonoBehaviour {

	[System.NonSerialized]
	public MainTrigger mTrigger;
	
	void Start() {mTrigger = GameObject.Find("_MainTrigger").GetComponent<MainTrigger>();}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "HorizontalWall")
		{
			mTrigger.northLock = true;
		}
	}
}
