using UnityEngine;
using System.Collections;

public class BoardDisplay : MonoBehaviour {


    private static BoardDisplay _instance = null;

    public float spaceHeight = 24f;
    public float spaceWidth = 32f;
    public GameObject[,] spaceDisplays;
    public GameObject spaceDisplayPrefab;

    public Sprite blankSprite;
    public Sprite playerSprite;
    public Sprite enemySprite;
    public Sprite wallSprite;
    public Sprite evidenceSprite;
    public Sprite lootSprite;
    public Sprite vaultSprite;
    public Sprite incineratorSprite;

    public static BoardDisplay Instance
    {
        get
        {
            if (_instance == null)
            {
                var tempInstance = new GameObject("BoardDisplay", new System.Type[] { typeof(BoardDisplay) });
                _instance = tempInstance.GetComponent<BoardDisplay>();
            }

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

    public void GenerateBoard()
    {
        for (int i = 0; i < GameState.Instance.BoardWidth; i++)
        {
            for (int j = 0; j <GameState.Instance.BoardHeight; j++)
            {
                spaceDisplays[i, j] = Instantiate(spaceDisplayPrefab, new Vector3(i * spaceWidth, j * spaceHeight), Quaternion.identity) as GameObject;
            }
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < GameState.Instance.BoardWidth; i++)
        {
            for (int j = 0; j < GameState.Instance.BoardHeight; j++)
            {
                spaceDisplays[i, j] = Instantiate(spaceDisplayPrefab, new Vector3(i * spaceWidth, j * spaceHeight), Quaternion.identity) as GameObject;
            }
        }
    }
}
