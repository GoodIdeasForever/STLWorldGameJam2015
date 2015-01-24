using UnityEngine;
using System.Collections;

public class SetLevelNumber : MonoBehaviour
{
	public UnityEngine.UI.Text levelNumberLabel;

	void Start()
	{
		if (null != NextLevelTracker.nextLevelInfo)
		{
			levelNumberLabel.text = string.Format("{0}", NextLevelTracker.nextLevelInfo.levelNumber);
		}
	}
}
