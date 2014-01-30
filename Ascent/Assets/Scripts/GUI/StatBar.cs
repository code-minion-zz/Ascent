using UnityEngine;
using System.Collections;

/// <summary>
/// Bar for tracking a stat that has a max and current value
/// </summary>
public class StatBar : MonoBehaviour {
	
	public UISprite barBack;
	public UISprite barFront;
	public Character owner;
	private float defaultWidth;
	
	private float maxVal;
	private float curVal;

	public enum eStat
	{
		Invalid = -1,
		HP,
		SP,
		EXP		
	};
	public eStat TrackStat = eStat.Invalid;

	protected bool enableDraw = true;
	public bool EnableDraw
	{
		get { return enableDraw;}
		set 
		{
			enableDraw = value;
			if (value)
			{
				StartDrawing();
			}
			else
			{
				StopDrawing();
			}
		}
	}
	
	// Use this for initialization
	void Awake () {
		defaultWidth = barFront.width;
	}
	
	public void Init(eStat stat, Character _character)
	{
		owner = _character;

		TrackStat = stat;

		switch (stat)
		{
			case eStat.HP:
			{
				maxVal = owner.DerivedStats.MaxHealth;
				curVal = owner.DerivedStats.CurrentHealth;
				owner.DerivedStats.onMaxHealthChanged += OnMaxValueChanged;
				owner.DerivedStats.onCurHealthChanged += OnCurValueChanged;
				barFront.color = Color.red;
			}
			break;
			case eStat.SP:
			{
				maxVal = owner.DerivedStats.MaxSpecial;
				curVal = owner.DerivedStats.CurrentSpecial;
				owner.DerivedStats.onMaxSpecialChanged += OnMaxValueChanged;
				owner.DerivedStats.onCurSpecialChanged += OnCurValueChanged;
				barFront.color = Color.blue;
			}
			break;
			case eStat.EXP:
			{
				maxVal = 100f;	// we assume that EXP caps at 100
				curVal = owner.CharacterStats.CurrentExperience;
				owner.CharacterStats.onExpChanged += OnCurValueChanged;
				barFront.color = Color.yellow;
			}
			break;
			default:
				Debug.LogError("StatBar : Critical Error");
			break;
		}
		
		AdjustBar();
	}

	public void Shutdown()
	{
		switch (TrackStat)
		{
		case eStat.HP:
			owner.DerivedStats.onMaxHealthChanged -= OnMaxValueChanged;
			owner.DerivedStats.onCurHealthChanged -= OnCurValueChanged;
			break;
		case eStat.SP:
			owner.DerivedStats.onMaxSpecialChanged -= OnMaxValueChanged;
			owner.DerivedStats.onCurSpecialChanged -= OnCurValueChanged;
			break;
		case eStat.EXP:
			owner.CharacterStats.onExpChanged -= OnCurValueChanged;
			break;
		}
		gameObject.SetActive(false);
	}
	
	void OnCurValueChanged(float value)
	{
		curVal = value;
		AdjustBar();
	}
	
	void OnMaxValueChanged(float value)
	{
		maxVal = value;
		AdjustBar();
	}
	
	void AdjustBar()
	{
        int result = (int)(defaultWidth / (maxVal / curVal));
        barFront.width = result;
        if (result <= 0)
        {
            barFront.gameObject.SetActive(false);
        }
        else
        {
            barFront.gameObject.SetActive(true);
        }
	}	

	protected void StopDrawing()
	{
		barBack.enabled = false;
		barFront.enabled = false;
	}	

	protected void StartDrawing()
	{
		barBack.enabled = true;
		barFront.enabled = true;
	}
}
