using UnityEngine;
using System.Collections;

public class BoardDisplay : MonoBehaviour {

    public float spaceHeight = 24f;
    public float spaceWidth = 32f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector2 GridToWorldSpace(Vector2 gridSpace)
    {
        return new Vector2(gridSpace.x * spaceWidth, gridSpace.y * spaceHeight);
    }
}
