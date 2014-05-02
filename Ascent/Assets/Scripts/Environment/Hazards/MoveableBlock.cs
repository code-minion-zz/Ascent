using UnityEngine;
using System.Collections;

public class MoveableBlock : Interactable 
{
	public bool grabbed;

	public bool IsInMotion;

	float offset = 1.0f;
	float moveTime = 0.5f;
	public float timeAccum;

	Vector3 startPos;
	Vector3 targetPos;

    public Transform northWestCorner;
    public Transform northEastCorner;
    public Transform southWestCorner;
    public Transform southEastCorner;

	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		//GetComponent<Shadow>().Initialise();
		//GetComponentInChildren<CharacterTilt>().Process();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
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
	}

    private Vector3 left = new Vector3(-1.0f, 0.0f, 0.0f);
    private Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 south = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 north = new Vector3(0.0f, 0.0f, 1.0f);

	public void MoveAlongGrid(Vector3 direction)
	{
		if (!IsInMotion)
		{
            Vector3 normalisedDirection = direction.normalized;

			SoundManager.PlaySound(AudioClipType.stonedrag, transform.position, 1f);
			IsInMotion = true;
			timeAccum = 0.0f;
			startPos = transform.position;


			targetPos = startPos + normalisedDirection * offset;

            if(normalisedDirection == left)
            {
                EffectFactory.Singleton.CreateMoveBlockDust(northWestCorner.position, Quaternion.identity);
                EffectFactory.Singleton.CreateMoveBlockDust(southWestCorner.position, Quaternion.identity);
            }
            else if (normalisedDirection == right)
            {
                EffectFactory.Singleton.CreateMoveBlockDust(northEastCorner.position, Quaternion.identity);
                EffectFactory.Singleton.CreateMoveBlockDust(southEastCorner.position, Quaternion.identity);
            }
            else if (normalisedDirection == south)
            {
                EffectFactory.Singleton.CreateMoveBlockDust(southWestCorner.position, Quaternion.identity);
                EffectFactory.Singleton.CreateMoveBlockDust(southEastCorner.position, Quaternion.identity);
            }
            else if (normalisedDirection == north)
            {
                EffectFactory.Singleton.CreateMoveBlockDust(northWestCorner.position, Quaternion.identity);
                EffectFactory.Singleton.CreateMoveBlockDust(northEastCorner.position, Quaternion.identity);
            }

            EffectFactory.Singleton.CreateMoveBlockDust(transform.position, Quaternion.identity);
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

	public bool CheckDirectionForAnotherBlock(Vector3 direction, float magnitude)
	{
		int layerMask = ~(1 << (int)Layer.Floor | 1 << (int)Layer.Hero);

		RaycastHit hit;
		Vector3 position = transform.position;
		position.y += 0.5f;
		if (Physics.Raycast(new Ray(position, direction), out hit, 1.0f, layerMask))
		{
			return true;
		}

		return false;
	}
}
