using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHeroSelect_MainPanel : UIPlayerMenuPanel 
{
	private enum EButtons
	{
		Load,
		New,

		MAX,
	}

	public virtual void Start()
	{
		buttons = new UIButton[(int)EButtons.MAX];

		buttons[(int)EButtons.Load] = transform.FindChild("Load Button").GetComponent<UIButton>();
		buttons[(int)EButtons.New] = transform.FindChild("New Button").GetComponent<UIButton>();

		currentHighlightedButton = (int)EButtons.Load;
		currentSelection = buttons[(int)EButtons.Load];

		UICamera.Notify(currentSelection.gameObject, "OnHover", true);

		buttonMax = (int)EButtons.MAX;

		initialised = true;
	}

	public override void OnEnable()
	{
		if (initialised)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
		}

		base.OnEnable();
	}

	public override void OnDisable()
	{
		if (initialised)
		{
			currentHighlightedButton = (int)EButtons.Load;
			currentSelection = buttons[(int)EButtons.Load];
		}

		base.OnDisable();
	}

	public override void OnMenuUp(InputDevice device)
	{
		UICamera.Notify(currentSelection.gameObject, "OnHover", false);

		currentSelection = PrevButton();

		UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}

	public override void OnMenuDown(InputDevice device)
	{
		UICamera.Notify(currentSelection.gameObject, "OnHover", false);

		currentSelection = NextButton();

		UICamera.Notify(currentSelection.gameObject, "OnHover", true);
	}

	public override void OnMenuOK(InputDevice device)
	{
		UICamera.Notify(currentSelection.gameObject, "OnPress", true);

		EButtons current = (EButtons)currentHighlightedButton;

		switch (current)
		{
			case EButtons.Load:
				{
					parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.LoadHero);
				}
				break;
			case EButtons.New:
				{
					parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.NewHero);
				}
				break;
		}
	}


	public override void OnMenuCancel(InputDevice device)
	{
		parent.CloseWindow();
	}
}
