using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

	Character owner;
	public StatBar hpBar;
	public StatBar spBar;
	// UIContainer buffIcons
	// UIContainer skillIcons

	public void Init(Character _owner)
	{
		owner = _owner;
		hpBar.Init(StatBar.eStat.HP,owner);
		spBar.Init(StatBar.eStat.SP,owner);
		Debug.Log("PlayerHUD");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}



}
