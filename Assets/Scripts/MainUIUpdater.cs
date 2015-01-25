using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUIUpdater : MonoBehaviour
{
	public Image[] evidenceIcons;
	public Text timeRemainingText;
	public Text levelNumber;

	void Update()
	{
		for (int i = 0; i < evidenceIcons.Length; ++i)
		{
			evidenceIcons[i].enabled = GameState.Instance.Evidence.Count >= (i + 1);
		}

		timeRemainingText.text = string.Format("{0:2N}:{1:2N}", GameState.Instance.TimeTillEvidenceDrop / 60, GameState.Instance.TimeTillEvidenceDrop % 60);
	}

	void Start()
	{
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
	}
}
