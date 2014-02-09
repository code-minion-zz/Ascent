using UnityEngine;
using System.Collections;

public class MenuButtonFunctions : MonoBehaviour 
{
	
	public string levelName;
	public GameObject defaultSelection;
	
	void OnEnable()
	{
		UICamera.fallThrough = this.gameObject;
	}
	
	void Awake()
	{
	}
	
	public void GoToLevel()
	{
        if (enabled)
        {
            Application.LoadLevel(levelName);
        }
	}	
	
	void Update()
	{
		InputDevice device = InputManager.GetDevice(0);
		
		Debug.Log (device);
		//if (device.IsConnected())
		{
			Debug.Log ("Connected");
			if(device.Start.IsPressed)
			{
				GoToLevel();
			}
		}
//		if (UICamera.selectedObject == null)
//		{
//			UICamera.selectedObject = this.gameObject;
//		}
	}
	
	void OnKey (KeyCode key)
	{
//		if (UICamera.selectedObject == this.gameObject)
//		{
//			UICamera.selectedObject = defaultSelection;
//			UICamera.Notify(defaultSelection,"OnHover",false);
//		}
	}
}
