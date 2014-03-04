using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerMenuScreen : MonoBehaviour 
{
	protected List<UIPlayerMenuWindow> windows;
    protected bool initialised = false;

	public UIPanel mainPanel;
	public UIPlayerMenuWindow prefabWindow;

	public virtual void Start()
	{
		// Check if P1 window was given
		if (prefabWindow == null)
		{
			Debug.LogError("Prefab Window is null. Drag P1 into it.");
		}

		// Grab and store references of P1
		windows = new List<UIPlayerMenuWindow>();
		windows.Add(prefabWindow);

		GameObject grid = transform.FindChild("Grid").gameObject;

		// Instantiate P2 and P3
		for (int i = 0; i < 2; ++i) // 2 for player two and three
		{
			GameObject windowGO =  NGUITools.AddChild(grid, prefabWindow.gameObject);
			windowGO.name = "P" + (i + 2);
			//NGUITools.SetActive(windowGO,false);

			UIPlayerMenuWindow window = windowGO.GetComponent<UIPlayerMenuWindow>();
			windows.Add(window);
		}

		// Init and deactivate the windows
		// Position Players Windows
		int maxPlayers = 3;
		int offsetX = -1;
		for (int i = 0; i < maxPlayers; ++i) // 3 players
		{
			windows[i].transform.position = new Vector3(offsetX + (offsetX * -i), prefabWindow.transform.position.y, prefabWindow.transform.position.z);
			//windows[i].Initialise();
			//windows[i].gameObject.SetActive(false);
		}
	}

//	[ContextMenu("Execute")]
//	public void OnMenuHax()
//	{
//		Game.Singleton.LoadLevel("Sewer_Levels", Game.EGameState.Tower);
//	}
}
