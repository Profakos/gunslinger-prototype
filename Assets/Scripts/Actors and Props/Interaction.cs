using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{ 
	public GameEvent interactEvent;
	public SaveData saveData;
	 
	public string defaultReaction;
	public List<ConditionalReactionId> conditionalReactionIdList = new List<ConditionalReactionId>();
	  
	public void Interact()
	{
		if (interactEvent == null) return;

		string interactionId = FindReactionId();

		if (string.IsNullOrWhiteSpace(interactionId)) return;

		interactEvent.Raise(interactionId);
	}

	public string FindReactionId()
	{
		foreach(var conditionalReaction in conditionalReactionIdList)
		{
			if (conditionalReaction.ConditionSatisfied(saveData)) return conditionalReaction.GetReactionId();
		}
		 
		return defaultReaction;
	}
}
  
[System.Serializable]
public class ConditionalReactionId
{  
	public List<Condition> conditionList = new List<Condition>();

	public string reactionId;

	public bool ConditionSatisfied(SaveData saveData)
	{
		string returnValue = string.Empty;

		foreach (var condition in conditionList)
		{

			if (!saveData.Load(condition.name, ref returnValue))
			{
				return false;
			}

			if (returnValue.Equals(condition.value)) return true;
		}


		return false;
	}
	 
	public string GetReactionId()
	{
		return reactionId;
	}

	[System.Serializable]
	public class Condition
	{
		public string name;
		public string value;
	}

}
