using UnityEngine;

public enum TileType {
  NONE,
  FREE,
  HOLE,
  HYDRANT,
  FIRE
}

public class Board {
  private TileType[][] board;
  public int size;

  private Vector3 xAxis;
  private Vector3 yAxis;

  public Board(int boardSize) {
    size = boardSize;
    board = new TileType[boardSize + 2][];
    for (int i = 0;i < boardSize + 2;i++) {
      board[i] = new TileType[boardSize + 2];
    }

    xAxis = Vector3.right * GameManager.Instance.TILE_SIZE;
    yAxis = Vector3.up * GameManager.Instance.TILE_SIZE;
  }

  public void SetTile(int i, int j, TileType tile) {
    if (i < 0 || j < 0 || i > size + 1 || j > size + 1) {
      Debug.Log("Board: SetTile: Out of bounds" + i + " " + j);
      return;
    }
    board[i][j] = tile;
  }

  public void SetTile(Vector2Int grid, TileType tile) {
    SetTile(grid.x, grid.y, tile);
  }

  public TileType GetTile(int i, int j) {
    if (i < 0 || j < 0 || i > size + 1 || j > size + 1) {
      Debug.Log("Board: GetTile: Out of bounds");
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

}