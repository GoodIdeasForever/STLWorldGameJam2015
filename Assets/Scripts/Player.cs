using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float tileMovementDuration = 1.0f;

	public int gridX { get; private set; }
	public int gridY { get; private set; }
	public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }
	public bool HasItem { get; private set; }
	public Direction FacingDirection { get; private set; }

	bool currentlyMoving;
	Direction currentMotionDirection;
	float movementStartTime;

	const float controllerInputDeadzone = 0.3f;
	
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
		if (!GameState.Instance.CanIMoveHere(gridX + GetDirectionXMotion(movementDirection),
		                                     gridY + GetDirectionXMotion(movementDirection)))
		{
			return;
		}

		currentlyMoving = true;
		currentMotionDirection = movementDirection;
		movementStartTime = Time.time;
	}

	int GetDirectionXMotion(Direction direction)
	{
		switch (direction)
		{
		case Direction.East:
			return 1;
		case Direction.North:
			return 0;
		case Direction.South:
			return 0;
		case Direction.West:
			return -1;
		}
		return 0;
	}

	int GetDirectionYMotion(Direction direction)
	{
		switch (direction)
		{
		case Direction.East:
			return 0;
		case Direction.North:
			return 1;
		case Direction.South:
			return -1;
		case Direction.West:
			return 0;
		}
		return 0;
	}

	Vector2 GetWorldSpaceMotion(Direction direction)
	{
		switch (direction)
		{
		case Direction.East:
			return Vector2.right;
		case Direction.North:
			return Vector2.up;
		case Direction.South:
			return -Vector2.up;
		case Direction.West:
			return -Vector2.right;
		}
		return Vector2.one;
	}

	void AnimateMove()
	{
		if (currentlyMoving)
		{
			var t = (Time.time - movementStartTime) / tileMovementDuration;
			if (t > 1)
			{
				currentlyMoving = false;
				gridX += GetDirectionXMotion(currentMotionDirection);
				gridY += GetDirectionYMotion(currentMotionDirection);
				transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
			}
			else
			{
				transform.position = Vector3.Lerp(BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition), BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition + GetWorldSpaceMotion(currentMotionDirection)), t);
			}
		}
	}
}
