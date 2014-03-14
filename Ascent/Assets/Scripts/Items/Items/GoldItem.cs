using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoldItem : Item
{
    private int goldValue;

    public int GoldValue
    {
        get { return goldValue; }
        set { goldValue = value; }
    }
}
