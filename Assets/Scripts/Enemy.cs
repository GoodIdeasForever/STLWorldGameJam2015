using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 gridSpacePosition { get { return new Vector2(X, Y); } }
    private bool currentlyMoving = false;
    public Direction FacingDirection { get; private set; }
	// Use this for initialization
	void Start () 
	{
		
	}

    void Awake()
    {
        FacingDirection = Direction.South;
        currentlyMoving = false;
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void FixedUpdate() 
	{
        if (!currentlyMoving)
            Move();
	}
    int PrioritizeMoves(Vector2[] moves)
    {
        if (moves.Length.Equals(1))
            return 0;
        else
        {
            int answer = 0;
            int accuracyNumber = Random.Range(0, 101);

            if (GameState.Instance.PathfindingAccuracy <= accuracyNumber)
            { // is chasing
                
            }
            else
            {
                answer = Random.Range(0, moves.Length);
            }
            return answer;
        }
    }

	Vector2? FindMove()
	{
        Vector2[] moves = GameState.Instance.GetPossibleMoves(X, Y);
        if (!moves.Length.Equals(0))
        {
            return moves[PrioritizeMoves(moves)];
        }
        return null;
	}
	void Move()
	{
        Vector2? nextMove = FindMove();
        if (nextMove.HasValue)
        {
            this.currentlyMoving = true;
            AnimateMove();
            GameState.Instance.MoveCharacter(Space.Enemy, X, Y, (int)nextMove.Value.x, (int)nextMove.Value.y);
            this.currentlyMoving = false;
        }
	}
	void AnimateMove()
	{
	    
	}
}
