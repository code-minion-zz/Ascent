using UnityEngine;
using System.Collections;

public class nTrigger : MonoBehaviour {

	[System.NonSerialized]
	public Collider exitCol;
	
	private MainTrigger mTrigger;
	
	void Start (){ mTrigger = 	GameObject.Find("_MainTrigger").GetComponent<MainTrigger>(); }
		
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Player"))
		{
			mTrigger.TranslateCameraUp();
			mTrigger.lastMovedDirection = MainTrigger.MoveCameraDirection.North;
			
			exitCol = col;
		}
	}
}