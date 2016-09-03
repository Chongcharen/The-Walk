using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Linq;
public static class JsonHelper
{

	public static T FromSingleJson<T>(string json)
	{
		T wrapper = JsonMapper.ToObject<T>(json);
		return wrapper;
	}
	public static T[] FromJson<T>(string json)
	{
		string newJson = "{\"Items\":" + json + "}";
		Wrapper<T> wrapper = JsonMapper.ToObject<Wrapper<T>>(newJson);
		return wrapper.Items;
	}
	public static List<T> FromJsonList<T>(string json){

		JsonMapper.RegisterImporter<int, long>((int value) =>
			{ 
				return (long)value;
			});
		string newJson = "{\"Items\":" + json + "}";
		Wrapper<T> wrapper = JsonMapper.ToObject<Wrapper<T>>(newJson);
		List<T> lst = new List<T> ();

		for(int i = 0; i < wrapper.Items.Length; i++)
		{
			lst.Add (wrapper.Items[i]);
		}
		return lst;
	}

	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return UnityEngine.JsonUtility.ToJson(wrapper);
	}



	[Serializable]
	private class Wrapper<T>
	{
		public T[] Items;
	}
}