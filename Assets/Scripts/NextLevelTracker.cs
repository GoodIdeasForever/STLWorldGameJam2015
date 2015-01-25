using UnityEngine;
using System.Collections;

public class NextLevelInfo
{
	public string levelName;
	public int levelNumber;
}

public class NextLevelTracker : MonoBehaviour
{
	public static NextLevelTracker instance;

	public static NextLevelInfo nextLevelInfo;

	public string nextLevelName;
	public int nextLevelNumber;

	void Awake()
	{
		instance = this;
		if (!string.IsNullOrEmpty(nextLevelName))
		{
			nextLevelInfo = new NextLevelInfo() { levelName = nextLevelName, levelNumber = nextLevelNumber };
		}
	}

	void OnDestroy()
	{
		instance = null;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.J))
		{
			PlayNextGameLevel();
		}
	}

	public void PlayNextGameLevel()
	{
		Application.LoadLevel(nextLevelName);
	}
}
