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
	
	void Awake () 
	{
		defaultWidth = barFront.width;
	}

	void OnEnable()
	{
		StartDrawing();
	}

	void OnDisable()
	{
		StopDrawing();
	}
	
	public void Init(eStat stat, Character _character)
	{
		owner = _character;

		TrackStat = stat;

		switch (stat)
		{
			case eStat.HP:
			{
				//maxVal = owner.stats.MaxHealth;
				//curVal = owner.stats.CurrentHealth;
				//owner.stats.onMaxHealthChanged += OnMaxValueChanged;
				//owner.stats.onCurHealthChanged += OnCurValueChanged;
				barFront.color = Color.red;

				maxVal = owner.Stats.MaxHealth;
				curVal = owner.Stats.CurrentHealth;
				owner.Stats.onMaxHealthChanged += OnMaxValueChanged;
				owner.Stats.onCurHealthChanged += OnCurValueChanged;
			}
			break;
			case eStat.SP:
			{
				//maxVal = owner.stats.MaxSpecial;
				//curVal = owner.stats.CurrentSpecial;
				//owner.stats.onMaxSpecialChanged += OnMaxValueChanged;
				//owner.stats.onCurSpecialChanged += OnCurValueChanged;
				barFront.color = Color.blue;

				maxVal = owner.Stats.MaxSpecial;
				curVal = owner.Stats.CurrentSpecial;
				owner.Stats.onMaxSpecialChanged += OnMaxValueChanged;
				owner.Stats.onCurSpecialChanged += OnCurValueChanged;
			}
			break;
			case eStat.EXP:
			{
				maxVal = 100f;	// we assume that EXP caps at 100
				curVal = ((Hero)owner).HeroStats.Experience;
				owner.Stats.onExpChanged += OnCurValueChanged;
				barFront.color = Color.yellow;
			}
			break;
			default:
				Debug.LogError("StatBar : Critical Error");
			break;
		}
		
		AdjustBar();
	}

    public void OnDestroy()
    {
        Shutdown();
    }

	public void Shutdown()
	{
		switch (TrackStat)
		{
		case eStat.HP:
			owner.Stats.onMaxHealthChanged -= OnMaxValueChanged;
			owner.Stats.onCurHealthChanged -= OnCurValueChanged;
			break;
		case eStat.SP:
			owner.Stats.onMaxSpecialChanged -= OnMaxValueChanged;
			owner.Stats.onCurSpecialChanged -= OnCurValueChanged;
			break;
		case eStat.EXP:
			owner.Stats.onExpChanged -= OnCurValueChanged;
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
