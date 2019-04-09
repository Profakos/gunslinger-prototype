using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CutsceneLine 
{
	public string text;

	public Sprite portrait;

	public string name;

	public List<SaveDataUpdate> saveDataUpdate = new List<SaveDataUpdate>();

	[Serializable]
	public class SaveDataUpdate
	{
		public string name;
		public string value;
	}
}
