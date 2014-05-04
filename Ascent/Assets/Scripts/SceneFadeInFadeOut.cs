using UnityEngine;
using System.Collections;

public class SceneFadeInFadeOut : MonoBehaviour 
{
	private enum EState 
	{
		Wait,
		Fading,
		Done,
	}

	public UIWidget widget;

	public float transitionTime = 2.0f;
	public float waitTimeIn = 0.0f;
	public float waitTimeOut = 0.0f;
	public Color startColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	public Color endColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	private float timer;
	private bool queuedTransition = false;

	private TweenColor tweener;

	private EState state = EState.Wait;

	private bool reverseTransition;

	private bool fading = false;
	public bool Fading
	{
		get {return fading;}
	}

	public delegate void TransitionEnd();
	public event TransitionEnd onTransitionEnd;
	public event TransitionEnd onReverseTransitionEnd;

	public void Start()
	{
		widget.color = startColor;
	}

	public void Update()
	{
		if (state != EState.Wait)
			return;
		
		if (timer > 0.0f)
		{
			timer -= Time.deltaTime;

			if (timer < 0.0f)
			{
				if (reverseTransition)
				{
					tweener = TweenColor.Begin(this.gameObject, transitionTime, startColor);
				}
				else
				{
					tweener = TweenColor.Begin(this.gameObject, transitionTime, endColor);
				}
				state = EState.Fading;
			}
		}
	}

	public void Transition()
	{
		if (waitTimeIn == 0.0f)
		{
			TransitionNow();
			return;
		}

		widget.color = startColor;
		timer = waitTimeIn;
		reverseTransition = false;
		fading = true;
		state = EState.Wait;
	}


	[ContextMenu("Transition")]
	public void TransitionNow()
	{
		widget.color = startColor;
		tweener = TweenColor.Begin(this.gameObject, transitionTime, endColor);
		reverseTransition = false;
		fading = true;
		state = EState.Fading;
	}

	public void ReverseTransition()
	{
		if (waitTimeOut == 0.0f)
		{
			ReverseTransitionNow();
			return;
		}

		if(fading)
		{
			tweener.mFactor = 1.0f;
			queuedTransition = true;
		}

		widget.color = endColor;
		timer = waitTimeOut;
		reverseTransition = true;
		fading = true;
		state = EState.Wait;
	}
 
	[ContextMenu("ReverseTransition")]
	public void ReverseTransitionNow()
	{
		widget.color = endColor;
		tweener = TweenColor.Begin(this.gameObject, transitionTime, startColor);
		reverseTransition = true;
		fading = true;
		state = EState.Fading;
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

		state = EState.Done;
		fading = false;

		if (queuedTransition)
		{
			state = EState.Wait;
			queuedTransition = false;
		}
	}
}
