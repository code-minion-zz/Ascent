using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchPanel : MonoBehaviour
{
    private bool isDown = false;
    public GameObject switchModel;
    public Color pressedColor = new Color(0.0f, 0.65f, 0.0f);
    public Color unpressedColor = new Color(0.65f, 0.0f, 0.0f);

	public delegate void SwitchChange(SwitchPanel switchPanel);
	public event SwitchChange onSwitchOn;
	public event SwitchChange onSwitchOff;

	private List<GameObject> thingsOnMe = new List<GameObject>();

	private bool firedEvent;

    public bool IsDown
    {
        get
        {
            return isDown;
        }

        set
        {
            if (isDown != value)
            {
                SoundManager.PlaySound(AudioClipType.switchclick, transform.position + Vector3.up, 10f);
            }

            isDown = value;

        }
    }

	public void FixedUpdate()
    {
        if (isDown)
        {
            Vector3 scale = switchModel.transform.localScale;
            scale.y = 1.0f;
            switchModel.transform.localScale = scale;
            switchModel.renderer.material.color = pressedColor;
        }
        else
        {
            Vector3 scale = switchModel.transform.localScale;
            scale.y = 5.0f;
            switchModel.transform.localScale = scale;
            switchModel.renderer.material.color = unpressedColor;
        }
    }

    void OnCollisionStay(Collision collision)
	{
		if (!IsDown)
		{
			IsDown = true;

			if (!firedEvent && onSwitchOn != null)
			{
				onSwitchOn.Invoke(this);
				firedEvent = true;
			}
		}

		if (!thingsOnMe.Contains(collision.gameObject))
			thingsOnMe.Add(collision.gameObject);
	}

    void OnCollisionExit(Collision collision)
    {
		if (IsDown)
		{
			if (!thingsOnMe.Contains(collision.gameObject))
			{
				return;
			}

			thingsOnMe.Remove(collision.gameObject);

			if (thingsOnMe.Count > 0)
			{
				return;
			}

			IsDown = false;
			firedEvent = false;

			if (onSwitchOff != null)
			{
				onSwitchOff.Invoke(this);
			}
		}
    }
}
