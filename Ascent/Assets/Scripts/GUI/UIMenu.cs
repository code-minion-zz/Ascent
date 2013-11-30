using UnityEngine;

using System;

/// <summary>
/// A Visible Menu (or Scene State)
/// </summary>
public class UIMenu : MonoBehaviour
{
	UIPlayerPanel				activePanel;
	BetterList<UIPlayerPanel> 	playerPanels;
	
	public delegate void PanelTransitionEvent(UIPlayerPanel destination);

	/// <summary>
	/// Perform all processes to deactivate a Menu
	/// </summary>
	public void Deactivate()
	{
		this.gameObject.SetActive(false);
	}

	public void Activate()
	{
		this.gameObject.SetActive(true);
	}

	protected void PlayAnimation(bool reverse = false)
	{
	}
}

