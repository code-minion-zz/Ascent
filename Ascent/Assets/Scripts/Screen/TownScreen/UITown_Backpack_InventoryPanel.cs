using UnityEngine;
using System.Collections;

public class UITown_Backpack_InventoryPanel : UIPlayerMenuPanel
{
    public override void OnEnable()
    {
        // TODO : populate item list based on Backpack state
		ShowMenu();

        base.OnEnable();
    }

    public void Update()
    {

	}
	
	public override void OnMenuOK(InputDevice device)
	{
		// TODO: Change character's equipment

		parent.TransitionToPanel((int)UITown_Backpack_Window.EBackpackPanels.Backpack_Main_Panel);
	}
	
	public override void OnMenuCancel(InputDevice device)
	{
		parent.TransitionToPanel((int)UITown_Backpack_Window.EBackpackPanels.Backpack_Main_Panel);
	}

	protected void ShowMenu()
	{
		// TODO: Disable current inventory tab

		UITown_Backpack_Window.EBackpackTab tab = ((UITown_Backpack_Window)parent).CurrentTab;
		switch (tab)
		{
		case UITown_Backpack_Window.EBackpackTab.Accessory:
		{
			// TODO: enable accessories tab
			break;
		}
		case UITown_Backpack_Window.EBackpackTab.Consumable:
		{
			// TODO: enable consumables tab

			break;
		}
		}
	}
}
