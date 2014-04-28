using UnityEngine;
using System.Collections;

public class SwitchPanel : MonoBehaviour
{
    private bool isDown = false;
    public GameObject switchModel;
    public Color pressedColor = new Color(0.0f, 0.65f, 0.0f);
    public Color unpressedColor = new Color(0.65f, 0.0f, 0.0f);

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
                SoundManager.PlaySound(AudioClipType.switchclick, transform.position, 1f);
            }

            isDown = value;

        }
    }

    public void Update()
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
		IsDown = true;
	}

    void OnCollisionExit(Collision collision)
    {
        IsDown = false;
    }
}
