using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Xml.Serialization.XmlInclude(typeof(AttackItemProperty))]
public abstract class ItemProperty 
{
	public float timeAccumulator;
	public abstract void Initialise();
	public abstract void CheckCondition();
	public abstract void DoAction ();
}