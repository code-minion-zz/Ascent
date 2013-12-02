using UnityEngine;
using System.Collections;

public class UIHeroSelect_LoadHeroPanel : UIPlayerMenuPanel
{
    public override void OnEnable()
    {
        // Load all games and list them

        base.OnEnable();
    }

    public void Update()
    {

    }

	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
	}
}
