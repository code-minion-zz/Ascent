using UnityEngine;
using System.Collections;

public class UIHeroSelect_HeroSelectedPanel : UIPlayerMenuPanel 
{
    public override void OnMenuOK(InputDevice device)
    {
        if (!parent.Ready)
        {
            parent.ReadyWindow(true);
        }
    }

    public override void OnMenuCancel(InputDevice device)
    {
        if (parent.Ready)
        {

            parent.ReadyWindow(false);
        }
        else
        {
            Object.Destroy(parent.Player.Hero);
            parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
        }
    }
}
