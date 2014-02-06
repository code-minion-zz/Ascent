using UnityEngine;
using System.Collections.Generic;


public class TextDriver : MonoBehaviour
{
    public GameObject floatingText;

    public void SpawnDamageText(GameObject target, int damage, Color color)
    {
        // Parent it to this game object, it also instantiates the floatingText prefab 
        // with the right size requirements for us.
        FloatingText ft = NGUITools.AddChild(gameObject, floatingText).GetComponent<FloatingText>();

        if (ft != null)
        {
            ft.SpawnAt(target);
            ft.follow = true;
            ft.UILabel.text = "" + damage;
            ft.UILabel.color = color;
            ft.Following();

            TweenPosition tp = ft.TweenPosition;
            tp.duration = 1.5f;
            tp.from = ft.transform.localPosition;
            tp.to = tp.from + Vector3.up * 100.0f;

            // Destroy the text after the tween duration.
            ft.DestroyText(tp.duration);
        }
        else
        {
            Debug.Log("Floating text component null");
        }
    }
}
