using UnityEngine;
using System.Collections;

public class MainMenuPlay : MonoBehaviour {
	
	public string levelName;
	
	public void OnClick()
	{
		Application.LoadLevel("Floor");
	}
}
