using UnityEngine;
using System.Collections;

public class sTrigger : MonoBehaviour {

	[System.NonSerialized]
	public Collider exitCol;
	
	private MainTrigger mTrigger;
	
	void Start (){ mTrigger = 	GameObject.Find("_MainTrigger").GetComponent<MainTrigger>(); }
		
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Player"))
		{
			mTrigger.TranslateCameraDown();
			mTrigger.lastMovedDirection = MainTrigger.MoveCameraDirection.South;
			
			exitCol = col;
		}
	}
}