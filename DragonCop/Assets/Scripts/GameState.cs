using UnityEngine;
using System.Collections;

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
	public Space[][] Board;
#endregion

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
	public bool CanIMoveHere(Space piece, int x, int y)
	{
		if (this.Board[x,y] == Space.Blank)
			return true;
		else 
			return false;
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
	Wall = 0x8
}
