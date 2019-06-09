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
	}

	void LateUpdate()
	{ 
	}

	void FixedUpdate()
	{
		HandleMovement();
	}

	//Handles movement based on player key input
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

	private void HandleMovement()
	{
		if (pressedDirections.Count == 0)
		{
			actor.SetInputMovement(new Vector2(0, 0));
		}
		else
		{
			switch (pressedDirections[0])
			{
				case Direction.North:
					actor.SetInputMovement(new Vector2(0, 1));
					break;
				case Direction.East:
					actor.SetInputMovement(new Vector2(1, 0));
					break;
				case Direction.South:
					actor.SetInputMovement(new Vector2(0, -1));
					break;
				case Direction.West:
					actor.SetInputMovement(new Vector2(-1, 0));
					break;
			}
		}
	}

	private void CancelMove()
	{
		actor.SetInputMovement(Vector2.zero);
	}
	  
	public void StartInteraction(string message)
	{
		pressedDirections.Clear();

		CancelMove();

		gameObject.SendMessage("SwapToCutscene", message);
	}
 
	 
}
