using UnityEngine;
using System.Collections;

public class AICondition_SP : AICondition
{
	private DerivedStats stats;
	private EType type;
	private ESign sign;
	private float value;
	private bool hasMet = false;

	public AICondition_SP(DerivedStats stats, EType type, ESign sign, float value)
	{
		this.stats = stats;
		this.type = type;
		this.sign = sign;
		this.value = value;
	}

	public override bool HasBeenMet()
	{
		float curValue = stats.CurrentHealth;

		if (type == EType.Percentage)
		{
			curValue = curValue / stats.MaxHealth;
		}

		if (Evaluate(curValue, value, sign))
		{
			hasMet = true;
			return true;
		}

		hasMet = false;
		return false;
	}

	protected bool Evaluate(float a, float b, ESign sign)
	{
		switch (sign)
		{
			case ESign.Equal: return a == b;
			case ESign.GreaterThan: return a > b;
			case ESign.EqualOrGreater: return a >= b;
			case ESign.LessThan: return a < b;
			case ESign.EqualOrLess: return a <= b;
		}

		return false;
	}

	protected string GetString(ESign sign)
	{
		string strSign = "";

		switch (sign)
		{
			case ESign.Equal: return "==";
			case ESign.GreaterThan: return ">";
			case ESign.EqualOrGreater: return ">=";
			case ESign.LessThan: return "<";
			case ESign.EqualOrLess: return "";
		}

		return strSign;
	}

	public override string ToString()
	{
		string substr = "";
		if (type == EType.Percentage)
		{
			substr = (value * 100.0f) + "%";
		}
		else
		{
			substr = value.ToString();
		}

		return "SP " + GetString(sign) + " " + substr + ": " + hasMet;
	}
}
