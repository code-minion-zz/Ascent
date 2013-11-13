using UnityEngine;
using System.Collections;

/// <summary>
/// Bar for tracking a stat that has a max and current value
/// </summary>
public class StatBar : MonoBehaviour {
	
	public UISprite barBack;
	public UISprite barFront;
	private CharacterStatistics ownerStat;
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
	
	float tick = 0f;
	
	// Use this for initialization
	void Start () {
		defaultWidth = barBack.width;
	}
	
	void Update()
	{
		// debugging the hp bar
//		tick += Time.deltaTime;
//		if (tick > 1f)
//		{
//			tick = 0;
//			ownerStat.CurrentHealth --;
//		}
	}
	
	public void Init(eStat stat, CharacterStatistics charStat)
	{
		ownerStat = charStat;
		switch (stat)
		{
			case eStat.HP:
			{
				maxVal = ownerStat.MaxHealth;
				curVal = ownerStat.CurrentHealth;
				ownerStat.onMaxHealthChanged += OnMaxValueChanged;
				ownerStat.onCurHealthChanged += OnCurValueChanged;
				barFront.color = Color.red;
			}
			break;
			case eStat.SP:
			{
				maxVal = ownerStat.MaxSpecial;
				curVal = ownerStat.CurrentSpecial;
				ownerStat.onMaxSpecialChanged += OnMaxValueChanged;
				ownerStat.onCurSpecialChanged += OnCurValueChanged;
				barFront.color = Color.blue;
			}
			break;
			case eStat.EXP:
			{
				maxVal = 100f;	// we assume that EXP caps at 100
				curVal = ownerStat.CurrentExperience;
				ownerStat.onExpChanged += OnCurValueChanged;
				barFront.color = Color.yellow;
			}
			break;
			default:
				Debug.LogError("StatBar : Critical Error");
			break;
		}
		
		Rescale();
	}
	
	void OnCurValueChanged(float value)
	{
		curVal = value;
		Rescale();
	}
	
	void OnMaxValueChanged(float value)
	{
		maxVal = value;
		Rescale();
	}
	
	void Rescale()
	{
		barFront.width = (int)(defaultWidth / (maxVal/curVal));
	}	
}
