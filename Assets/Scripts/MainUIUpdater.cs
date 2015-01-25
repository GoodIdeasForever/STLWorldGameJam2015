using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUIUpdater : MonoBehaviour
{
	static public MainUIUpdater Instance;

	public Image[] evidenceIcons;
	public Text timeRemainingText;
	public Text levelNumber;

	void Awake()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Update()
	{
		for (int i = 0; i < evidenceIcons.Length; ++i)
		{
			evidenceIcons[i].enabled = GameState.Instance.Evidence.Count >= (i + 1);
		}

		timeRemainingText.text = string.Format("{0:D2}:{1:D2}", (int)(GameState.Instance.TimeTillEvidenceDrop / 60), (int)(GameState.Instance.TimeTillEvidenceDrop % 60));
	}

	void Start()
	{
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
	}
}
