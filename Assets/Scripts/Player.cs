using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int X = 0;
	public int Y = 0;
	public Player()
	{
		this.HasItem = false;
		this.FacingDirection = Direction.South;
	}
	public bool HasItem
	{
		get;
		private set;
	}
	public void PickupItem()
	{
		this.HasItem = true;
	}
	public void DropItem()
	{
		this.HasItem = false;
	}
	public Direction FacingDirection
	{
		get; 
		private set;
	}

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
		AnimateMove();
	}
	
	void Move()
	{
		
	}
	
	void AnimateMove()
	{
	
	}
}
