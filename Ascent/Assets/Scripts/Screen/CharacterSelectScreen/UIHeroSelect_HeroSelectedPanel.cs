using UnityEngine;
using System.Collections;

public class UIHeroSelect_HeroSelectedPanel : UIPlayerMenuPanel 
{
    //public override void Initialise()
    //{
    //}

    public override void OnMenuOK(InputDevice device)
    {
        // Ready up
    }


    public override void OnMenuCancel(InputDevice device)
    {
        parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
    }
}
