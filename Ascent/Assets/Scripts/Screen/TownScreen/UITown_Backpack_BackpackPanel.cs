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

	BetterList<GameObject> tabs = new BetterList<GameObject>();


	public virtual void Start()
	{
		buttons = new UIButton[(int)EButtons.MAX];
		
		tabs.Add(transform.FindChild("Tabs/Accessory Tab").gameObject);
		tabs.Add(transform.FindChild("Tabs/Consumable Tab").gameObject); 

		Transform accessories = transform.Find ("Accessories");
		Transform consumables = transform.Find ("Consumables");

		buttons[(int)EButtons.ACC1] = accessories.transform.FindChild("Accessory1").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC2] = accessories.transform.FindChild("Accessory2").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC3] = accessories.transform.FindChild("Accessory3").GetComponent<UIButton>();
		buttons[(int)EButtons.ACC4] = accessories.transform.FindChild("Accessory4").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM1] = consumables.transform.FindChild("Consumable1").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM2] = consumables.transform.FindChild("Consumable2").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM3] = consumables.transform.FindChild("Consumable3").GetComponent<UIButton>();
		buttons[(int)EButtons.ITM4] = consumables.transform.FindChild("Consumable4").GetComponent<UIButton>();

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
			SetTab(((UITown_Backpack_Window)parent).CurrentTab);
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
		UITown_Backpack_Window.EBackpackTab tab = ((UITown_Backpack_Window)parent).CurrentTab;
		if (tab == UITown_Backpack_Window.EBackpackTab.Accessory) 
		{
			tab = UITown_Backpack_Window.EBackpackTab.Consumable;
		}
		else
		{
			tab = UITown_Backpack_Window.EBackpackTab.Accessory;
		}
	}

	public override void OnMenuOK(InputDevice device)
	{
		UICamera.Notify(currentSelection.gameObject, "OnPress", true);

		EButtons current = (EButtons)currentButton;

		parent.TransitionToPanel((int)UITown_Backpack_Window.EBackpackPanels.Backpack_Inventory_Panel);
	}


	public override void OnMenuCancel(InputDevice device)
	{
		// TODO: Link back to the main town screen

		//parent.CloseWindow();
	}
	
	public void SetTab(UITown_Backpack_Window.EBackpackTab tab)
	{
		Transform accessoryTab = transform.FindChild("Tabs/Accessory Tab");
		Transform consumableTab = transform.FindChild("Tabs/Consumable Tab");

		if (tab == UITown_Backpack_Window.EBackpackTab.Accessory)
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
