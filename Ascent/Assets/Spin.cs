using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	// Functions
	// Public
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {	
		spinVec = initSpin;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		this.GetComponent<Transform>().Rotate(spinVec); 
	}
	// Protected
	
	// Private
	
	// Members
	//Public
	public Vector3 initSpin = Vector3.up;
	
	//Protected
	protected Vector3 spinVec;
	
	//Private
}
