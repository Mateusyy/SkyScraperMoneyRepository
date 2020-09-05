using System;
using System.Collections.Generic;
using UnityEngine;

public class JSONSerializer
{
    public static T FromJson<T>(string json) where T : class
    {
        if (typeof(T) == typeof(Dictionary<string, string>))
        {
            return FromJsonStringStringDictionary(json) as T;
        }

        return default(T);
    }

    private static Dictionary<string, string> FromJsonStringStringDictionary(string json)
    {
        StringStringDictionaryArray loadedData = JsonUtility.FromJson<StringStringDictionaryArray>(json);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            dictionary.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        return dictionary;
    }

    [System.Serializable]
    private class StringStringDictionaryArray
    {
        public StringStringDictionary[] items;
    }

    [System.Serializable]
    private class StringStringDictionary
    {
        public string key;
        public string value;
    }
}