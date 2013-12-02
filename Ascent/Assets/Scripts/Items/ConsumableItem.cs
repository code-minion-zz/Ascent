using UnityEngine;
using System.Collections;

public class ConsumableItem : Item
{
	int charges;

	public int Charges
	{
		get{ return charges; }
		set { charges = value; }
	}

    public virtual void Consume()
    {
		int i;
		for (i = 0; i < itemProperties.size; ++i)
		{
			//itemProperties[i];
		}
        // To be derived.
    }
}
