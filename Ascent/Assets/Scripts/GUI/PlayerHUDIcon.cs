using UnityEngine;
using System.Collections;

public class PlayerHUDIcon : MonoBehaviour
{
    protected bool initialised;

    protected UISprite iconSprite;
    public UISprite IconSprite
    {
        get { return iconSprite; }
        set { iconSprite = value; }
    }

    public string IconName
    {
        get { return iconSprite.spriteName; }
        set { iconSprite.spriteName = value; }
    }

    public void Update()
    {
        if (initialised)
        {
            // TODO: Fade in and out if the duration is expiring.
        }
    }
}
