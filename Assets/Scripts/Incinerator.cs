using UnityEngine;
using System.Collections;

public class Incinerator : MonoBehaviour
{
	public int zDepth = 5;
	public int gridX { get; private set; }
	public int gridY { get; private set; }
	public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }

	public void SpawnAtGridPosition(int x, int y)
	{
		gridX = x;
		gridY = y;
		transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
		transform.position = new Vector3(transform.position.x, transform.position.y, zDepth);
		GameState.Instance.PlaceObjectOnBoard(Space.Incinerator, gridX, gridY);
    }
}
