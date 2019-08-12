using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//A cutscene line may display a line of text with a portrait, sets the world state, or asks for a multiple choice
[Serializable]
public class CutsceneLine 
{
	public string text;

	public Sprite portrait;

	public string name;

	public List<WorldStateUpdate> worldStateUpdate = new List<WorldStateUpdate>();

	public List<Choice> multipleChoice = new List<Choice>();

	[Serializable]
	public class WorldStateUpdate
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
