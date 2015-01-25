using UnityEngine;
using System.Collections;

public class LevelCompleteOverlay : MonoBehaviour
{
	public UnityEngine.UI.Text levelNumber;

	void Start()
	{
		levelNumber.text = GameState.Instance.LevelNumber.ToString();
	}
}
