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
		

		Transform acc = transform.Find ("BackpackTab/Accessories");
		Transform con = transform.Find ("BackpackTab/Consumables");

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

		tabs[0].SendMessage("OnHover", false,SendMessageOptions.DontRequireReceiver);

		initialised = true;
	}

	public override void OnEnable()
	{
		if (initialised)
		{
			UICamera.Notify(currentSelection.gameObject, "OnHover", true);
			SetTab(((UITownWindow)parent).CurrentTab);
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
		ToggleTab ();
	}

	public override void OnMenuRight(InputDevice device)
	{		
		ToggleTab ();
	}

	void ToggleTab()
	{
		UITownWindow.EBackpackTab tab = ((UITownWindow)parent).CurrentTab;
		if (tab == UITownWindow.EBackpackTab.Accessory) 
		{
			tab = UITownWindow.EBackpackTab.Consumable;
		}
		else
		{
			tab = UITownWindow.EBackpackTab.Accessory;
		}
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
	
	public void SetTab(UITownWindow.EBackpackTab tab)
	{
		Transform accessoryTab = transform.FindChild("Tabs/Accessory Tab");
		Transform consumableTab = transform.FindChild("Tabs/Consumable Tab");

		if (tab == UITownWindow.EBackpackTab.Accessory)
		{
			UICamera.Notify(accessoryTab.gameObject, "OnPress", true);
			UICamera.Notify(consumableTab.gameObject, "OnPress", false);
		}
		else
		{
			UICamera.Notify(accessoryTab.gameObject, "OnPress", false);
			UICamera.Notify(consumableTab.gameObject, "OnPress", true);
		}
	}
}
