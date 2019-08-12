using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The highest level of controller, which handles swapping between different gameplay loops, such as exploring, battling, and talking
public class GameController : MonoBehaviour
{
	Stack<GameState> gameStateStack = new Stack<GameState>();

	CutsceneController cutsceneController;
	WalkaroundController walkaroundController;

	public WorldState worldState;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		cutsceneController = gameObject.GetComponent<CutsceneController>();
		walkaroundController = gameObject.GetComponent<WalkaroundController>();

		GameState startingGameState = GameState.Walkaround;

		gameStateStack.Push(startingGameState);

		SwapGameState(startingGameState);
	}


	// Start is called before the first frame update
	void Start()
    {
		worldState.Reset();

		worldState.Save("Test", "1");
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// Swaps to cutscene mode, if the cutscene with the specified name exists
	void SwapToCutscene(string message)
	{  
		SwapGameState(GameState.Cutscene);

		gameStateStack.Push(GameState.Cutscene);
		
		//If failed, goes back to the previous game mode (battle or walkaround)
		if (!cutsceneController.TryStartCutscene(message))
		{
			SwapToPrevious();
		};
	}

	// Moves back to the previous view
	void SwapToPrevious()
	{
		gameStateStack.Pop(); 
		SwapGameState(gameStateStack.Peek());

	}

	//Disables the other gamestates, and awakens the selected one
	void SwapGameState(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Battle:
				cutsceneController.enabled = false;
				walkaroundController.enabled = false;
				break;
			case GameState.Cutscene:
				cutsceneController.enabled = true;
				walkaroundController.enabled = false;
				break;
			case GameState.Walkaround:
				cutsceneController.enabled = false;
				walkaroundController.enabled = true;
				break;
		}
	}

	// Updates the state of the world (doors unlocked, things picked up, etc)
	void UpdateWorldState(CutsceneLine.WorldStateUpdate data)
	{
		worldState.Save(data.name, data.value);
	}
}
