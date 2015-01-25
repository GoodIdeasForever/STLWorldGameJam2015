using UnityEngine;
using System.Collections;

public class Evidence : MonoBehaviour
{
	public GameObject burnEffectPrefab;
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
		GameState.Instance.PlaceObjectOnBoard(Space.Evidence, gridX, gridY);
		GameState.Instance.Evidence.Add(this);
	}

	public void DisplayBurnEffect(int x, int y)
	{
		Vector2 worldSpace = BoardDisplay.Instance.GridToWorldSpace(new Vector2(x, y));
		var burnEffect = GameObject.Instantiate(burnEffectPrefab, new Vector3(worldSpace.x, worldSpace.y, -5), Quaternion.identity) as GameObject;
		Destroy(burnEffect, 3);
	}
}
