using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITown_Backpack_BackpackPanel : UIPlayerMenuPanel 
{
	enum EButtons
	{
		ACC1 = 0,
		ACC2,
		ACC3,
		ACC4,
		ITM1,
		ITM2,
		ITM3,
		ITM4,

		MAX,
	}

	int		 lastActiveButton = 0;



	public virtual void Start()
	{
		buttons = new UIButton[(int)EButtons.MAX];
		

		Transform acc = transform.FindChild ("BackpackTab");
		Transform con = transform.FindChild ("BackpackTab");

		buttons[(int)EButtons.ACC1] = acc.FindChild("Accessory Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC2] = acc.FindChild("Accessory Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC3] = acc.FindChild("Accessory Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC4] = acc.FindChild("Accessory Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM1] = con.FindChild("Consumable Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM2] = con.FindChild("Consumable Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM3] = con.FindChild("Consumable Slot").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM4] = con.FindChild("Consumable Slot").GetComponent<UIButton>();

		currentButton = (int)EButtons.ACC1;
		currentSelection = buttons[(int)EButtons.ACC1];

		lastActiveButton = currentButton;

		UICamera.Notify(currentSelection.gameObject, "OnHover", true);

		buttonMax = (int)EButtons.MAX;

		//tabs[0].SendMessage("OnHover", false,SendMessageOptions.DontRequireReceiver);

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
			lastActiveButton = currentButton;
			//currentButton = (int)EButtons.ACC1;
			currentSelection = buttons[lastActiveButton];
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
	
	public override void OnMenuLeft(InputDevice device)
	{
	}

	public override void OnMenuRight(InputDevice device)
	{		
	}

	public override void OnMenuOK(InputDevice device)
	{
		UICamera.Notify(currentSelection.gameObject, "OnPress", true);

		EButtons current = (EButtons)currentButton;

		parent.TransitionToPanel((int)UITownWindow.EBackpackPanels.INVENTORY);
	}


	public override void OnMenuCancel(InputDevice device)
	{
		// TODO: Link back to the main town screen

		//parent.CloseWindow();
	}

	public override void OnMenuHax(InputDevice device)
	{
		Game.Singleton.LoadLevel("overhaul", Game.EGameState.Tower);
	}
}
