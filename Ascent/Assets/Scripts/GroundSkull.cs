using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundSkull : MonoBehaviour 
{
	private const int maxEyes = 2; // 0: LeftEye, 1: RightEye

	public Transform[] eyePositions = new Transform[maxEyes];

	private bool[] eyesLit = new bool[maxEyes]; 
	//private GroundSkullEye[] leftEye = new GroundSkullEye[maxEyes];

	public SwitchPanel[] observedSwitches = new SwitchPanel[maxEyes];

	private List<GameObject>[] flames = new List<GameObject>[maxEyes];

	public bool BothEyesOn
	{
		get { return eyesLit[0] && eyesLit[1]; }
	}

	void Start()
	{
		LightAlreadyEnabledEyes();

		for (int i = 0; i < maxEyes; ++i)
		{
			flames[i] = new List<GameObject>();
			observedSwitches[i].onSwitchOn += SwitchOn;
			observedSwitches[i].onSwitchOff += SwitchOff;
		}
	}

	void OnEnable()
	{
		LightAlreadyEnabledEyes();
	}

	void OnDisable()
	{
		if (flames[0] != null)
		{
			for (int i = 0; i < maxEyes; ++i)
			{
				for (int j = 0; j < flames[i].Count; ++j)
				{
					flames[i][j].GetComponent<GroundSkullEye>().FadeOutAndDie();
				}
			}
			flames[0].Clear();
			flames[1].Clear();
		}
	}

	private void LightAlreadyEnabledEyes()
	{
		for (int i = 0; i < maxEyes; ++i)
		{
			eyesLit[i] = observedSwitches[i].IsDown;

			if (eyesLit[i])
			{
				GameObject blueGo = EffectFactory.Singleton.CreateBlueFlame(eyePositions[i].position, Quaternion.identity);
				flames[i].Add(blueGo);
			}
		}
	}
	
	public void SwitchOn(SwitchPanel switchPanel)
	{
		// Create and move flame from switch to the eye

		Vector3 startPos = switchPanel.transform.position;
		startPos.y = 1.0f;
		GameObject blueGO = EffectFactory.Singleton.CreateBlueFlame(startPos, Quaternion.identity);

		int eye = GetEyeCorrespondingToSwitch(switchPanel);

		TweenPosition tween = TweenPosition.Begin(blueGO, 1.0f, eyePositions[eye].position);

		if (eye == 0)
		{
			tween.onFinished.Add(new EventDelegate(this, "ReachedLeftEye"));
		}
		else
		{
			tween.onFinished.Add(new EventDelegate(this, "ReachedRightEye"));
		}

		flames[eye].Add(blueGO);
	}

	public void SwitchOff(SwitchPanel switchPanel)
	{
		// Dissipate all the blue flames for this switch/eye.

		int eye = GetEyeCorrespondingToSwitch(switchPanel);

		foreach (GameObject go in flames[eye])
		{
			go.GetComponent<GroundSkullEye>().FadeOutAndDie();
		}
		flames[eye].Clear();
		eyesLit[eye] = false;
	}

	int GetEyeCorrespondingToSwitch(SwitchPanel switchPanel)
	{
		return switchPanel == observedSwitches[0] ? 0 : 1;
	}

	public void ReachedLeftEye()
	{
		if (flames[0].Count > 0)
		{
			eyesLit[0] = true;
		}
	}

	public void ReachedRightEye()
	{
		if (flames[1].Count > 0)
		{
			eyesLit[1] = true;
		}
	}
}
