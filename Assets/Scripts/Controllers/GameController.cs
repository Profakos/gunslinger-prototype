using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	Stack<GameState> gameStateStack = new Stack<GameState>();

	CutsceneController cutsceneController;
	WalkaroundController walkaroundController;

	public SaveData saveData;

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
		saveData.Reset();

		saveData.Save("Test", "1");
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	/*
	 * Swaps to cutscene mode, if the cutscene with the specified name exists
	 */
	void SwapToCutscene(string message)
	{  
		SwapGameState(GameState.Cutscene);

		gameStateStack.Push(GameState.Cutscene);
		
		if (!cutsceneController.TryStartCutscene(message))
		{
			SwapToPrevious();
		};
	}

	/*
	 * Moves back to the previous view
	 */
	void SwapToPrevious()
	{
		gameStateStack.Pop(); 
		SwapGameState(gameStateStack.Peek());

	}

	/*
	 * Sleeps the other gamestates, and awakens the selected one
	 */
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

	/*
	 * Updates the state of the world (doors unlocked, things picked up, etc)
	 */
	void UpdateSaveData(CutsceneLine.SaveDataUpdate data)
	{
		saveData.Save(data.name, data.value);
	}
}
