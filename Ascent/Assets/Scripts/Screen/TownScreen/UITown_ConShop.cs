using UnityEngine;
using System.Collections.Generic;

public class UITown_ConShop : UITown_Shop
{
	public override void Initialise()
	{
		shopType = UIItemButton.EType.CONSUMABLE;

		base.Initialise();

	}
}