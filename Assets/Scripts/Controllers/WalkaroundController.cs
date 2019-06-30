using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkaroundController : MonoBehaviour
{ 
	public Actor actor;

	public List<Direction> pressedDirections = new List<Direction>();
	  
	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
	{
		HandleMoveKeys();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			actor.TryInteract();
		}
		  
		HandleMovement();
	}

	void LateUpdate()
	{ 
	}

	void FixedUpdate()
	{
	}

	/*
	 * Handles movement based on player key input
	 */
	private void HandleMoveKeys()
	{
		if(actor == null)
		{
			return;
		}
		  
		if (Input.GetKeyDown(KeyCode.W) && !pressedDirections.Contains(Direction.North))
		{
			pressedDirections.Insert(0, Direction.North);
		}

		if (Input.GetKeyUp(KeyCode.W))
		{
			pressedDirections.Remove(Direction.North);
		} 
		 
		if (Input.GetKeyDown(KeyCode.D) && !pressedDirections.Contains(Direction.East))
		{
			pressedDirections.Insert(0, Direction.East);
		}

		if (Input.GetKeyUp(KeyCode.D))
		{
			pressedDirections.Remove(Direction.East);
		}

		if (Input.GetKeyDown(KeyCode.S) && !pressedDirections.Contains(Direction.South))
		{
			pressedDirections.Insert(0, Direction.South);
		}

		if (Input.GetKeyUp(KeyCode.S))
		{
			pressedDirections.Remove(Direction.South);
		}

		if (Input.GetKeyDown(KeyCode.A) && !pressedDirections.Contains(Direction.West))
		{
			pressedDirections.Insert(0, Direction.West);
		}

		if (Input.GetKeyUp(KeyCode.A))
		{
			pressedDirections.Remove(Direction.West);
		}  
	}

	/*
	 * Deletegates the desired movement direction to the player actor
	 */
	private void HandleMovement()
	{

		actor.UpdateInputMovement(pressedDirections.Count == 0 ? Direction.None : pressedDirections[0]); 
	}

	/*
	 * Clears the inputmovement vector of the player actor
	 */
	private void CancelMove()
	{
		actor.UpdateInputMovement(Direction.None);
	}
	  
	/*
	 * Stops all player movement inputs from triggering, and swaps to the cutscene controller
	 */
	public void StartInteraction(string message)
	{
		pressedDirections.Clear();

		CancelMove();

		gameObject.SendMessage("SwapToCutscene", message);
	}
 
	 
}
