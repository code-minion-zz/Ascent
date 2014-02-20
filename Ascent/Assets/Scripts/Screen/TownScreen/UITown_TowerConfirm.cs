using UnityEngine;
using System.Collections;

public class UITown_TowerConfirm : UITown_RadialPanel {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Initialise()
	{
		base.Initialise();

		initialised = true;

	}
	
	public override void OnEnable ()
	{
		base.OnEnable ();
		
		if (initialised)
		{
			// deactivate arrow
			(parent as UITownWindow).ShowArrow(false);
			(parent as UITownWindow).ShowInfo(false);
		}
	}

	public override void OnDisable ()
	{
		base.OnDisable ();
		
		if (initialised)
		{
			// deactivate arrow
			(parent as UITownWindow).ShowArrow(true);
			(parent as UITownWindow).ShowInfo(true);
		}
	}
//	
//	public override void OnMenuOK()
//	{
//		
//	}

	public override void OnMenuCancel(InputDevice input)
	{
		(parent as UITownWindow).RequestTransitionToPanel(0);
	}
}
