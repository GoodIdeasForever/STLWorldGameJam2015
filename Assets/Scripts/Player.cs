using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float tileMovementDuration = 1.0f;
	public AudioSource player;

	public int gridX { get; private set; }
	public int gridY { get; private set; }
	public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }
	public bool HasItem { get; private set; }
	public Direction FacingDirection { get; private set; }

	bool currentlyMoving;
	Direction currentMotionDirection;
	float movementStartTime;

	const float controllerInputDeadzone = 0.15f;
	
	void Awake()
	{
		HasItem = false;
		FacingDirection = Direction.South;
		currentlyMoving = false;
	}

	public void SpawnAtGridPosition(int x, int y)
	{
		gridX = x;
		gridY = y;
		transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
		GameState.Instance.PlaceObjectOnBoard(Space.Player, gridX, gridY);
	}

	public void PickupItem()
	{
		HasItem = true;
	}

	public void DropItem()
	{
		HasItem = false;
	}
	
	void Update()
	{		
		if (!currentlyMoving)
		{
			// Measure input axes and assign desired movement direction
			if (Mathf.Abs(Input.GetAxis("Horizontal")) > controllerInputDeadzone
			    && Mathf.Abs(Input.GetAxis("Horizontal")) >= Mathf.Abs(Input.GetAxis("Vertical")))
			{
				Move(Input.GetAxis("Horizontal") > 0 ? Direction.East : Direction.West);
			}
			else if (Mathf.Abs(Input.GetAxis("Vertical")) > controllerInputDeadzone
			    && Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Abs(Input.GetAxis("Horizontal")))
			{
				Move(Input.GetAxis("Vertical") > 0 ? Direction.North : Direction.South);
			}
		}
	}

	void LateUpdate()
	{
		AnimateMove();
	}

	void Move(Direction movementDirection)
	{
		if (!GameState.Instance.CanIMoveHere(gridX + movementDirection.XMotion(),
		                                     gridY + movementDirection.YMotion()))
		{
			return;
		}

		currentlyMoving = true;
		currentMotionDirection = movementDirection;
		movementStartTime = Time.time;
		player.Play();
	}
	
	void AnimateMove()
	{
		if (currentlyMoving)
		{
			var t = (Time.time - movementStartTime) / tileMovementDuration;
			if (t > 1)
			{
				var oldGridX = gridX;
				var oldGridY = gridY;
				currentlyMoving = false;
				gridX += currentMotionDirection.XMotion();
				gridY += currentMotionDirection.YMotion();
				transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
				GameState.Instance.MoveCharacter(Space.Player, oldGridX, oldGridY, gridX, gridY);
			}
			else
			{
				transform.position = Vector3.Lerp(BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition), BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition + currentMotionDirection.WorldSpaceMotion()), t);
			}
		}
	}
}
