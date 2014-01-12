using UnityEngine;
using System.Collections;

public static class MonoExtensions
{
	public static T GetComponentSafe<T>(this GameObject child) where T : Component
	{
		T result = child.GetComponent<T>();
		if (result == null)
		{
			Debug.LogError("Could not find component.", child);
			return null;
		}
		return result;
	}

	public static T GetComponentSafe<T>(this Transform child) where T : Component
	{
		T result = child.GetComponent<T>();
		if (result == null)
		{
			Debug.LogError("Could not find component.", child);
			return null;
		}
		return result;
	}

	public static T GetOrAddComponent<T>(this GameObject child) where T : Component
	{
		T result = child.GetComponent<T>();
		if (result == null)
		{
			result = child.gameObject.AddComponent<T>();
		}
		return result;
	}

	public static T GetOrAddComponent<T>(this Transform child) where T : Component
	{
		T result = child.GetComponent<T>();
		if (result == null)
		{
			result = child.gameObject.AddComponent<T>();
		}
		return result;
	}

}