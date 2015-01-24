using UnityEngine;
using System.Collections;

public class BoardDisplay : MonoBehaviour {


    private static BoardDisplay _instance = null;

    public float spaceHeight = 24f;
    public float spaceWidth = 32f;

    public static BoardDisplay Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BoardDisplay();
            return _instance;
        }
    }

	void Start () 
    {
	
	}
	
	void Update () 
    {

	}

    public Vector2 GridToWorldSpace(Vector2 gridSpace)
    {
        return new Vector2(gridSpace.x * spaceWidth, gridSpace.y * spaceHeight);
    }
}
