using UnityEngine;
using System.Collections.Generic;


public class TextDriver : MonoBehaviour
{
    public GameObject floatingText;

    public void SpawnDamageText(GameObject target, int damage)
    {
        // Parent it to this game object, it also instantiates the floatingText prefab 
        // with the right size requirements for us.
        FloatingText ft = NGUITools.AddChild(gameObject, floatingText).GetComponent<FloatingText>();

        //FloatingText ft = NGUITools.AddChild<FloatingText>(gameObject);

        if (ft != null)
        {
            ft.SpawnAt(target);
            ft.follow = true;
            ft.UILabel.text = "" + damage;
            ft.UILabel.color = Color.red;

            ft.Following();

            // TODO: Figure out why this tweening is not moving it and not calling the
            // DestroyText function at all.
            TweenPosition tp = ft.TweenPosition;
            tp.duration = 1.5f;
            tp.from = ft.transform.localPosition;
            tp.to = tp.from + Vector3.up * 10.0f;
            tp.callWhenFinished = "DestroyText";
        }
        else
        {
            Debug.Log("Floating text component null");
        }
    }

    public void DestroyText()
    {
        Debug.Log("Destroy me");
    }

    public void DeleteText()
    {
        Debug.Log("Text deleted");
    }
}
