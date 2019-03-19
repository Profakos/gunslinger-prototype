﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkaroundController : MonoBehaviour
{ 
	public Actor actor;
	public SaveData saveData;

	// Start is called before the first frame update
	void Start()
    {
		saveData.Reset();

		saveData.Save("test", "2");
	}

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			actor.TryInteract();
		}  
	}

	void LateUpdate()
	{ 
	}

	void FixedUpdate()
	{
		HandleMove(); 
	}

	//Handles movement based on player key input
	private void HandleMove()
	{
		if(actor == null)
		{
			return;
		}

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector2 vector = new Vector2(moveHorizontal, moveVertical);

		actor.SetInputMovement(vector);
	}
	  
	public void StartInteraction(string message)
	{ 
		Debug.Log("Interaction: " + message);
	}
	 
}