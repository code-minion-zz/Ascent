using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Xml.Serialization.XmlInclude(typeof(AccuracyItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(AttackItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(CriticalItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(DodgeItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(ExperienceItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(GoldItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(MDefenceItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(PDefenceItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(SecondaryStatItemProperty))]
[System.Xml.Serialization.XmlInclude(typeof(SpecialItemProperty))]
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

	public abstract void Initialise();
	public abstract void CheckCondition();
	public abstract void DoAction ();
}