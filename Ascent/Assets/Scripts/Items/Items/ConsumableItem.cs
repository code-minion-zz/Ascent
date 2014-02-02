using UnityEngine;
using System.Collections;

public class ConsumableItem : Item
{
	public enum EConsumableType
	{
		INVALID = -1,

		Health,

		MAX
	}

	int charges;

	public int Charges
	{
		get{ return charges; }
		set { charges = value; }
	}

    public virtual void Consume()
    {
    }
}
