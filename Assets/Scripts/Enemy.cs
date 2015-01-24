using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float tileMovementDuration = 1.0f;

    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 gridSpacePosition { get { return new Vector2(X, Y); } }
    private bool currentlyMoving = false;
    public Direction FacingDirection { get; private set; }
    Direction currentMotionDirection;
    float movementStartTime;
	AudioSource enemy;


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
        if (!currentlyMoving)
            Move();
	}
    void LateUpdate()
    {
        AnimateMove();
    }
    int PrioritizeMoves(Vector2[] moves)
    {
		if (moves.Length.Equals(1))
			return 0;
		else
		{
			int answer = 0;
			int accuracyNumber = Random.Range(0, 101);
			float tempDistance;
			if (GameState.Instance.PathfindingAccuracy <= accuracyNumber)
			{ // is chasing
				Vector2 target = GameState.Instance.Player.transform.position;
				float distance = 1000;
				
				for(int i = 0; i < moves.Length; i++)
				{
					tempDistance = Vector2.Distance(moves[i], target);
					if (tempDistance < distance)
						answer = i;
				}
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
			enemy.Play();
            movementStartTime = Time.time;
            currentMotionDirection = DirectionExtentions.GetDirection(X, Y, (int)nextMove.Value.x, (int)nextMove.Value.y);
            this.currentlyMoving = true;
        }
	}
	void AnimateMove()
	{
        if (currentlyMoving)
        {
            var t = (Time.time - movementStartTime) / tileMovementDuration;
            if (t > 1)
            {
                var oldGridX = X;
                var oldGridY = Y;
                currentlyMoving = false;
                X += currentMotionDirection.XMotion();
                Y += currentMotionDirection.YMotion();
                transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
                GameState.Instance.MoveCharacter(Space.Player, oldGridX, oldGridY, X, Y);
            }
            else
            {
                transform.position = Vector3.Lerp(BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition), BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition + currentMotionDirection.WorldSpaceMotion()), t);
            }
        }
	}
}
