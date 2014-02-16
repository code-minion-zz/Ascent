using UnityEngine;
using System.Collections;

public class StatusEffectHUDIcon : PlayerHUDIcon 
{
    protected StatusEffect statusEffect;


    public void Initialise(StatusEffect effect)
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


        if (effect.Timed)
        {
            float alphaRatio = 1.0f- (effect.TimeElapsed / effect.FullDuration);

            Color color = iconSprite.color;
            color.a = alphaRatio;
            iconSprite.color = color;
        }

        //IconName = effect.Name;
        IconName = "Skills";
        initialised = true;
    }
}
