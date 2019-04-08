using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{ 
	public GameEvent gameEvent; 
	public UnityEvent response; 
	public StringEvent stringResponse;

	[System.Serializable]
	public class StringEvent : UnityEvent<string> { }
	   
	private void OnEnable()
	{
		gameEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		gameEvent.UnregisterListener(this);
	}

	public void OnEventRaised()
	{
		if (response == null) return;
		response.Invoke();
	}

	public void OnEventRaised(string message)
	{ 
		if (stringResponse == null) return;
		stringResponse.Invoke(message); 
	}

}
