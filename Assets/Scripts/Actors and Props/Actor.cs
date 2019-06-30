using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	public float speed = 5f;

	public Direction facing = Direction.South;
	 
	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;
	private SpriteRenderer spriteRenderer;
	 
	private Vector2 inputVector = Vector2.zero;
	private bool movementInProgress = false;

	private Animator animator;


	private Dictionary<string, string> gameState = new Dictionary<string, string>();

	void Awake()
	{
		animator = gameObject.GetComponent<Animator>();
	}

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
		if(!movementInProgress)
		{  
			if(inputVector == Vector2.zero)
			{
				animator.SetBool("Walking", false);
				animator.SetInteger("Direction", (int)facing);
				return;
			}
			 
			animator.SetInteger("Direction", (int)facing);

			Vector2 checkForColliders = GetFacedTilePosition();
			
			//Check if there is anything blocking movement on the tile we are moving towards

			RaycastHit2D hit = Physics2D.Raycast(checkForColliders, Vector2.zero);

			if(hit.collider != null)
			{  
				animator.SetBool("Walking", false);
				return;
			}
			
			movementInProgress = true;
			 
			animator.SetBool("Walking", true);
			 
			StartCoroutine(MoveByTile(inputVector));
		}
	}
	
	public void UpdateInputMovement(Direction dir)
	{ 
		if (dir == facing && inputVector != Vector2.zero) return;

		switch (dir)
		{
			case Direction.None:
				inputVector = new Vector2(0, 0);
				break; 
			case Direction.North:
				inputVector = new Vector2(0, 1);
				facing = Direction.North;
				break;
			case Direction.East:
				inputVector = new Vector2(1, 0);
				facing = Direction.East;
				break;
			case Direction.South:
				inputVector = new Vector2(0, -1);
				facing = Direction.South;
				break;
			case Direction.West:
				inputVector = new Vector2(-1, 0);
				facing = Direction.West;
				break; 
		} 
	}

	/*
	 * Check if there is an interactable on the tile we are looking at
	 */
	public void TryInteract()  
	{
		if (movementInProgress) return;

		Vector2 facedTilePosition = GetFacedTilePosition();

		LayerMask mask = LayerMask.GetMask("Interaction");

		RaycastHit2D hit = Physics2D.Raycast(facedTilePosition, Vector2.zero, mask);

		if (hit.collider != null && hit.collider.tag == "Interactable")
		{
			Interaction interaction = hit.transform.gameObject.GetComponent<Interaction>();

			if (interaction == null) return;

			interaction.Interact();
		}
			
	}

	/*
	 *Moves one discrete tile
	 */
	private IEnumerator MoveByTile(Vector2 direction)
	{  
		Vector2 moveInDirection = direction * speed;

		Vector2 targetPosition = (Vector2)transform.position + direction;

		moveInDirection *= Time.fixedDeltaTime;
		  
		while ((Vector2)transform.position != targetPosition)
		{
			rigidBody.MovePosition((Vector2)transform.position + moveInDirection);
			yield return null;
		}
		  
		movementInProgress = false;
		 
		yield return null;
	}

	/*
	 * Finds out the coordinate of the tile faced by the actor
	 */
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
	 

}
