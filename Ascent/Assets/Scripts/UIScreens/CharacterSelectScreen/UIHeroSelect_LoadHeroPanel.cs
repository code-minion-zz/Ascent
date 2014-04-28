using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHeroSelect_LoadHeroPanel : UIPlayerMenuPanel
{
    UIGrid grid;
    Dictionary<UIButton, HeroSaveData> saves;

    public override void Initialise()
    {
        grid = transform.FindChild("Grid").transform.GetComponent<UIGrid>();

		initialised = true;
    }

    public override void OnEnable()
    {
		base.OnEnable();

		if (initialised)
		{
			OnSaveListChanged();

			if (currentSelection != null)
			{
				UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			}
		}
    }

    public override void OnMenuUp(InputDevice device)
    {
       base.OnMenuUp(device);
    }

    public override void OnMenuDown(InputDevice device)
    {
        base.OnMenuDown(device);
    }

    public override void OnMenuOK(InputDevice device)
    {
		if (currentSelection != null)
		{
			UICamera.Notify(currentSelection.gameObject, "OnPress", true);

			//Load the selected
			if (currentSelection != null)
			{
				Hero LoadedHero = AscentGameSaver.LoadHero(saves[currentSelection]);
				LoadedHero.Initialise(device, saves[currentSelection]);
				parent.Player.Hero = LoadedHero;
				LoadedHero.transform.parent = parent.Player.transform;
				parent.Player.Hero.gameObject.SetActive(false);
				//LoadedHero.HeroController.CanUseInput = true;
			}

			parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.HeroSelected);
		}
    }

	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
	}

    public void OnSaveListChanged()
    {
        // Grab saves to display
        List<HeroSaveData> heroSaves = AscentGameSaver.SaveData.heroSaves;

        if (buttons != null)
        {
            foreach (UIButton b in buttons)
            {
                Destroy(b.gameObject);
            }
        }

		if (heroSaves.Count > 0)
		{

			// Sort the list ascending
			heroSaves.Sort(AscentGameSaver.SortListByDateAscending);

			// Create and store buttons out of the saves
			buttons = new UIButton[heroSaves.Count];
			saves = new Dictionary<UIButton, HeroSaveData>();

			for (int i = 0; i < heroSaves.Count; ++i)
			{
				GameObject go = Instantiate(Resources.Load("Prefabs/UI/SaveEntry")) as GameObject;
				buttons[i] = go.GetComponentInChildren<UIButton>();
				go.transform.parent = grid.transform;
				go.transform.position = Vector3.zero;
				go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				go.transform.name = i.ToString();

				buttons[i].GetComponentInChildren<UILabel>().text = heroSaves[i].ToString();

				saves.Add(buttons[i], heroSaves[i]);
			}


			currentHighlightedButton = 0;
			currentSelection = buttons[0];

			UICamera.Notify(currentSelection.gameObject, "OnHover", true);

			buttonMax = buttons.Length;

			grid.Reposition();
		}
    }
}
