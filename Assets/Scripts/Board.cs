using UnityEngine;

public enum TileType {
  NONE,
  FREE,
  HOLE,
  HYDRANT,
  BONUS,
  FIRE
}

public class Board {
  private TileType[][] board;
  public int size;

  private Vector3 xAxis;
  private Vector3 yAxis;

  private int[][] dangerMatrix;

  public Board(int boardSize) {
    size = boardSize;
    board = new TileType[boardSize + 2][];
    for (int i = 0;i < boardSize + 2;i++) {
      board[i] = new TileType[boardSize + 2];
    }

    dangerMatrix = new int[boardSize + 2][];
    for (int i = 0;i < boardSize + 2;i++) {
      dangerMatrix[i] = new int[boardSize + 2];
      for (int j = 0;j < boardSize + 2;j++) {
        dangerMatrix[i][j] = 0;
      }
    }

    xAxis = Vector3.right * GameManager.Instance.TILE_SIZE;
    yAxis = Vector3.up * GameManager.Instance.TILE_SIZE;
  }

  public void SetTile(int i, int j, TileType tile) {
    if (i < 0 || j < 0 || i > size + 1 || j > size + 1) {
      return;
    }
    board[i][j] = tile;
  }

  public void SetTile(Vector2Int grid, TileType tile) {
    SetTile(grid.x, grid.y, tile);
  }

  public void SetTile(Vector3 position, TileType tile) {
    SetTile(VectorToGridPosition(position), tile);
  }

  public TileType GetTile(int i, int j) {
    if (i < 0 || j < 0 || i > size + 1 || j > size + 1) {
      return TileType.NONE;
    }
    return board[i][j];
  }

  public TileType GetTile(Vector2Int grid) {
    return GetTile(grid.x, grid.y);
  }

  public TileType GetTile(Vector3 position) {
    return GetTile(VectorToGridPosition(position));
  }


  public Vector3 GridToVectorPosition(int i, int j)
  {
    return (-(size / 2 + 0.5f) + i) * xAxis + ((size / 2 + 0.5f) - j) * yAxis;
  }

  public Vector3 GridToVectorPosition(Vector2Int grid) {
    return GridToVectorPosition(grid.x, grid.y);
  }

  public Vector2Int VectorToGridPosition(Vector3 pos) {
    int i = Mathf.RoundToInt(pos.x / xAxis.x + (size / 2 + 0.5f));
    int j = Mathf.RoundToInt((size / 2 + 0.5f) - pos.y / yAxis.y);
    return new Vector2Int(i, j);
  }

  public Vector3 GetGridPosition(Vector3 pos) {
    Vector2Int gridPos = VectorToGridPosition(pos);
    return GridToVectorPosition(gridPos.x, gridPos.y);
  }

  public bool IsWalkable(Vector2Int grid) {
    TileType tile = GetTile(grid);
    return tile == TileType.BONUS || tile == TileType.FREE;
  }

  public bool IsSafe(Vector2Int grid) {
    return GetDanger(grid) == 0;
  }

  public int GetDanger(int i, int j) {
    if (i < 0 || j < 0 || i > size + 1 || j > size + 1) {
      return 0;
    }
    return dangerMatrix[i][j];
  }

  public int GetDanger(Vector2Int grid) {
    return GetDanger(grid.x, grid.y);
  }

  public void SetDanger(int x, int y, int length, int danger) {
    dangerMatrix[x][y] += danger;
    for (int i = 1;i <= length;i++) {
      if (x + i < size + 2) dangerMatrix[x + i][y] += danger;
      if (x - i >= 0) dangerMatrix[x - i][y] += danger;
      if (y + i < size + 2) dangerMatrix[x][y + i] += danger;
      if (y - i >= 0) dangerMatrix[x][y - i] += danger;
    }
  }

  public void SetDanger(Vector2Int grid, int length, int danger) {
    SetDanger(grid.x, grid.y, length, danger);
  }

  public void SetDanger(Vector3 position, int length, int danger) {
    SetDanger(VectorToGridPosition(position), length, danger);
  }
}