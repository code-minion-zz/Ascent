using UnityEngine;
using System.Collections;

public class MenuButtonFunctions : MonoBehaviour 
{
	
	public string levelName;
	public GameObject defaultSelection;
	InputDevice[] devices;
	
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
			Game.Singleton.LoadLevel(levelName, Game.EGameState.HeroSelect);
        }
	}	
	
	void Update()
	{
		foreach (InputDevice id in InputManager.Devices)
		{
			if(id.Start.IsPressed)
			{
				GoToLevel();
				return;
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
