﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
#region Private Field
	private static GameState _instance = null;
#endregion

#region Public Fields
	public float SpeedOfPlayer;
	public float TimeTillEvidenceDrop;
	public float SpeedOfCop;
	public float PathfindingAccuracy;
	public int NumberOfCops;
	public int NumberOfEvidencePieces;
	public int NumberOfEvidenceDestroyed = 0;
	public int TotalPossibleEvidencePieces;
	public int Width;
	public int Height;
	public int LevelNumber = 1;
    public int BoardWidth = 32;
    public int BoardHeight = 24;
	public Space[,] Board;
	
	public List<Enemy> Enemies;
	public List<Loot> Loot;
	public List<Evidence> Evidence;
    public Player Player;
#endregion

	public GameState()
	{
		this.Enemies = new List<Enemy>();
		this.Loot = new List<Loot>();
		this.Evidence = new List<Evidence>();
        this.Board = new Space[this.BoardWidth, this.BoardHeight];
        this.IsGameOver = false;
        this.DidPlayerWin = false;
	}
#region Properties
	public static GameState Instance 
	{
        get
        {
            if (_instance == null)
            {
                var tempInstance = new GameObject("GameState", new System.Type[] { typeof(GameState) });
                _instance = tempInstance.GetComponent<GameState>();
            }

            return _instance;
        }
	}
	public int Score 
	{ 
		get 
		{
			return this.TotalPossibleEvidencePieces - this.NumberOfEvidenceDestroyed;
		}
	}
    public bool IsGameOver
    {
        get;
        private set;
    }
    public bool DidPlayerWin
    {
        get;
        private set;
    }
#endregion
#region Public Functions
	public bool CanIMoveHere(int x, int y)
	{
        if (x >= 0 && x < this.Width && y >= 0 && y < this.Height)
        {
            if (!this.Board[x, y].Equals(Space.Wall))
                return true;
            else
                return false;
        }
        else 
            return false;
	}
    public Vector2[] GetPossibleMoves(int x, int y)
    {
        List<Vector2> validMoves = new List<Vector2>();
        if (this.CanIMoveHere(x, y + 1))
        { // Can I Move North
            validMoves.Add(new Vector2(x, y + 1));
        }
        else if (this.CanIMoveHere(x, y - 1))
        { // Can I Move North
            validMoves.Add(new Vector2(x, y - 1));
        }
        else if (this.CanIMoveHere(x + 1, y))
        { // Can I Move North
            validMoves.Add(new Vector2(x + 1, y));
        }
        else if (this.CanIMoveHere(x - 1, y))
        { // Can I Move North
            validMoves.Add(new Vector2(x - 1, y));
        }
        return validMoves.ToArray();
    }
    public void MoveCharacter(int oldX, int oldY, int newX, int newY)
    {
        Space character = this.Board[oldX, oldY];
        Space newSpace = this.Board[newX, newY];
        if ((newSpace.Equals(Space.Enemy) || newSpace.Equals(Space.Player)) &&
            (character.Equals(Space.Enemy) || character.Equals(Space.Player)))
        {
            this.DidPlayerWin = false;
            this.IsGameOver = true;
        }
        else if (this.Score.Equals(this.TotalPossibleEvidencePieces))
        {
            this.DidPlayerWin = true;
            this.IsGameOver = true;
        }

        this.Board[oldX, oldY] = Space.Blank;
        this.Board[newX, newY] = character;
    }

	public void PlaceObjectOnBoard(Space objectType, int x, int y)
	{
		if (this.Board[x, y].IsSet(Space.Wall))
		{
			this.Board[x, y].Set(objectType);
		}
		else
		{
			Debug.LogError("You can't put a thing there is a wall!");
		}
	}
    
#endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate() {
	
	}
}

[System.Flags]
public enum Space
{
	Blank = 0x0,
	Player = 0x2,
	Enemy = 0x4,
	Wall = 0x8,
	Evidence = 0x16,
	Loot = 0x32,
	Vault = 0x64,
	Incinerator = 0x128
}

public static class SpaceExtensions 
{
	public static bool IsSet(this Space space, Space flags)
	{
		return (space & flags) == flags;
	}
	
	public static bool IsNotSet(this Space space, Space flags)
	{
		return (space & (~flags)) == 0;
	}
	
	public static Space Set(this Space space, Space flags)
	{
		return space | flags;
	}
	
	public static Space Clear(this Space space, Space flags)
	{
		return space & (~flags);
	}
}

[System.Flags]
public enum Direction
{
	North = 0x1,
	South = 0x2,
	East = 0x4,
	West = 0x8
}

public static class DirectionExtentions
{
	public static int XMotion(this Direction direction)
	{
		return ((int)(direction & Direction.East) >> 2) - ((int)(direction & Direction.West) >> 3);
	}

	public static int YMotion(this Direction direction)
	{
		return ((int)(direction & Direction.North)) - ((int)(direction & Direction.South) >> 1);
    }

	public static Vector2 WorldSpaceMotion(this Direction direction)
	{
		var motion = Vector2.zero;

		if ((direction & Direction.North) != 0)
		{
			motion += Vector2.up;
		}

		if ((direction & Direction.South) != 0)
		{
			motion += -Vector2.up;
        }
        
		if ((direction & Direction.East) != 0)
		{
			motion += Vector2.right;
        }
        
		if ((direction & Direction.West) != 0)
		{
			motion += -Vector2.right;
        }
        
        return motion;
    }
}
