using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
  FREE,
  HOLE,
  HYDRANT
}

public class GameManager : MonoBehaviour {

  private static GameManager instance = null;
  private Vector3 xAxis;
  private Vector3 yAxis;
  public GameObject playerPrefab;
  public GameObject holePrefab;

  private TileType[][] board;
  private int boardSize;

  public static GameManager Instance {
    get {
      if (instance == null) {
        instance = new GameObject("GM").AddComponent<GameManager>();
      }
      return instance;
    }
  }

  void Awake() {
    if (instance != null) {
      DestroyImmediate(this);
      return;
    }
    instance = this;
  }

  // Start is called before the first frame update
  void Start() {
    Vector3 size = new Vector3(21, 21, 0);
    xAxis = size.x * Vector3.right;
    yAxis = size.y * Vector3.up;

    boardSize = 12;
    board = new TileType[boardSize + 2][];
    for (int i = 0;i < boardSize + 2;i++) {
      board[i] = new TileType[boardSize + 2];
      for (int j = 0;j < boardSize + 2;j++) {
        if (i == 0 || j == 0 || i == boardSize + 1 || j == boardSize + 1) {
          board[i][j] = TileType.HOLE;
        }
        else board[i][j] = TileType.FREE;
      }
    }

    for (int i = 0;i < boardSize + 2;i++) {
      for (int j = 0;j < boardSize + 2;j++) {
        if (board[i][j] == TileType.HOLE) {
          GameObject hole = Instantiate<GameObject>(holePrefab);
          holePrefab.transform.position = GridToVectorPosition(i, j);
        }
      }
    }

    for(int i=0; i < 1; i++) {
      GameObject playerObject = Instantiate<GameObject>(playerPrefab);
      playerObject.GetComponent<PlayerController>().SetButtons(i);
      playerObject.transform.position = GridToVectorPosition(1, 1);
    }
  }

  // Update is called once per frame
  void Update() {

  }

  public Vector3 GridToVectorPosition(int i, int j) {
    return (-6.5f + i) * xAxis + (6.5f - j) * yAxis;
  }

  public Vector2Int VectorToGridPosition(Vector3 pos) {
    int i = Mathf.RoundToInt(pos.x / xAxis.x + 6.5f);
    int j = Mathf.RoundToInt(6.5f - pos.y / yAxis.y);
    return new Vector2Int(i, j);
  }

  public Vector3 GetGridPosition(Vector3 pos) {
    Vector2Int gridPos = VectorToGridPosition(pos);
    return GridToVectorPosition(gridPos.x, gridPos.y);
  }
}
