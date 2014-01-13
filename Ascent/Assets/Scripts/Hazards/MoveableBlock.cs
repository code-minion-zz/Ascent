using UnityEngine;
using System.Collections;

public class MoveableBlock : Interactable 
{
	public bool grabbed;

	public bool moving;

	float offset = 1.0f;
	float moveTime = 0.5f;
	float timeAccum;

	Vector3 startPos;
	Vector3 targetPos;

	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		//GetComponent<Shadow>().Initialise();
		//GetComponentInChildren<CharacterTilt>().Process();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!moving)
		{
			if(Input.GetKeyUp(KeyCode.UpArrow))
			{
				Move(new Vector3(0, 0.0f, offset));
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				Move(new Vector3(0, 0.0f, -offset));
			}
			else if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				Move(new Vector3(offset, 0.0f, 0));
			}
			else if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				Move(new Vector3(-offset, 0.0f, 0));
			}
		}

		if (moving)
		{
			timeAccum += Time.deltaTime;
			if(timeAccum > moveTime)
			{
				timeAccum = 1.0f;
				moving = false;
			}

			 transform.position = Vector3.Lerp(startPos, targetPos, timeAccum / moveTime);
		}

		//GetComponent<Shadow>().Process();
		//GetComponentInChildren<CharacterTilt>().Process();
	}

	public void Move(Vector3 direction)
	{
		if (!moving)
		{
			moving = true;
			timeAccum = 0.0f;
			startPos = transform.position;
			targetPos = startPos + direction.normalized * offset;
		}
	}
}
