using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AI_ArcSensor : AI_Sensor
{
	public float radius = 5.0f;
	public float arcAngle = 45.0f;

	private Vector3 arcLine;
	private Vector3 arcLine2;

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		Handles.DrawWireArc(transform.parent.position, Vector3.up, arcLine, arcAngle, radius);
	}
#endif

	public override void Update()
	{
		base.Update();

		arcLine = MathUtility.RotateAboutPoint(transform.parent.forward * radius, transform.parent.position, -arcAngle * 0.5f);
		arcLine2 = MathUtility.RotateAboutPoint(transform.parent.forward * radius, transform.parent.position, arcAngle * 0.5f);

		Debug.DrawLine(transform.parent.position, transform.parent.position + arcLine); // To the rotated arc
		Debug.DrawLine(transform.parent.position, transform.parent.position + arcLine2); // To the rotated arc
	}

	public override bool SenseAll(ref List<Character> sensedCharacters)
	{
		// Get list of enemies
		List<Character> characters = Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies;

		// Add the heroes
		List<Player> players = Game.Singleton.Players;
		foreach (Player p in players)
		{
			characters.Add(p.Hero.GetComponent<Hero>());
		}

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (MathUtility.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
			{
				if (!sensedCharacters.Contains(c))
				{
					sensedCharacters.Add(c);
				}
			}
		}

		if (sensedCharacters.Count > 0)
		{
			return true;
		}

		return false;
	}

	public override bool SenseAllies(ref List<Character> sensedCharacters)
	{
		// Get list of enemies
		List<Character> characters = Game.Singleton.Tower.CurrentFloor.CurrentRoom.Enemies;

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (MathUtility.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
			{
				if (!sensedCharacters.Contains(c))
				{
					sensedCharacters.Add(c);
				}
			}
		}

		if (sensedCharacters.Count > 0)
		{
			return true;
		}

		return false;
	}

	public override bool SenseEnemies(ref List<Character> sensedCharacters)
	{
		List<Character> characters = new List<Character>();

		// Add the heroes
		List<Player> players = Game.Singleton.Players;
		foreach (Player p in players)
		{
			characters.Add(p.Hero.GetComponent<Hero>());
		}

		// Check all characters to see if there is a collision
		foreach (Character c in characters)
		{
			if (MathUtility.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
			{
				if (!sensedCharacters.Contains(c))
				{
					sensedCharacters.Add(c);
				}
			}
		}

		if (sensedCharacters.Count > 0)
		{
			return true;
		}

		return false;
	}
}
