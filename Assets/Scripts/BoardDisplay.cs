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
    public Incinerator incineratorPrefab;
    public Player playerPrefab;
    public Enemy enemyPrefab;
    public Evidence evidencePrefab;

    
    public BackgroundSpace[] backgroundLayoutEditor;
    public int layoutHeight = 0;
    public int layoutWidth = 0;

    public GameObject[,] backgroundSpaces;

    public enum BackgroundSpace
    {
        Blank,
        Wall,
        Incinerator,
        PlayerSpawn,
        EnemySpawn,
        EvidencePlacement
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

    public void GenerateBoard()
    {
        for (int i = 0; i < layoutHeight; i++)
        {
            for (int j = 0; j < layoutWidth; j++)
            {
                backgroundSpaces[j,i] = Instantiate(spaceDisplayPrefab, new Vector3(j * spaceWidth, i * spaceHeight, 10), Quaternion.identity) as GameObject;
                backgroundSpaces[j, i].transform.SetParent(gameObject.transform);
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
                            //GameState.Instance.PlaceObjectOnBoard(Space.Blank, j, i);
                            break;
                        case BackgroundSpace.Wall:
                            spriteRenderer.sprite = wallSprite;
                            GameState.Instance.PlaceObjectOnBoard(Space.Wall, j, i);
                            break;
                        case BackgroundSpace.Incinerator:
                            spriteRenderer.sprite = incineratorSprite;
                            Incinerator incinerator = Instantiate(incineratorPrefab) as Incinerator;
                            incinerator.SpawnAtGridPosition(j, i);
                            break;
                        case BackgroundSpace.EvidencePlacement:
                            Evidence evidence = Instantiate(evidencePrefab) as Evidence;
                            evidence.SpawnAtGridPosition(j, i);
                            break;
                        case BackgroundSpace.EnemySpawn:
                            Enemy enemy = Instantiate(enemyPrefab) as Enemy;
                            enemy.SpawnAtGridPosition(j, i);
                            break;
                        case BackgroundSpace.PlayerSpawn:
                            Player player = Instantiate(playerPrefab) as Player;
                            player.SpawnAtGridPosition(j, i);
							GameState.Instance.Player = player;
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
