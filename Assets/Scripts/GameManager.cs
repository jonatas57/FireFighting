using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
  FREE,
  HOLE,
  HYDRANT,
  FIRE
}

public class GameManager : MonoBehaviour
{

  private static GameManager instance = null;
  private Vector3 xAxis;
  private Vector3 yAxis;
  public GameObject playerPrefab;
  public GameObject holePrefab;
  public GameObject firePrefab;
  public GameObject bonusPrefab;


  private TileType[][] board;
  private int boardSize;
  public float waterForce;

  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new GameObject("GM").AddComponent<GameManager>();
      }
      return instance;
    }
  }

  void Awake()
  {
    if (instance != null)
    {
      DestroyImmediate(this);
      return;
    }
    instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {
    Vector3 size = new Vector3(21, 21, 0);
    xAxis = size.x * Vector3.right;
    yAxis = size.y * Vector3.up;

    boardSize = 12;
    board = new TileType[boardSize + 2][];
    for (int i = 0; i < boardSize + 2; i++)
    {
      board[i] = new TileType[boardSize + 2];
      for (int j = 0; j < boardSize + 2; j++)
      {
        if (i == 0 || j == 0 || i == boardSize + 1 || j == boardSize + 1)
        {
          board[i][j] = TileType.HOLE;
        }
        else if ((i <= 2 || i >= boardSize - 1) && (j <= 2 || j >= boardSize - 1))
        {
          board[i][j] = TileType.FREE;
        }
        else if (Random.Range(0, 5) <= 3) board[i][j] = TileType.FIRE;
        else board[i][j] = TileType.FREE;
      }
    }

    for (int i = 0; i < boardSize + 2; i++)
    {
      for (int j = 0; j < boardSize + 2; j++)
      {
        if (board[i][j] == TileType.HOLE)
        {
          GameObject hole = Instantiate<GameObject>(holePrefab);
          hole.transform.position = GridToVectorPosition(i, j);
        }
        else if (board[i][j] == TileType.FIRE)
        {
          GameObject fire = Instantiate<GameObject>(firePrefab);
          fire.transform.position = GridToVectorPosition(i, j);
        }
      }
    }

    for (int i = 0; i < 2; i++)
    {
      GameObject playerObject = Instantiate<GameObject>(playerPrefab);
      playerObject.GetComponent<PlayerController>().SetButtons(i);
      playerObject.transform.position = GameManager.Instance.GridToVectorPosition(i == 0 ? 1 : 12, i == 0 ? 1 : 12);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public Vector3 GridToVectorPosition(int i, int j)
  {
    return (-6.5f + i) * xAxis + (6.5f - j) * yAxis;
  }

  public Vector2Int VectorToGridPosition(Vector3 pos)
  {
    int i = Mathf.RoundToInt(pos.x / xAxis.x + 6.5f);
    int j = Mathf.RoundToInt(6.5f - pos.y / yAxis.y);
    return new Vector2Int(i, j);
  }

  public Vector3 GetGridPosition(Vector3 pos)
  {
    Vector2Int gridPos = VectorToGridPosition(pos);
    return GridToVectorPosition(gridPos.x, gridPos.y);
  }

  public void SetTile(int i, int j, TileType tile)
  {
    board[i][j] = tile;
  }

  public TileType GetTile(Vector2Int gridPos)
  {
    return board[gridPos.x][gridPos.y];
  }

  public bool CheckPosition(Vector3 pos, TileType tile)
  {
    return GetTile(VectorToGridPosition(pos)) == tile;
  }

  public bool isFree(int i, int j)
  {
    return board[i][j] == TileType.FREE;
  }
}
