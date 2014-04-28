using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
	UILabel loadingText;

	private string levelToLoad;

	// Use this for initialization
	void Start () 
	{
		Transform xform = transform.FindChild("UI Root (2D)");
		xform = xform.FindChild("Camera");
		xform = xform.FindChild("Anchor");
		xform = xform.FindChild("Panel");
		xform = xform.FindChild("Background");
		xform = xform.FindChild("Title");

		loadingText = xform.GetComponent<UILabel>();

		levelToLoad = Game.Singleton.LevelName;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float progress = (Application.GetStreamProgressForLevel(levelToLoad));
		loadingText.text = "Loading: " + (progress * 100.0f) + "%";
		//Debug.Log(loadingText.text);
		if(progress >= 1.0f)
		{
			//Debug.Log("Load complete: " + levelToLoad);

			Application.LoadLevel(levelToLoad);
		}
	}
}
