using UnityEngine;
using System.Collections.Generic;

public class UITown_AccShop : UITown_Shop
{	
	public override void Initialise()
	{
		shopType = UIItemButton.EType.ACCESSORY;

		base.Initialise();

	}
}