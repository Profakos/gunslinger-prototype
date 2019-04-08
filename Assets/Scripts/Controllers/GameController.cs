using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	Stack<GameState> gameStateStack = new Stack<GameState>();

	CutsceneController cutsceneController;
	WalkaroundController walkaroundController;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SwapToCutscene(string message)
	{  
		SwapGameState(GameState.Cutscene);

		gameStateStack.Push(GameState.Cutscene);
		
		if (!cutsceneController.TryStartCutscene(message))
		{
			SwapToPrevious();
		};
	}

	void SwapToPrevious()
	{
		gameStateStack.Pop(); 
		SwapGameState(gameStateStack.Peek());

	}

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
}
