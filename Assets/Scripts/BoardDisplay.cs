using UnityEngine;
using UnityEditor;
using System.Collections;

public class BoardDisplay : MonoBehaviour {


    private static BoardDisplay _instance = null;

    public float spaceHeight = 2f;
    public float spaceWidth = 2f;
    public GameObject spaceDisplayPrefab;
    public Sprite blankSprite;
    public Sprite wallSprite;
    public Sprite incineratorSprite;
    public BackgroundSpace[,] backgroundLayout;

    public GameObject[,] backgroundSpaces;

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

	void Awake()
	{
		if (null == _instance)
		{
			_instance = this;
		}
	}

	void OnDestroy()
	{
		if (this == _instance)
		{
			_instance = null;
		}
	}

    public Vector2 GridToWorldSpace(Vector2 gridSpace)
    {
        return new Vector2(gridSpace.x * spaceWidth, gridSpace.y * spaceHeight);
    }

   [MenuItem("GameJam/GenerateBoard")]
    public void GenerateBoard()
    {
        for (int i = 0; i < GameState.Instance.BoardWidth; i++)
        {
            for (int j = 0; j <GameState.Instance.BoardHeight; j++)
            {
                backgroundSpaces[i, j] = Instantiate(spaceDisplayPrefab, new Vector3(i * spaceWidth, j * spaceHeight), Quaternion.identity) as GameObject;
            }
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < GameState.Instance.BoardWidth; i++)
        {
            for (int j = 0; j < GameState.Instance.BoardHeight; j++)
            {
                var spriteRenderer = backgroundSpaces[i,j].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    switch ( backgroundLayout[i,j] )
                    {
                        case BackgroundSpace.Blank:
                            spriteRenderer.sprite = blankSprite;
                            break;
                        case BackgroundSpace.Wall:
                            spriteRenderer.sprite = wallSprite;
                            break;
                        case BackgroundSpace.Incinerator:
                            spriteRenderer.sprite = incineratorSprite;
                            break;
                        default:
                            Debug.LogWarning(string.Format("No sprite defined for background space type {0}", backgroundLayout[i, j].ToString()));
                            break;
                    }
                }
            }
        }
    }

    public enum BackgroundSpace
    {
        Blank,
        Wall,
        Incinerator,
    }
}
