using UnityEngine;
using System.Collections.Generic;

public class UITown_AccShop : UITown_Shop
{	
	public override void Initialise()
	{
		shopType = UIItemButton.EType.ACCESSORY;

		base.Initialise();

	}

	public override void OnEnable()
	{
		base.OnEnable();
				
		if (initialised)
		{
		}
	}

	protected override void ChangeTitle()
	{
		switch (shopMode)
		{
		case EMode.BUY:
			(parent as UITownWindow).SetTitle("Jeweller");
			break;
		case EMode.REPAIR:
			(parent as UITownWindow).SetTitle("Jeweller - Repair");
			break;
		case EMode.APPRAISE:
			(parent as UITownWindow).SetTitle("Jeweller - Appraise");
			break;
		}
	}
}