//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System;

public class UIItemButton : UIButton
{
	public enum EType
	{
		NONE,
		ACCESSORY,
		CONSUMABLE
	}
	
	public UISprite Icon;
	public UILabel Name;
	public EType Type = EType.NONE;
	Item linkedItem = null; // Item linked to this button
	public Item LinkedItem
	{
		get 
		{ 
			if (linkedItem != null) return linkedItem;

			Debug.Log("No Item Linked to this Item Button");
			return null;
		}

		set { linkedItem = value; }
	}

	void Start()
	{

	}

	protected override void OnEnable()
	{
		base.OnEnable();
		
		if (Icon == null)
		{
			Icon = GetComponent<UISprite>();
			if (Icon == null) 
			{
				Icon = transform.Find ("Icon").GetComponent<UISprite>();
			}
		}
		
		if (Name == null)
		{
			Name = GetComponentInChildren<UILabel>();
		}
	}

	/// <summary>
	/// Makes button blank and hides it
	/// </summary>
	public void Reset()
	{
		Type = EType.NONE;
		linkedItem = null;
		NGUITools.SetActive(gameObject, false);
	}
}
