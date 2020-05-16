using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
  public int boardSize;

  public float maxWaterTime;
  public float waterForce;
  public int id_winner;

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
    DontDestroyOnLoad(this);
  }

  void Start() {
    NewGame();
  }

  void Update() {
  }

  public void NewGame()
  {
    SceneManager.LoadScene("GameScene");
  }

  public void ResetLevel()
  {
    id_winner = -1;
    waterForce = 15.0f;
    xAxis = 21 * Vector3.right;
    yAxis = 21 * Vector3.up;
    boardSize = 12;

    board = new TileType[boardSize + 2][];
    for (int i = 0; i < boardSize + 2; i++)
    {
      board[i] = new TileType[boardSize + 2];
    }
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

  public void SetTile(Vector2Int pos, TileType tile)
  {
    SetTile(pos.x, pos.y, tile);
  }

  public TileType GetTile(Vector2Int gridPos)
  {
    return board[gridPos.x][gridPos.y];
  }

  public bool CheckPosition(int i, int j, TileType tile)
  {
    return GetTile(new Vector2Int(i, j)) == tile;
  }

  public bool CheckPosition(Vector3 pos, TileType tile)
  {
    return GetTile(VectorToGridPosition(pos)) == tile;
  }

  public bool isFree(int i, int j)
  {
    return board[i][j] == TileType.FREE;
  }

  public void SetWinner(int id_player)
  {
    id_winner = id_player;
    SceneManager.LoadScene("EndScene");
  }
}
