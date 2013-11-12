// Developed by Mana Khamphanpheng 2013

// Dependencies
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class WanderAction : RAINAction
{
	public Vector3 directionVector;

	//GameObject that is going to wander and contain AI
	float distanceToWander;
	int minDistanceToWander;
	int maxDistanceToWander;
	int maxTurnAngle;

    public WanderAction()
    {
        actionName = "WanderAction";
    }

	public override void Start(AI ai)
	{

		base.Start(ai);

		//Load Parameters for Wander
		maxTurnAngle = ai.WorkingMemory.GetItem<int>("maxTurnAngle");
		minDistanceToWander = ai.WorkingMemory.GetItem<int>("minDistanceToWander");
		maxDistanceToWander = ai.WorkingMemory.GetItem<int>("maxDistanceToWander");

		//Calculate Distance to Wander
		distanceToWander = Random.Range(minDistanceToWander, maxDistanceToWander);

		//Set Random Direction
		Vector2 newVector = Random.insideUnitCircle;
		directionVector.x = newVector.x * distanceToWander;
		directionVector.z = newVector.y * distanceToWander;
		directionVector.y = 0;

		float angle = Vector3.Angle(ai.Body.transform.forward, directionVector);

		//Check if angle is larger than Max Turning Angle
		while (angle > maxTurnAngle)
		{

			//Set Random Direction
			newVector = Random.insideUnitCircle;
			directionVector.x = newVector.x * distanceToWander;
			directionVector.z = newVector.y * distanceToWander;
			directionVector.y = 0;

			angle = Vector3.Angle(ai.Body.transform.forward, directionVector);

		}

		//Random Chance for bigger angle of rotation - Helps to break out object from edge of nav mesh
		if ((Random.Range(0, 10)) < 2)
		{

			//Set Random Direction
			newVector = Random.insideUnitCircle;
			directionVector.x = newVector.x * distanceToWander;
			directionVector.z = newVector.y * distanceToWander;
			directionVector.y = 0;

			angle = Vector3.Angle(ai.Body.transform.forward, directionVector);
		}

		//Set Position to Move to relative to Current Position
		directionVector = ai.Body.transform.position + directionVector;
		//directionVector.y = Terrain.activeTerrain.SampleHeight(directionVector);

		//Set location into working memory for Move action in BT
		ai.WorkingMemory.SetItem<Vector3>("directionVector", directionVector);

	}

	public override ActionResult Execute(AI ai)
	{

		return ActionResult.SUCCESS;

	}

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}