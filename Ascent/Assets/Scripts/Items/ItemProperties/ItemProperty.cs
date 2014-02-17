using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Xml.Serialization.XmlInclude(typeof(AttackItemProperty))]
public abstract class ItemProperty 
{
    public enum EType
    {
        None = -1,
        Attack = 0,
        Accuracy,
        CriticalHit,
        Dodge,
        Experience,
        Gold,
        MDefence,
        PDefence,
        Special,

        Max,
    }

	public float timeAccumulator;
	public abstract void Initialise();
	public abstract void CheckCondition();
	public abstract void DoAction ();
}