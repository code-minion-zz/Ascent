using UnityEngine;
using System.Collections;

public class eTrigger : MonoBehaviour {

	[System.NonSerialized]
	public Collider exitCol;
	
	private MainTrigger mTrigger;
	
	void Start (){ mTrigger = 	GameObject.Find("_MainTrigger").GetComponent<MainTrigger>(); }
		
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Player"))
		{
			mTrigger.TranslateCameraRight();
			mTrigger.lastMovedDirection = MainTrigger.MoveCameraDirection.East;
			
			exitCol = col;
		}
	}
}