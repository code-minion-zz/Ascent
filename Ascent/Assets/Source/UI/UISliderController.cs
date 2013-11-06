using UnityEngine;
using System.Collections;

public class UISliderController : MonoBehaviour 
{

	UISlider mSlider;

	void Start () 
    { 
        mSlider = GetComponent<UISlider>(); 
        Update(); 
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
	
	public void MoveSlider(KeyCode key)
	{
		switch (key)
			{
			case KeyCode.LeftArrow:
			//mSlider.s
				break;
			case KeyCode.RightArrow:
				break;
			case KeyCode.UpArrow:
				break;
			case KeyCode.DownArrow:
				break;
		}
	}	
}
