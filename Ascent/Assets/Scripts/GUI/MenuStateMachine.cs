using UnityEngine;

using System;

public class MenuStateMachine : MonoBehaviour
{
	public UIMenu				activeMenu;
	public BetterList<UIMenu> 	menus;

	public delegate void MenuTransitionEvent(UIMenu destination);

	void OnDestroy()
	{
		//menus.;
	}
}

