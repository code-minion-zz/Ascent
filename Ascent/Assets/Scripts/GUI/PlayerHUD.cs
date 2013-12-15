using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour {

	Hero owner;
	public StatBar hpBar;
	public StatBar spBar;
    public UILabel ability1;
    public UILabel ability2;
    public UILabel ability3;
    public UILabel ability4;
    public UILabel item1;
    public UILabel item2;
    public UILabel item3;
	// UIContainer buffIcons
	// UIContainer skillIcons

	public void Init(Hero _owner)
	{
		owner = _owner;
		hpBar.Init(StatBar.eStat.HP,owner);
		spBar.Init(StatBar.eStat.SP,owner);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        List<Action> abilities = owner.Abilities;

        ability1.text = abilities[1].Name + ". CD: " + abilities[1].RemainingCooldown;
        if (abilities[1].RemainingCooldown <= 0.0f)
        {
            ability1.color = Color.green;
        }
        else
        {
            ability1.color = Color.red;
        }
        

        ability2.text = abilities[2].Name + ". CD: " + abilities[2].RemainingCooldown;
        if (abilities[2].RemainingCooldown <= 0.0f)
        {
            ability2.color = Color.green;
        }
        else
        {
            ability2.color = Color.red;
        }

        ability3.text = abilities[3].Name + ". CD: " + abilities[3].RemainingCooldown;
        if (abilities[3].RemainingCooldown <= 0.0f)
        {
            ability3.color = Color.green;
        }
        else
        {
            ability3.color = Color.red;
        }

        ability4.text = abilities[4].Name + ". CD: " + abilities[4].RemainingCooldown;
        if (abilities[4].RemainingCooldown <= 0.0f)
        {
            ability4.color = Color.green;
        }
        else
        {
            ability4.color = Color.red;
        }

        //ConsumableItem[] items = owner.HeroBackpack.ConsumableItems;
        //int itemCount = items.Length;

        //if (itemCount > 0)
        //{
        //    item1.text = items[0].Name + ". Qty: " + "0" + ". CD: " + "0";

        //    //if(itemCount > 1)
        //    //{
        //    //    item2.text = ;

        //    //    if(itemCount > 2)
        //    //    {
        //    //         item3.text = ;
        //    //    }
        //    //}
        //}
        //else
        {
            item1.text = "NoItem" + ". Qty: " + "0" + ". CD: " + "0";
            item2.text = "NoItem" + ". Qty: " + "0" + ". CD: " + "0";
            item3.text = "NoItem" + ". Qty: " + "0" + ". CD: " + "0";
        }
	}



}
