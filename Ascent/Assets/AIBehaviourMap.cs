using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AIBehaviourMap
{
    [HideInInspector]
    [SerializeField]
    private List<int> keysList = new List<int>();
    public List<int> KeysList
    {
        get { return keysList; }
        set { keysList = value; }
    }

    [SerializeField]
    private List<AIBehaviour> valuesList = new List<AIBehaviour>();
    public List<AIBehaviour> ValuesList
    {
        get { return valuesList; }
        set { valuesList = value; }
    }

    public void Add(int key, AIBehaviour data)
    {
        if (!ContainsKey(key))
        {
            keysList.Add(key);
            valuesList.Add(data);
        }
        else
        {
            setMap(key, data);
        }
    }

    public void Remove(int key)
    {
        valuesList.Remove(getMap(key));
        keysList.Remove(key);
    }

    public void setMap(int key, AIBehaviour data)
    {
        int keyIndex = 0;
        for (int i = 0; i < keysList.Count; i++)
        {
            if (keysList[i].Equals(key))
            {
                keyIndex = i;
            }
        }

        valuesList[keyIndex] = data;
    }

    public int getKey(int key)
    {
        for (int i = 0; i < keysList.Count; i++)
        {
            if (keysList[i].Equals(key))
                return keysList[i];
        }
        return default(int);

    }

    public void Clear()
    {
        if (keysList.Count > 0)
            keysList.Clear();
        if (valuesList.Count > 0)
            valuesList.Clear();
    }

    public bool ContainsKey(int key)
    {
        return convertToDictionary().ContainsKey(key);
    }

    public bool ContainsValue(AIBehaviour data)
    {
        return convertToDictionary().ContainsValue(data);
    }

    public AIBehaviour getMap(int key)
    {
        int keyIndex = 0;
        for (int i = 0; i < keysList.Count; i++)
        {
            if (keysList[i].Equals(key))
                keyIndex = i;
        }

        return valuesList[keyIndex];
    }

    public Dictionary<int, AIBehaviour> convertToDictionary()
    {
        Dictionary<int, AIBehaviour> dictionaryData = new Dictionary<int, AIBehaviour>();

        try
        {
            for (int i = 0; i < keysList.Count; i++)
            {
                dictionaryData.Add(keysList[i], valuesList[i]);
            }
        }
        catch (Exception)
        {
            Debug.LogError("KeysList.Count is not equal to ValuesList.Count. It shouldn't happen!");
        }

        return dictionaryData;
    }

    public void convertFromDictionary(Dictionary<int, AIBehaviour> dict)
    {
       
    }
}