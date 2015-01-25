using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float tileMovementDuration = 1.0f;
	public AudioSource enemy;

    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 gridSpacePosition { get { return new Vector2(X, Y); } }
    private bool currentlyMoving = false;
    public Direction FacingDirection { get; private set; }
    Direction currentMotionDirection;
    float movementStartTime;


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
    Vector2 PrioritizeMoves(Vector2 start, Vector2[] moves)
    {
		if (moves.Length.Equals(1))
			return moves[0];
		else
		{
            Vector2 answer = moves[Random.Range(0, moves.Length)];
			int accuracyNumber = Random.Range(0, 101);
			if (GameState.Instance.PathfindingAccuracy <= accuracyNumber)
			{ // is chasing
                List<Vector2> path = AStar(start, GameState.Instance.Player.gridSpacePosition);
                if (path != null)
                {
                    answer = path[path.Count - 2];
                }
			}
			return answer;
        }
    }

    int GetLeastDistance(Vector2 point1, Vector2 point2)
    {
        return (int)(Mathf.Abs(point1.x - point2.x) + Mathf.Abs(point1.y - point2.y));
    }
    
    List<Vector2> AStar(Vector2 start, Vector2 goal)
    {
        List<Vector2> closedSet = new List<Vector2>();
        List<Vector2> openSet = new List<Vector2>() { start };
        Dictionary<Vector2, Vector2> came_from = new Dictionary<Vector2, Vector2>();

        Dictionary<Vector2, int> gScore = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> fScore = new Dictionary<Vector2, int>();
        gScore[start] = 0;
        fScore[start] = gScore[start] + GetLeastDistance(start,goal);

        while (openSet.Count > 0)
        {
            Vector2 current = openSet[0]; ///////// GLARING DISASTER
            for (int i=1; i< openSet.Count; i++)
            {
            	if (fScore[openSet[i]] < fScore[openSet[i-1]])
            		current = openSet[i];
            }
            if (current == goal)
                return reconstruct_path(came_from, start, goal);
            openSet.Remove(current);
            closedSet.Add(current);
			Vector2[] neighbors = GameState.Instance.GetPossibleMoves((int)current.x, (int)current.y, Space.Wall | Space.Enemy);
            foreach (Vector2 neighbor in neighbors)
            {
                if (!closedSet.Contains(neighbor))
                {
                    int tenative_g_score = gScore[current] + GetLeastDistance(current, neighbor);

                    if (!openSet.Contains(neighbor) || (gScore.ContainsKey(neighbor) ? tenative_g_score < gScore[neighbor] : false))
                    {
                        came_from[neighbor] = current;
                        gScore[neighbor] = tenative_g_score;
                        fScore[neighbor] = gScore[neighbor] + GetLeastDistance(neighbor, goal);
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    List<Vector2> reconstruct_path(Dictionary<Vector2, Vector2> came_from, Vector2 start, Vector2 current)
    {
        List<Vector2> totalPath = new List<Vector2>() { current };
        while (current != start)
        {
        	Vector2 oldCurrent = current;
            current = came_from[current];			
			came_from.Remove(oldCurrent);
            totalPath.Add(current);
        }
        return totalPath;
    }

    Vector2? FindMove()
	{
        Vector2[] moves = GameState.Instance.GetPossibleMoves(X, Y, Space.Wall | Space.Enemy);
        if (!moves.Length.Equals(0))
        {
            return PrioritizeMoves(new Vector2(X,Y), moves);
        }
        return null;
	}
	void Move()
	{
		if (GameState.Instance.IsGameOver)
		{
			return;
		}

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
                GameState.Instance.MoveCharacter(Space.Enemy, oldGridX, oldGridY, X, Y);
            }
            else
            {
                transform.position = Vector3.Lerp(BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition), BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition + currentMotionDirection.WorldSpaceMotion()), t);
            }
        }
	}

	public void SpawnAtGridPosition(int x, int y)
	{
		X = x;
		Y = y;
		transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
		GameState.Instance.PlaceObjectOnBoard(Space.Enemy, X, Y);
	}
}
