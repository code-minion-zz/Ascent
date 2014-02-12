using UnityEngine;
using System.Collections;

public class StatusEffectIcon : MonoBehaviour 
{
    protected bool initialised;
    protected StatusEffect statusEffect;

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

    public void Initialise(StatusEffect effect, string iconName)
    {
        statusEffect = effect;
        iconSprite = GetComponent<UISprite>();

#if UNITY_EDITOR
        if (iconSprite == null)
        {
            Debug.LogError("No UISprite is attached to the StatusEffectIcon. Check the gameObject.");
            return;
        }
#endif

        IconName = iconName;
        initialised = true;
    }

    public void Update()
    {
        if(initialised)
        {
            // TODO: Fade in and out if the duration is expiring.
        }
    }
}
