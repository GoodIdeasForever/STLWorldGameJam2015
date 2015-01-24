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

    
    public BackgroundSpace[] backgroundLayoutEditor;
    public int layoutHeight = 0;
    public int layoutWidth = 0;

    public GameObject[,] backgroundSpaces;

    public enum BackgroundSpace
    {
        Blank,
        Wall,
        Incinerator,
    }

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
		backgroundSpaces = new GameObject[layoutWidth,layoutHeight];
	}
	void Start () 
    	{
        	GenerateBoard();
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
        for (int i = 0; i < layoutHeight; i++)
        {
            for (int j = 0; j < layoutWidth; j++)
            {
                backgroundSpaces[j, i] = Instantiate(spaceDisplayPrefab, new Vector3(j * spaceWidth, i * spaceHeight), Quaternion.identity) as GameObject;
            }
        }
        UpdateDisplay();
    }

    public BackgroundSpace GetSpaceType(Vector2 gridSpace)
    {
        if (gridSpace.x > layoutWidth || gridSpace.y > layoutHeight || gridSpace.x < 0 || gridSpace.y < 0)
        {
            Debug.LogWarning(string.Format("Invalid gridSpace {0}", gridSpace.ToString()));
            return BackgroundSpace.Blank;
        }
        return backgroundLayoutEditor[(int)gridSpace.x + (int)gridSpace.y * layoutWidth];
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < layoutHeight; i++)
        {
            for (int j = 0; j < layoutWidth; j++)
            {
                var spriteRenderer = backgroundSpaces[j,i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    switch ( backgroundLayoutEditor[j + i * layoutWidth] )
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
                            Debug.LogWarning(string.Format("No sprite defined for background space type {0}", backgroundLayoutEditor[j + i * layoutWidth]));
                            break;
                    }
                }
            }
        }
    }



}
