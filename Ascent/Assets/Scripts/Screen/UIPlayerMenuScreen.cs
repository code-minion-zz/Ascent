using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerMenuScreen : MonoBehaviour 
{
	protected List<UIPlayerMenuWindow> windows;
    protected bool initialised = false;

	public virtual void Start()
	{
		windows = new List<UIPlayerMenuWindow>();

		UIPlayerMenuWindow[] foundWindows = GetComponentsInChildren<UIPlayerMenuWindow>();
		foreach (UIPlayerMenuWindow win in foundWindows)
		{
			windows.Add(win);
            win.Initialise();
			win.gameObject.SetActive(false);
		}
	}
}
