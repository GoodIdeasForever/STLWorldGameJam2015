﻿using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour
{
    public int zDepth = 5;
	public int gridX;
	public int gridY;
    public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }

    public void SpawnAtGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
        transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, zDepth);
        GameState.Instance.PlaceObjectOnBoard(Space.Loot, gridX, gridY);
        GameState.Instance.Loot.Add(this);
    }
}
