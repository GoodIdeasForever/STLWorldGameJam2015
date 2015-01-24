using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	public string SceneToLoad = "Main"; //Initialize this as the main scene in case of emergency I guess

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadNewScene (string SceneToLoad) {
		Application.LoadLevel(SceneToLoad);
	}
}
