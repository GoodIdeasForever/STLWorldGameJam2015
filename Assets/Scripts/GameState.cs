using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
#region Private Field
	private static GameState _instance = null;
	private float nextEvidenceDropTime;
#endregion

#region Public Fields
	public float SpeedOfPlayerMultiplier = 1;
	public float TimeTillEvidenceDrop;
	public float SpeedOfCopMultiplier = 1;
	public float PathfindingAccuracy;
    public int NumberOfLootCollected = 0;
	public int NumberOfEvidenceDestroyed = 0;
    public int MaxNumberOfItemsInPack = 1;
    public float WeightCostPerItem = 0;
	public int LevelNumber = 1;
    public int BoardWidth = 32;
    public int BoardHeight = 24;
	public Space[,] Board;
	
	public List<Enemy> Enemies;
	public List<Loot> Loot;
	public List<Evidence> Evidence;
    public Player Player;
	public AudioSource levelup; 
	public AudioSource destroy;
    public int MaxNumberOfFartsAllowedOnField = 500;
    public int NumberOfFartsOnField = 0;
    public int FartLifeSpanInMilliseconds = 1000;
    public int CopSleepWhenOnFartInMilliseconds = 1000;
    public Dictionary<Vector2, System.DateTime> Farts = new Dictionary<Vector2, System.DateTime>();
    public GameObject fartPrefab;
#endregion

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
	public float NextEvidenceDropTime
	{
		get { return Mathf.Max(nextEvidenceDropTime - Time.time, 0); }
	}
#endregion
#region Public Functions
    public void DropFart(int x, int y)
    {
        if (this.NumberOfFartsOnField < this.MaxNumberOfFartsAllowedOnField)
        {
            if (!this.Board[x, y].IsSet(Space.Fart))
            {
                this.Farts[new Vector2(x, y)] = System.DateTime.Now;
                this.Board[x, y] = this.Board[x, y].Set(Space.Fart);
                this.NumberOfFartsOnField++;
            }
        }
    }
    public void RemoveFart(int x, int y)
    {
        if (this.NumberOfFartsOnField > 0)
        {
            if (this.Board[x, y].IsSet(Space.Fart))
            {
                this.Board[x, y] = this.Board[x, y].Clear(Space.Fart);
                this.NumberOfFartsOnField--;
            }
        }
    }
    public bool CanIMoveHere(int x, int y, Space invalidFlags = Space.Wall)
	{
        if ((x >= 0 && x < this.BoardWidth && y >= 0 && y < this.BoardHeight) && ((invalidFlags & this.Board[x, y]) == Space.Blank))
        {
            return true;
        }
        else 
            return false;
	}
    public Vector2[] GetPossibleMoves(int x, int y, Space invalidFlags = Space.Wall)
    {
        List<Vector2> validMoves = new List<Vector2>();
        if (this.CanIMoveHere(x + 1, y, invalidFlags))
        { // Can I Move North
            validMoves.Add(new Vector2(x + 1, y));
        }
        if (this.CanIMoveHere(x - 1, y, invalidFlags))
        { // Can I Move North
            validMoves.Add(new Vector2(x - 1, y));
        }
        if (this.CanIMoveHere(x, y + 1, invalidFlags))
        { // Can I Move North
            validMoves.Add(new Vector2(x, y + 1));
        }
        if (this.CanIMoveHere(x, y - 1, invalidFlags))
        { // Can I Move North
            validMoves.Add(new Vector2(x, y - 1));
        }
        
        return validMoves.ToArray();
    }
    public void MoveCharacter(Space space, int oldX, int oldY, int newX, int newY, Object character)
    {
        Space oldSpace = this.Board[oldX, oldY];
        Space newSpace = this.Board[newX, newY];
		this.Board[oldX, oldY] = oldSpace.Clear(space);
        if (space.IsSet(Space.Player))
        {
            if (newSpace.IsSet(Space.Incinerator))
            {
				destroy.Play();
                List<Space> evidence = Player.ItemsInBack.FindAll(t => t == Space.Evidence);
				if (evidence.Count > 0)
				{
					BoardDisplay.Instance.evidencePrefab.DisplayBurnEffect(newX, newY);
				}
                this.NumberOfEvidenceDestroyed+= evidence.Count;
                Player.ItemsInBack.RemoveAll(t => t == Space.Evidence);
            }
            if (newSpace.IsSet(Space.Vault))
            {
                destroy.Play();
                List<Space> loot = Player.ItemsInBack.FindAll(t => t == Space.Loot);
                this.NumberOfLootCollected += loot.Count;
                Player.ItemsInBack.RemoveAll(t => t == Space.Loot);
            }
            if (Evidence.Count == 0 && Player.GetEvidenceCount() == 0)
            {
                this.DidPlayerWin = true;
                this.IsGameOver = true;
				levelup.Play();
				StartCoroutine(LoadNextLevel());
			}
            if (newSpace.IsSet(Space.Evidence) && Player.ItemsInBack.Count < MaxNumberOfItemsInPack)
            {
                Player.ItemsInBack.Add(Space.Evidence);
				DeleteEvidenceAt(newX, newY);
				this.Board[newX, newY] = this.Board[newX, newY].Clear(Space.Evidence);
            }
            if (newSpace.IsSet(Space.Loot) && Player.ItemsInBack.Count < MaxNumberOfItemsInPack)
            {
                Player.ItemsInBack.Add(Space.Loot);
				DeleteLootAt(newX, newY);
				this.Board[newX, newY] = this.Board[newX, newY].Clear(Space.Loot);
            }
            if (newSpace.IsSet(Space.Enemy))
            {
                this.DidPlayerWin = false;
                this.IsGameOver = true;
				StartCoroutine(LoadTitleScene());
            }
        }
        else if (space.IsSet(Space.Enemy))
        {
            if (newSpace.IsSet(Space.Fart))
            {
                this.RemoveFart(newX, newY);
                ((Enemy)character).FreezeEnemyFor(this.CopSleepWhenOnFartInMilliseconds);
            }
            else if (newSpace.IsSet(Space.Player))
            {
                this.DidPlayerWin = false;
                this.IsGameOver = true;
				StartCoroutine(LoadTitleScene());
            }
        }
		this.Board[newX, newY] = this.Board[newX, newY].Set(space);
    }

	IEnumerator LoadNextLevel()
	{
		Application.LoadLevelAdditive("LevelCompleteUI");
		yield return new WaitForSeconds(5);
		NextLevelTracker.instance.PlayNextGameLevel();
	}

	IEnumerator LoadTitleScene()
	{
		yield return new WaitForSeconds(5);
		Application.LoadLevel("GameOver");
    }
	
	public void PlaceObjectOnBoard(Space objectType, int x, int y)
	{
		if (!this.Board[x, y].IsSet(Space.Wall))
		{
			this.Board[x, y] = this.Board[x, y].Set(objectType);
		}
		else
		{
			Debug.LogError("You can't put a thing there is a wall!");
		}
	}
    
	void DeleteEvidenceAt(int X, int Y)
	{
		for (int i = 0; i < Evidence.Count; ++i)
		{
			if (Evidence[i].gridX == X && Evidence[i].gridY == Y)
			{
				Destroy(Evidence[i].gameObject);
				Evidence.RemoveAt(i);
				return;
			}
		}
	}

	void DeleteLootAt(int X, int Y)
	{
		for (int i = 0; i < Loot.Count; ++i)
		{
			if (Loot[i].gridX == X && Loot[i].gridY == Y)
			{
				Destroy(Loot[i].gameObject);
				Loot.RemoveAt(i);
				return;
			}
		}
	}
