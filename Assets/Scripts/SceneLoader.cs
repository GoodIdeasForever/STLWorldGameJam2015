using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
	public void LoadNewScene(string sceneToLoad)
	{
		Application.LoadLevel(sceneToLoad);
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}
}
