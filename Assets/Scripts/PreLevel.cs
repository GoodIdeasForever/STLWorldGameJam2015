using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreLevel : MonoBehaviour
{
	public Text levelNumber;
	public Text levelDescription;

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
		yield return null;
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
		levelDescription.text = string.Format("Evidence to burn: {0}\n\n" + 
		                                      "Drop extra evidence every {1} seconds\n\n" +
		                                      "Dragoncop count: {2}\n\n" +
		                                      "Can carry {3} item{4}\n\n" +
		                                      "Loot available: {5}",
		                                      GameState.Instance.Evidence.Count,
		                                      GameState.Instance.TimeTillEvidenceDrop,
		                                      GameObject.FindObjectsOfType<Enemy>().Length,
		                                      GameState.Instance.MaxNumberOfItemsInPack,
		                                      GameState.Instance.MaxNumberOfItemsInPack > 1 ? "s" : "",
		                                      GameState.Instance.Loot.Count);
	}

	public void StartLevel()
	{
		Time.timeScale = 1;
		Destroy(gameObject);
	}
}
