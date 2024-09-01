using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomJsonItem : MonoBehaviour
{
    public static List<T> FromJSON<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);

        return wrapper.items;
    }

    public static string ToJSON<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = list;

        return JsonUtility.ToJson(wrapper.items);
    }

    [Serializable]
    public class Wrapper<T>
    {
        public List<T> items;
    }
}
