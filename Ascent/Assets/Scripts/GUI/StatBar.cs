using UnityEngine;
using System.Collections;

/// <summary>
/// Bar for tracking a stat that has a max and current value
/// </summary>
public class StatBar : MonoBehaviour 
{
	public UISprite barBack;
	public UISprite barFront;
	public Character owner;
	protected float defaultWidth;
	
	private float maxVal;
	private float curVal;

	public HealthBlock[] healthBlocks;
	public SpecialBlock[] specialBlocks;
	public SpecialGroup[] specialGroups;

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
		if(barFront != null)
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
	
	public virtual void Init(eStat stat, Character _character)
	{
		owner = _character;

		TrackStat = stat;

		switch (stat)
		{
			case eStat.HP:
			{
				if (owner is  Enemy)
				{
					//barFront.color = Color.red;

					GameObject background = transform.FindChild("Background").gameObject;
					background.SetActive(false);
					
					GameObject healthBlock = Resources.Load("Prefabs/UI/HealthBlock") as GameObject;

					healthBlocks = new HealthBlock[owner.Stats.MaxHealth];

					UIWidget statbarWidget = GetComponent<UIWidget>();

					int barWidth = statbarWidget.width;

					if (owner is Abomination || owner is WatcherBoss)
					{
						statbarWidget.width *= 3;
						statbarWidget.height *= 2;
						barWidth = statbarWidget.width;
					}

					int blockWidth = barWidth / healthBlocks.Length;
					int depth = statbarWidget.depth;

					for(int i = 0; i < healthBlocks.Length; ++i)
					{
						healthBlocks[i] = NGUITools.AddChild(gameObject, healthBlock).GetComponent<HealthBlock>();
						healthBlocks[i].transform.localScale = transform.localScale;
						healthBlocks[i].GetComponent<UIWidget>().height = statbarWidget.height;
						healthBlocks[i].GetComponent<UIWidget>().width = (int)blockWidth;
						healthBlocks[i].GetComponent<UIWidget>().depth = depth++;
						healthBlocks[i].name = i + "block";
					}
					GetComponent<UIGrid>().cellWidth = blockWidth;
					GetComponent<UIGrid>().Reposition();

					statbarWidget.depth = ++depth;
					statbarWidget.enabled = false;

					if (!(owner is Abomination || owner is WatcherBoss))
					{
						statbarWidget.height += 4;
					}

					//background.SetActive(true);
				}

				maxVal = owner.Stats.MaxHealth;
				curVal = owner.Stats.CurrentHealth;

				owner.Stats.onMaxHealthChanged += OnMaxValueChanged;
				owner.Stats.onCurHealthChanged += OnCurValueChanged;
			}
			break;
			case eStat.SP:
			{
				//if (owner is Enemy)
				//    barFront.color = Color.blue;

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

	public void Update()
	{
		if (owner is Enemy)
		{
			if (!owner.gameObject.activeInHierarchy)
			{
				gameObject.SetActive(false);
			}
		}
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

			if(TrackStat == eStat.HP)
			{
				for (int i = 0; i < owner.Stats.MaxHealth; ++i )
				{
					if (i < healthBlocks.Length)
						healthBlocks[i].gameObject.SetActive(i < owner.Stats.CurrentHealth);
				}
			}

		if (owner is Hero)
		{
			if(TrackStat == eStat.SP)
			{
				for (int i = 0; i < owner.Stats.MaxSpecial; ++i)
				{
					if (i < specialBlocks.Length)
						specialBlocks[i].gameObject.SetActive(i < owner.Stats.CurrentSpecial);
				}
			}
		}

		if(owner is Enemy)
		{
			int result = (int)(defaultWidth / (maxVal / curVal));
			//barFront.width = result;
			if (result <= 0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
	}	

	protected void StopDrawing()
	{
		if (owner is Enemy)
		{
			//barBack.enabled = false;
			//barFront.enabled = false;
			gameObject.SetActive(false);
		}
	}	

	protected void StartDrawing()
	{
		if (owner is Enemy)
		{
			//barBack.enabled = true;
			//barFront.enabled = true;
			gameObject.SetActive(true);
		}
	}
}
