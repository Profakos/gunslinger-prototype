using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public float speed = 5f;

	public Direction facing = Direction.South;

	public Sprite northSprite;
	public Sprite eastSprite;
	public Sprite southSprite;
	public Sprite westSprite;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;

	private Vector2 inputVector = Vector2.zero;
	private bool movementInProgress = false;


	private Dictionary<string, string> gameState = new Dictionary<string, string>();

	// Start is called before the first frame update
	void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
		UpdateFacing();

		if(!movementInProgress)
		{ 
			Vector2 checkForColliders = GetFacedTilePosition();
			
			//Check if there is anything blocking movement on the tile we are moving towards

			RaycastHit2D hit = Physics2D.Raycast(checkForColliders, Vector2.zero);

			if(hit.collider != null)
			{
				return;
			}
			
			movementInProgress = true;
			StartCoroutine(MoveByTile(inputVector));
		}
	}
	
	public void SetInputMovement(Vector2 vector)
	{
		inputVector = vector;
	}
	  
	//Check if there is an interactable on the tile we are looking at
	public void TryInteract()  
	{
		Vector2 facedTilePosition = GetFacedTilePosition();

		LayerMask mask = LayerMask.GetMask("Interaction");

		RaycastHit2D hit = Physics2D.Raycast(facedTilePosition, Vector2.zero, mask);

		if (hit.collider != null && hit.collider.tag == "Interactable")
		{
			//Debug.Log(hit.transform.name);

			Interaction interaction = hit.transform.gameObject.GetComponent<Interaction>();

			if (interaction == null) return;

			interaction.Interact();
		}
			
	}

	private IEnumerator MoveByTile(Vector2 direction)
	{ 
		Vector2 moveInDirection = direction * speed;

		Vector2 targetPosition = (Vector2)transform.position + direction;

		moveInDirection *= Time.fixedDeltaTime;
		  
		while((Vector2)transform.position != targetPosition)
		{
			rigidBody.MovePosition((Vector2)transform.position + moveInDirection);
			yield return null;
		}

		movementInProgress = false;

		yield return null;
	}

	private Vector2 GetFacedTilePosition()
	{
		Vector2 facedTilePosition = transform.position;

		facedTilePosition.y += 0.5f; //tile center
		
		switch (facing)
		{
			case Direction.North:
				facedTilePosition.y += 1f;
				break;
			case Direction.East:
				facedTilePosition.x += 1f;
				break;
			case Direction.South:
				facedTilePosition.y -= 1f;
				break;
			case Direction.West:
				facedTilePosition.x -= 1f;
				break;
		}
		
		return facedTilePosition;
	}

	private void UpdateFacing()
	{
		float moveHorizontal = inputVector.x;
		float moveVertical = inputVector.y;

		if (moveHorizontal != 0)
		{
			if (moveHorizontal > 0)
			{
				facing = Direction.East;
				spriteRenderer.sprite = eastSprite; 
			}
			else
			{ 
				facing = Direction.West;
				spriteRenderer.sprite = westSprite;
			}
		}
		if (moveVertical != 0)
		{
			if (moveVertical > 0)
			{
				facing = Direction.North;
				spriteRenderer.sprite = northSprite;
			}
			else
			{
				facing = Direction.South;
				spriteRenderer.sprite = southSprite;
			}
		}
	}

 


}
