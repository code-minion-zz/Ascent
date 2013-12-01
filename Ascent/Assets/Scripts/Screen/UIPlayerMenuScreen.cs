using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerMenuScreen : MonoBehaviour 
{
	protected List<UIPlayerMenuWindow> windows;

	public virtual void Start()
	{
		windows = new List<UIPlayerMenuWindow>();

		UIPlayerMenuWindow[] foundWindows = GetComponentsInChildren<UIPlayerMenuWindow>();
		foreach (UIHeroSelect_Window win in foundWindows)
		{
			windows.Add(win);
			win.gameObject.SetActive(false);
		}
	}
}
