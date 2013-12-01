using UnityEngine;
using System.Collections;

public class UIHeroSelect_LoadHeroPanel : UIPlayerMenuPanel
{
	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
	}
}
