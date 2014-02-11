/// <summary>
/// Modified from NGUI/Examples/Spin. Spins a GameObject at a constant rate.
/// </summary>

using UnityEngine;

/// <summary>
/// Want something to spin? Attach this script to it. Works equally well with rigidbodies as without.
/// </summary>

public class Spin : MonoBehaviour
{
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	Rigidbody mRb;
	Transform mTrans;

	public float ElapsedSeconds = 0f;

	void Start ()
	{
		mTrans = transform;
		mRb = rigidbody;
	}

	void Update ()
	{
		if (mRb == null)
		{
			ApplyDelta(Time.deltaTime);
		}
	}
	void FixedUpdate ()
	{
		if (mRb != null)
		{
			ApplyDelta(Time.deltaTime);
		}
	}

	public void ApplyDelta (float delta)
	{
		ElapsedSeconds += delta;
		delta *= Mathf.Rad2Deg * Mathf.PI * 2f;
		Quaternion offset = Quaternion.Euler(rotationsPerSecond * delta);

		if (mRb == null)
		{
			mTrans.rotation = mTrans.rotation * offset;
		}
		else
		{
			mRb.MoveRotation(mRb.rotation * offset);
		}
	}
}
