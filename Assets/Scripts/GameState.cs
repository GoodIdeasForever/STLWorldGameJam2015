using UnityEngine;
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
#endregion

	public GameState()
	{
		this.Enemies = new List<Enemy>();
		this.Loot = new List<Loot>();
		this.Evidence = new List<Evidence>();
        this.Board = new Space[this.BoardWidth, this.BoardHeight];
	}
#region Properties
	public static GameState Instance 
	{
		get 
		{ 
			if (_instance == null)
				_instance = new GameState();
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
        this.Board[oldX, oldY] = Space.Blank;
        this.Board[newX, newY] = character;
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
public enum Direction
{
	North = 0x0,
	South = 0x2,
	East = 0x4,
	West = 0x8
}
