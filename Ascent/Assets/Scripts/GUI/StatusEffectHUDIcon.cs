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
            float alphaRatio = 1.0f - (effect.TimeElapsed / effect.FullDuration);

            Color color = iconSprite.color;

			if (statusEffect is Blessing)
			{
				color = new Color(0.3f, 1.0f, 0.3f, alphaRatio);
			}
			else
			{
				if (statusEffect.Type == StatusEffect.EEffectType.Debuff)
				{
					color = new Color(1.0f, 0.4f, 0.4f, alphaRatio);
				}
				else
				{
					color = new Color(0.4f, 1.0f, 0.4f, alphaRatio);
				}
			}

            iconSprite.color = color;
        }

		IconName = "StatusEffect_" + statusEffect.GetType().ToString();

		//Debug.Log("StatusEffect_" + statusEffect.GetType().ToString());
		
		initialised = true;
    }
}
