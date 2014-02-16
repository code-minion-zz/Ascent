using UnityEngine;
using System.Collections;

public class Blessing : StatusEffect 
{
	protected int purchaseValue;
	public int PurchaseValue
	{
		get { return purchaseValue; }
		set { purchaseValue = value; }
	}
}
