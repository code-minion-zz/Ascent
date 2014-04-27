using UnityEngine;
using System.Collections;

public class UIHeroSelect_NewHeroPanel : UIPlayerMenuPanel 
{
    private enum EButtons
    {
        Warrior,
        Rogue,
        Mage,

        MAX,
    }

    public virtual void Start()
    {
        buttons = new UIButton[(int)EButtons.MAX];

        buttons[(int)EButtons.Warrior] = transform.FindChild("Warrior").GetComponent<UIButton>();
        buttons[(int)EButtons.Rogue] = transform.FindChild("Rogue").GetComponent<UIButton>();
        buttons[(int)EButtons.Mage] = transform.FindChild("Mage").GetComponent<UIButton>();

        currentHighlightedButton = (int)EButtons.Warrior;
        currentSelection = buttons[(int)EButtons.Warrior];

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

        parent.Player.CreateHero(Character.EHeroClass.Warrior);

		parent.Player.Hero.gameObject.SetActive(false);
		parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.HeroSelected);
    }


	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UIHeroSelect_Window.EHeroSelectPanels.Main);
	}
}
