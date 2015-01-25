using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreLevel : MonoBehaviour
{
	public Text levelNumber;

	void Awake()
	{
		Time.timeScale = 0;
	}

	IEnumerator Start()
	{
		while (null == GameState.Instance)
		{
			yield return null;
		}
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
	}

	public void StartLevel()
	{
		Time.timeScale = 1;
		Destroy(gameObject);
	}
}
