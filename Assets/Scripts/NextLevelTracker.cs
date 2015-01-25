﻿using UnityEngine;
using System.Collections;

public class NextLevelInfo
{
	public string levelName;
	public int levelNumber;
}

public class NextLevelTracker : MonoBehaviour
{
	public static NextLevelInfo nextLevelInfo;

	public string nextLevelName;
	public int nextLevelNumber;

	void Awake()
	{
		if (!string.IsNullOrEmpty(nextLevelName))
		{
			nextLevelInfo = new NextLevelInfo() { levelName = nextLevelName, levelNumber = nextLevelNumber };
		}
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
		if (Application.loadedLevelName == "PreLevel" || nextLevelNumber == 0)
		{
			Application.LoadLevel(nextLevelInfo.levelName);
		}
		else
		{
			Application.LoadLevel("PreLevel");
		}
	}
}
