using UnityEngine;
using System.Collections;

public class WizardAnimator : MonoBehaviour 
{
    Animation anim;
	// Use this for initialization
	void Start () 
    {
        anim = GetComponentInChildren<Animation>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        anim.Play("Run");
	}
}
