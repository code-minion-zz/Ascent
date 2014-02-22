using UnityEngine;
using System.Collections;

public class UITown_Chapel : UITown_RadialPanel {

	public override void Initialise()
	{
		base.Initialise();
		
		initialised = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void OnEnable()
	{		
		base.OnEnable();
		
		if (initialised) (parent as UITownWindow).SetTitle("Chapel");
	}
}
