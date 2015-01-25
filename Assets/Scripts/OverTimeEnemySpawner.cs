using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnPosition
{
	public int X;
	public int Y;
}

public class OverTimeEnemySpawner : MonoBehaviour
{
	public Enemy enemyPrefab;
	public float spawnEveryXSeconds = 10;
	public SpawnPosition[] spawnPositions = new SpawnPosition[1];

	private float nextEnemySpawnTime;

	void Awake()
	{
		nextEnemySpawnTime = Time.time + spawnEveryXSeconds;
	}

	void Update()
	{
		if (Time.time > nextEnemySpawnTime)
		{
			nextEnemySpawnTime += spawnEveryXSeconds;

			var newEnemy = GameObject.Instantiate(enemyPrefab) as Enemy;

			var spawnPositionIndex = Random.Range(0, spawnPositions.Length);
			newEnemy.SpawnAtGridPosition(spawnPositions[spawnPositionIndex].X, spawnPositions[spawnPositionIndex].Y);
		}
	}
}
