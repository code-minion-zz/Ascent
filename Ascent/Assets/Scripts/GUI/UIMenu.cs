using UnityEngine;

using System;

public class UIMenu : MonoBehaviour
{
	UIPlayerPanel				activePanel;
	BetterList<UIPlayerPanel> 	playerPanels;
	
	public delegate void PanelTransitionEvent(UIPlayerPanel destination);
}

