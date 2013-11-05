using UnityEngine;
using System.Collections;

[AddComponentMenu("UI/Functionality/Load Level")]
public class MenuButtonFunctions : MonoBehaviour {
	
	public string levelName = "Floor";
	
	void OnEnable()
	{
	}
	
	public void GoToLevel()
	{
		if (enabled) Application.LoadLevel(levelName);
	}
}
