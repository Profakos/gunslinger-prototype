using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{ 
	public GameEvent interactEvent;
	public WorldState worldState;
	 
	public string defaultReaction;
	public List<ConditionalReactionId> conditionalReactionIdList = new List<ConditionalReactionId>();
	  
	//Triggers the event based, and passes along the reaction Id, if any
	public void Interact()
	{
		if (interactEvent == null) return;

		string interactionId = FindReactionId();

		if (string.IsNullOrWhiteSpace(interactionId)) return;

		interactEvent.Raise(interactionId);
	}

	// Cycles through the reaction stored in the interactable object, and returns with the first one whose condition is satisfied
	// or with the default reaction, if none is found
	public string FindReactionId()
	{
		foreach(var conditionalReaction in conditionalReactionIdList)
		{
			if (conditionalReaction.ConditionSatisfied(worldState)) return conditionalReaction.GetReactionId();
		}
		 
		return defaultReaction;
	}
}
  
[System.Serializable]
public class ConditionalReactionId
{  
	//Return with this reaction Id only if the condition is satisfied
	public List<Condition> conditionList = new List<Condition>();

	public string reactionId;

	public bool ConditionSatisfied(WorldState worldState)
	{
		if(worldState == null)
		{
			Debug.Log("WorldState is null!");
			return false;
		}

		string returnValue = string.Empty;

		foreach (var condition in conditionList)
		{

			if (!worldState.Load(condition.name, ref returnValue))
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
