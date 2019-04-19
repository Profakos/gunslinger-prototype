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

	public List<Choice> multipleChoice = new List<Choice>();

	[Serializable]
	public class SaveDataUpdate
	{
		public string name;
		public string value;
	}

	[Serializable]
	public class Choice
	{
		public string text;
		public string cutsceneId;
	}
}
