using UnityEngine;
using System.Collections;

public class SwitchModes : MonoBehaviour {

	private MainTrigger mainTrigger;
	
	void Start()
	{
		mainTrigger = GameObject.Find("_MainTrigger").GetComponent<MainTrigger>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			mainTrigger.SwitchMode();
		}
	}
}
