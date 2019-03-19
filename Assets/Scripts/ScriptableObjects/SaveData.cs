﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
	[Serializable]
	public class KeyValuePairLists<T>
	{
		public List<string> keys = new List<string>();
		public List<T> values = new List<T>();
		public void Clear()
		{
			keys.Clear();
			values.Clear();
		}
		public void TrySetValue(string key, T value)
		{
			int index = keys.FindIndex(x => x == key);
			if (index > -1)
			{
				values[index] = value;
			}
			else
			{
				keys.Add(key);
				values.Add(value);
			}
		}
		public bool TryGetValue(string key, ref T value)
		{
			int index = keys.FindIndex(x => x == key);
			if (index > -1)
			{
				value = values[index];
				return true;
			}
			return false;
		}
	}

	public KeyValuePairLists<bool> boolKeyValuePairLists = new KeyValuePairLists<bool>();
	public KeyValuePairLists<int> intKeyValuePairLists = new KeyValuePairLists<int>();
	public KeyValuePairLists<string> stringKeyValuePairLists = new KeyValuePairLists<string>();
	public KeyValuePairLists<Vector2> vector2KeyValuePairLists = new KeyValuePairLists<Vector2>();

	public void Reset()
	{
		boolKeyValuePairLists.Clear();
		intKeyValuePairLists.Clear();
		stringKeyValuePairLists.Clear();
		vector2KeyValuePairLists.Clear(); 
	}
	private void Save<T>(KeyValuePairLists<T> lists, string key, T value)
	{
		lists.TrySetValue(key, value);
	}
	private bool Load<T>(KeyValuePairLists<T> lists, string key, ref T value)
	{
		return lists.TryGetValue(key, ref value);
	}
	public void Save(string key, bool value)
	{
		Save(boolKeyValuePairLists, key, value);
	}
	public void Save(string key, int value)
	{
		Save(intKeyValuePairLists, key, value);
	}
	public void Save(string key, string value)
	{
		Save(stringKeyValuePairLists, key, value);
	}
	public void Save(string key, Vector2 value)
	{
		Save(vector2KeyValuePairLists, key, value);
	}
	public bool Load(string key, ref bool value)
	{
		return Load(boolKeyValuePairLists, key, ref value);
	}
	public bool Load(string key, ref int value)
	{
		return Load(intKeyValuePairLists, key, ref value);
	}
	public bool Load(string key, ref string value)
	{
		return Load(stringKeyValuePairLists, key, ref value);
	}
	public bool Load(string key, ref Vector2 value)
	{
		return Load(vector2KeyValuePairLists, key, ref value);
	}
}