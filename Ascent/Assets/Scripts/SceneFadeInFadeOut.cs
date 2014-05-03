using UnityEngine;
using System.Collections;

public class SceneFadeInFadeOut : MonoBehaviour 
{
	public UIWidget widget;

	public float transitionTime = 2.0f;
	public Color startColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	public Color endColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	private float timeElapsed;

	private TweenColor tweener;

	private bool reverseTransition;

	public delegate void TransitionEnd();
	public event TransitionEnd onTransitionEnd;
	public event TransitionEnd onReverseTransitionEnd;

	public void Start()
	{
		widget.color = startColor;
	}

	[ContextMenu("Transition")]
	public void Transition()
	{
		widget.color = startColor;
		tweener = TweenColor.Begin(this.gameObject, transitionTime, endColor);
		reverseTransition = false;
	}
 
	[ContextMenu("ReverseTransition")]
	public void ReverseTransition()
	{
		widget.color = endColor;
		tweener = TweenColor.Begin(this.gameObject, transitionTime, startColor);
		reverseTransition = true;
	}

	public void OnTransitionEnd()
	{
		if (reverseTransition)
		{
			if (onReverseTransitionEnd != null)
			{
				onReverseTransitionEnd.Invoke();
			}
		}
		else
		{
			if (onTransitionEnd != null)
			{
				onTransitionEnd.Invoke();
			}
		}
	}
}
