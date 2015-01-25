using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour
{
    GameObject collectionEffectPrefab;
    public int zDepth = 5;
	public int gridX;
	public int gridY;
    public Vector2 gridSpacePosition { get { return new Vector2(gridX, gridY); } }

    public void SpawnAtGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
        transform.position = BoardDisplay.Instance.GridToWorldSpace(gridSpacePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, zDepth);
        GameState.Instance.PlaceObjectOnBoard(Space.Loot, gridX, gridY);
        GameState.Instance.Loot.Add(this);
    }

    public void DisplayCollectEffect(int x, int y)
    {
        Vector2 worldSpace = BoardDisplay.Instance.GridToWorldSpace(new Vector2(x, y));
        var collectEffect = GameObject.Instantiate(collectionEffectPrefab, new Vector3(worldSpace.x, worldSpace.y, -5), Quaternion.identity) as GameObject;
        Destroy(collectEffect, 3);
    }
}
