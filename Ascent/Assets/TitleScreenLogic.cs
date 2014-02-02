using UnityEngine;
using System.Collections;

public class TitleScreenLogic : MonoBehaviour {

	UICamera thisCam;
	public GameObject defaultButton;

	// Use this for initialization
	void Start () {
		//thisCam = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("3D UI")).GetComponent<UICamera>();
		//UICamera.currentTouch = new UICamera.MouseOrTouch();
		//UICamera.currentTouch.current = defaultButton;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
