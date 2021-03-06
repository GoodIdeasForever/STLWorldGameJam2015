﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public float tileMovementDuration = 1.0f;
	public AudioSource player;
	public SpriteRenderer sprite;

	public Sprite[] upSprites;
	public Sprite[] downSprites;
	public Sprite[] rightSprites;
	public Sprite[] leftSprites;
	public float spriteFrameAdvanceTime;

	public int gridX { get; private set; }
	public int gridY { get; private set; }
	public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }
    public bool HasItem { get { return this.ItemsInBack.Count > 0; } }
    public List<Space> ItemsInBack { get; private set; }
	public Direction FacingDirection { get; private set; }

	bool currentlyMoving;
	Direction currentMotionDirection;
	float movementStartTime;
	float nextSpriteFrameTime;
	int currentSpriteFrameIndex;
	Direction nextMovementDirection;

	const float controllerInputDeadzone = 0.15f;

	bool soundStarted = false;
	
	void Awake()
	{
		FacingDirection = Direction.South;
		currentlyMoving = false;
        ItemsInBack = new List<Space>();
	}

	public void SpawnAtGridPosition(int x, int y)
	{
		gridX = x;
		gridY = y;
		transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
		GameState.Instance.PlaceObjectOnBoard(Space.Player, gridX, gridY);
	}

	public int GetEvidenceCount()
	{
		int count = 0;
		for (int i = 0; i < ItemsInBack.Count; ++i)
		{
			if (ItemsInBack[i] == Space.Evidence)
			{
				++count;
			}
		}
		return count;
	}
	
	void Update()
	{		
		// Measure input axes and assign desired movement direction
		if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > controllerInputDeadzone
		    && Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= Mathf.Abs(Input.GetAxisRaw("Vertical")))
		{
			Move(Input.GetAxisRaw("Horizontal") > 0 ? Direction.East : Direction.West);
		}
		else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > controllerInputDeadzone
		         && Mathf.Abs(Input.GetAxisRaw("Vertical")) > Mathf.Abs(Input.GetAxisRaw("Horizontal")))
		{
			Move(Input.GetAxisRaw("Vertical") > 0 ? Direction.North : Direction.South);
		}
		else
		{
			nextMovementDirection = Direction.None;
		}

		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
		{
			GameState.Instance.DropFart(gridX, gridY);
		}

		if(currentlyMoving && !soundStarted)
		{
			soundStarted = true;
			player.Play();
		}
		if(!currentlyMoving)
		{
			soundStarted = false;
			player.Pause();
		}
	}

	void LateUpdate()
	{
		AnimateMove();
	}

	void Move(Direction movementDirection)
	{
		if (GameState.Instance.IsGameOver)
		{
			return;
		}

		if (movementDirection == Direction.None)
		{
			return;
		}

		if (currentlyMoving)
		{
			nextMovementDirection = movementDirection;
			return;
		}

		if (!GameState.Instance.CanIMoveHere(gridX + movementDirection.XMotion(),
		                                     gridY + movementDirection.YMotion()))
		{
			return;
		}

		currentlyMoving = true;
		currentMotionDirection = movementDirection;
		movementStartTime = Time.time;
		SetPlayerSprite();
		nextSpriteFrameTime = Time.time + spriteFrameAdvanceTime;
		player.Play();
	}
	
	void AnimateMove()
	{
		if (currentlyMoving)
		{
			var t = (Time.time - movementStartTime) / (tileMovementDuration + this.ItemsInBack.Count * GameState.Instance.WeightCostPerItem);
			if (t > 1)
			{
				var oldGridX = gridX;
				var oldGridY = gridY;
				currentlyMoving = false;
				gridX += currentMotionDirection.XMotion();
				gridY += currentMotionDirection.YMotion();
				transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
				GameState.Instance.MoveCharacter(Space.Player, oldGridX, oldGridY, gridX, gridY, this);

				Move(nextMovementDirection);
				movementStartTime = Time.time - t + 1;
			}
			else
			{
				transform.position = Vector3.Lerp(BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition), BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition + currentMotionDirection.WorldSpaceMotion()), t);
			}

			if (Time.time > nextSpriteFrameTime)
			{
				nextSpriteFrameTime += spriteFrameAdvanceTime;
				NextPlayerSprite();
			}
		}
	}

	void SetPlayerSprite()
	{
		switch (currentMotionDirection)
		{
		case Direction.North:
			currentSpriteFrameIndex = currentSpriteFrameIndex % upSprites.Length;
			sprite.sprite = upSprites[currentSpriteFrameIndex];
			break;
		case Direction.South:
			currentSpriteFrameIndex = currentSpriteFrameIndex % downSprites.Length;
			sprite.sprite = downSprites[currentSpriteFrameIndex];
			break;
		case Direction.East:
			currentSpriteFrameIndex = currentSpriteFrameIndex % rightSprites.Length;
			sprite.sprite = rightSprites[currentSpriteFrameIndex];
			break;
		case Direction.West:
			currentSpriteFrameIndex = currentSpriteFrameIndex % leftSprites.Length;
			sprite.sprite = leftSprites[currentSpriteFrameIndex];
			break;
		}
	}

	void NextPlayerSprite()
	{
		++currentSpriteFrameIndex;
		switch (currentMotionDirection)
		{
		case Direction.North:
			currentSpriteFrameIndex = currentSpriteFrameIndex % upSprites.Length;
			sprite.sprite = upSprites[currentSpriteFrameIndex];
			break;
		case Direction.South:
			currentSpriteFrameIndex = currentSpriteFrameIndex % downSprites.Length;
			sprite.sprite = downSprites[currentSpriteFrameIndex];
			break;
		case Direction.East:
			currentSpriteFrameIndex = currentSpriteFrameIndex % rightSprites.Length;
			sprite.sprite = rightSprites[currentSpriteFrameIndex];
			break;
		case Direction.West:
			currentSpriteFrameIndex = currentSpriteFrameIndex % leftSprites.Length;
			sprite.sprite = leftSprites[currentSpriteFrameIndex];
			break;
		}
	}
}
