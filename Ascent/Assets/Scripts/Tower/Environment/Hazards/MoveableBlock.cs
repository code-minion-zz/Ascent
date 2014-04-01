using UnityEngine;
using System.Collections;

public class MoveableBlock : Interactable 
{
	public bool grabbed;

	public bool IsInMotion;

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
		if(!IsInMotion)
		{
			if(Input.GetKeyUp(KeyCode.UpArrow))
			{
				MoveAlongGrid(new Vector3(0, 0.0f, offset));
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				MoveAlongGrid(new Vector3(0, 0.0f, -offset));
			}
			else if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				MoveAlongGrid(new Vector3(offset, 0.0f, 0));
			}
			else if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				MoveAlongGrid(new Vector3(-offset, 0.0f, 0));
			}
		}

		if (IsInMotion)
		{
			timeAccum += Time.deltaTime;
			if(timeAccum > moveTime)
			{
				timeAccum = 1.0f;
				IsInMotion = false;
			}

			 transform.position = Vector3.Lerp(startPos, targetPos, timeAccum / moveTime);
		}

		//GetComponent<Shadow>().Process();
		//GetComponentInChildren<CharacterTilt>().Process();
	}

	public void MoveAlongGrid(Vector3 direction)
	{
		if (!IsInMotion)
		{
			SoundManager.PlaySound(AudioClipType.stonedrag, transform.position, 1f);
			IsInMotion = true;
			timeAccum = 0.0f;
			startPos = transform.position;
			targetPos = startPos + direction.normalized * offset;
		}
	}

	public void Move(Vector3 direction)
	{
		if (!IsInMotion)
		{
			IsInMotion = true;
			timeAccum = 0.0f;
			startPos = transform.position;
			targetPos = startPos + direction.normalized * offset;
		}
	}
}
