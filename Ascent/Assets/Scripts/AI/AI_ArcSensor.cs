using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AI_ArcSensor : AI_Sensor
{
	public float radius = 5.0f;

	float arcAngle = 45.0f;
	Vector3 arcLine;
	Vector3 arcLine2;

	public void OnDrawGizmos()
	{
		Handles.DrawWireArc(transform.parent.position, Vector3.up, arcLine, arcAngle, radius);
	}

	public void Update()
	{
		arcLine = MathRectHelper.RotateAboutPoint(transform.parent.forward * radius, transform.parent.position, -arcAngle * 0.5f);
		arcLine2 = MathRectHelper.RotateAboutPoint(transform.parent.forward * radius, transform.parent.position, arcAngle * 0.5f);

		Debug.DrawLine(transform.parent.position, transform.parent.position + arcLine.normalized * radius); // To the rotated arc
		Debug.DrawLine(transform.parent.position, transform.parent.position + arcLine2.normalized * radius); // To the rotated arc
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
			if (MathRectHelper.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
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
			if (MathRectHelper.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
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
			if (MathRectHelper.IsWithinCircleArc(c.transform.position, transform.position, arcLine, arcLine2, radius))
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
