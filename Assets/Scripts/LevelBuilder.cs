using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
  private Board board;
  public float waterForce;
  public GameObject playerPrefab;
  public GameObject holePrefab;
  public GameObject firePrefab;
  public GameObject bonusPrefab;
  public GameObject virtualHolePrefab;
  private int ii;
  private int jj;
  private int virtual_hole_qty;
  private int id_direction;
  private bool [][] Matrix_virtual_holes;
  public float time_destroy;
  

  private void Start() {
    BuildLevel();
    ii = 1;
    jj = 1;
    virtual_hole_qty = 0;
    id_direction = 0;
    time_destroy = 10.0f;

    int boardSize = GameManager.Instance.boardSize;
    Matrix_virtual_holes = new bool [boardSize+2][];
    for(int i=0; i < boardSize+2; i++){
      Matrix_virtual_holes[i] = new bool [boardSize+2];
      for(int j=0; j < boardSize+2; j++){
        if(i == 0 || i == 13 || j == 0 || j == 13) Matrix_virtual_holes[i][j] = false;
        else Matrix_virtual_holes[i][j] = true;
      }
    }
  }

  public void BuildLevel() {
    GameManager.Instance.ResetLevel();
    int boardSize = GameManager.Instance.boardSize;
    board = new Board(boardSize);

    // Cria matriz de blocos
    List<GameObject> fireBlocks = new List<GameObject>();
    for (int i = 0;i < boardSize + 2;i++) {
      for (int j = 0; j < boardSize + 2; j++) {
        if (i == 0 || j == 0 || i == boardSize + 1 || j == boardSize + 1) {
          board.SetTile(i, j, TileType.HOLE);
        }
        else if ((i <= 2 || i >= boardSize - 1) && (j <= 2 || j >= boardSize - 1)) {
          board.SetTile(i, j, TileType.FREE);
        }
        else if (Random.Range(0, 5) <= 3) {
          board.SetTile(i, j, TileType.FIRE);
        }
        else board.SetTile(i, j, TileType.FREE);
      }
    }

    GameObject empty = new GameObject();
    GameObject fireObjects = Instantiate<GameObject>(empty, transform);
    fireObjects.name = "Fire Blocks";
    GameObject holeObjects = Instantiate<GameObject>(empty, transform);
    holeObjects.name = "Hole Blocks";

    for (int i = 0; i < boardSize + 2; i++) {
      for (int j = 0; j < boardSize + 2; j++) {
        if (board.GetTile(i, j) == TileType.HOLE) {
          GameObject hole = Instantiate<GameObject>(holePrefab, holeObjects.transform);
          hole.transform.position = board.GridToVectorPosition(i, j);
        }
        else if (board.GetTile(i, j) == TileType.FIRE) {
          GameObject fire = Instantiate<GameObject>(firePrefab, fireObjects.transform);
          fire.transform.position = board.GridToVectorPosition(i, j);
          fireBlocks.Add(fire);
        }
      }
    }

    // Distribui bônus
    for (int i = 0; i < 20; i++) {
      int x = Random.Range(0, fireBlocks.Count - 1);
      fireBlocks[x].GetComponent<FireController>().AddBonus(i < 10 ? BonusType.INCREASE_HYDRANT : BonusType.INCREASE_WATER);
      fireBlocks.RemoveAt(x);
    }

    // Instancia jogadores
    for (int i = 0; i < 2; i++)
    {
      GameObject playerObject = Instantiate<GameObject>(playerPrefab);
      PlayerController playerCtrlr = playerObject.GetComponent<PlayerController>();
      playerCtrlr.SetButtons(i);
      playerCtrlr.board = board;
      playerObject.transform.position = board.GridToVectorPosition(i == 0 ? 1 : 12, i == 0 ? 1 : 12);
      if (i == 0) playerObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      else playerObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    GameManager.Instance.board = board;
  }

  public void SetHole(int x, int y) {
    Matrix_virtual_holes[x][y] = false;
    GameObject virtualHole = Instantiate<GameObject>(virtualHolePrefab);
    virtualHole.transform.position = board.GridToVectorPosition(x, y);
    virtual_hole_qty++;
  }

  public void FixedUpdate(){
    time_destroy -= Time.deltaTime;
    if(time_destroy < 0 && virtual_hole_qty < 144){
      if(id_direction % 4 == 0){
        if(Matrix_virtual_holes[ii][jj]){
          SetHole(ii, jj);
          jj += 1;

          if(!Matrix_virtual_holes[ii][jj]){
            ii += 1;
            jj -= 1;
            id_direction++;
          }
        }
      }
      else if(id_direction % 4 == 1){
        if(Matrix_virtual_holes[ii][jj]) {
          SetHole(ii, jj);
          ii += 1;
        
          if(!Matrix_virtual_holes[ii][jj]){
            ii -= 1;
            jj -= 1;
            id_direction++;
          }
        }
      }
      else if(id_direction % 4 == 2){
        if(Matrix_virtual_holes[ii][jj]){
          SetHole(ii, jj);
          jj -= 1;
          
          if(!Matrix_virtual_holes[ii][jj]){
            ii -= 1;
            jj += 1;
            id_direction++;
          }
        }
      }
      else{
        if(Matrix_virtual_holes[ii][jj]){
          SetHole(ii, jj);
          ii -= 1;
          
          if(!Matrix_virtual_holes[ii][jj]){
            ii += 1;
            jj += 1;
            id_direction++;
          }
        }
      }
      time_destroy = 0.2f;
    }
  }
}
