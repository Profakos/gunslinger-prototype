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
			Vector2 lineOrigin = (Vector2)gameObject.transform.position + boxCollider.offset;
			
			RaycastHit2D hit = Physics2D.Raycast(lineOrigin, inputVector, 1.0f);

			if(hit.collider != null)
			{
				return;
			}
			
			movementInProgress = true;
			StartCoroutine(MoveByTile(inputVector));
		}
	}

	void FixedUpdate()
	{ 
		//MoveInDirection(inputVector);
	}
	 
	public void SetInputMovement(Vector2 vector)
	{
		inputVector = vector;
	}
	  
	//TODO: 
	public void TryInteract() //casts a ray from the edge of a colliders. 
	{
		Vector2 origin = (Vector2)gameObject.transform.position + boxCollider.offset; //center of a unit length tile
		Vector2 colliderSize = boxCollider.size;

		Vector2 direction = Vector2.zero;

		float rayLength = 1f; // Interaction Distance
		  
		switch (facing)
		{
			case Direction.North:
				direction.y = 1f;
				origin.y += colliderSize.y * 0.4f;
				break;
			case Direction.East:
				direction.x = 1f;
				origin.x += colliderSize.x * 0.4f;
				break;
			case Direction.South:
				direction.y = -1f;
				origin.y += colliderSize.y * -0.4f;
				break;
			case Direction.West:
				direction.x = -1f;
				origin.x += colliderSize.x * -0.4f;
				break;
		}
		 
		LayerMask mask = LayerMask.GetMask("Interaction");

		RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, mask);

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
