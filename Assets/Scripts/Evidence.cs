using UnityEngine;
using System.Collections;

public class Evidence : MonoBehaviour
{
	public int gridX { get; private set; }
	public int gridY { get; private set; }
	public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }
	
	public void SpawnAtGridPosition(int x, int y)
	{
		gridX = x;
		gridY = y;
		transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
		GameState.Instance.PlaceObjectOnBoard(Space.Evidence, gridX, gridY);
	}
}
