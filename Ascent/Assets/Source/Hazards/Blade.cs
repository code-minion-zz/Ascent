using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour 
{
	private float damage;

	public void Initialise(float damage)
	{
		this.damage = damage;
	}

	void OnCollisionEnter(Collision collision)
	{
		// TODO: Deal damage to other object if it is a character
		damage = damage + damage - damage; // suppress the warning;
	}

}
