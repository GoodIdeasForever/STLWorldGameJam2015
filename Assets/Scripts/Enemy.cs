using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int X = 0;
	public int Y = 0;
    private bool _moving = false;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void FixedUpdate() 
	{
	
	}
	Vector2? FindMove()
	{
        Vector2[] moves = GameState.Instance.GetPossibleMoves(X, Y);
        if (!moves.Length.Equals(0))
        {
            int direction = Random.Range(0, moves.Length);
            return moves[direction];
        }
        return null;
	}
	void Move()
	{
		
	}
	void AnimateMove()
	{
	
	}
}
