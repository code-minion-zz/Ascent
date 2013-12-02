using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHeroSelect_Window : UIPlayerMenuWindow
{
	public enum EHeroSelectPanels
	{
		Main = 0,
		LoadHero,
		NewHero,
        HeroSelected
	}

	public override void AddAllMenuPanels()
	{
		panels[(int)EHeroSelectPanels.Main] = GetComponentInChildren<UIHeroSelect_MainPanel>();
		panels[(int)EHeroSelectPanels.LoadHero] = GetComponentInChildren<UIHeroSelect_LoadHeroPanel>();
		panels[(int)EHeroSelectPanels.NewHero] = GetComponentInChildren<UIHeroSelect_NewHeroPanel>();
        panels[(int)EHeroSelectPanels.HeroSelected] = GetComponentInChildren<UIHeroSelect_HeroSelectedPanel>();

		foreach (KeyValuePair<int, UIPlayerMenuPanel> p in panels)
		{
			p.Value.gameObject.SetActive(false);
		}

		activePanel = panels[(int)EHeroSelectPanels.Main];
	}

	public override void TransitionToPanel(int panel)
	{
		activePanel.gameObject.SetActive(false);
		activePanel = panels[panel];
		activePanel.gameObject.SetActive(true);
	}

	public override void CloseWindow()
	{
		((UIHeroSelect_Screen)parentScreen).CloseWindow(this);

		base.CloseWindow();
	}
}