#endregion

	void Awake()
	{
		if (null == _instance)
		{
			_instance = this;
		}
		
        this.Enemies = new List<Enemy>();
        this.Loot = new List<Loot>();
        this.Evidence = new List<Evidence>();
        this.Board = new Space[this.BoardWidth, this.BoardHeight];
        this.IsGameOver = false;
        this.DidPlayerWin = false;
        for (int i = 0; i < this.BoardWidth; i++)
        {
            for (int j = 0; j < this.BoardHeight; j++)
            {
                this.Board[i, j] = Space.Blank;
            }
        }
		Application.LoadLevelAdditive("PlayUI");
	}

    void Update()
    {
        foreach (KeyValuePair<Vector2, System.DateTime> keyPair in this.Farts)
        {
            if ((System.DateTime.Now - keyPair.Value).TotalMilliseconds >= FartLifeSpanInMilliseconds)
            {
                RemoveFart((int)keyPair.Key.x, (int)keyPair.Key.y);
            }
            else
            {
                Vector2 worldSpace = BoardDisplay.Instance.GridToWorldSpace(keyPair.Key);
                var fart = GameObject.Instantiate(fartPrefab, new Vector3(worldSpace.x, worldSpace.y, 10), Quaternion.identity) as GameObject;
				Destroy (fart, 6);
            }
        }

		if (TimeTillEvidenceDrop > 0 && Time.time > nextEvidenceDropTime && !Board[Player.gridX, Player.gridY].IsSet(Space.Evidence))
		{
			nextEvidenceDropTime += TimeTillEvidenceDrop;

			Evidence evidence = Instantiate(BoardDisplay.Instance.evidencePrefab) as Evidence;
			evidence.SpawnAtGridPosition(Player.gridX, Player.gridY);
		}
    }

    void Start()
    {
        BoardDisplay.Instance.GenerateBoard();
		if (TimeTillEvidenceDrop > 0)
		{
			nextEvidenceDropTime = Time.time + TimeTillEvidenceDrop;
		}
    }
	
	void OnDestroy()
	{
		if (this == _instance)
        {
            _instance = null;
        }
    }
}

[System.Flags]
public enum Space
{
	Blank = 0,
	Player = 2,
	Enemy = 4,
	Wall = 8,
	Evidence = 16,
	Loot = 32,
	Vault = 64,
	Incinerator = 128,
    Fart = 256
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
	None = 0x0,
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

	public static Direction GetDirection(int startX, int startY, int endX, int endY)
	{
		Direction computedDirection = Direction.None;
		if (endX > startX)
		{
			computedDirection |= Direction.East;
		}
		if (startX > endX)
		{
			computedDirection |= Direction.West;
		}
		if (endY > startY)
		{
			computedDirection |= Direction.North;
		}
		if (startY > endY)
		{
			computedDirection |= Direction.South;
		}
		return computedDirection;
	}
}
