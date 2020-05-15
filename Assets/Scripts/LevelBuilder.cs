using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
  public float waterForce;
  public GameObject playerPrefab;
  public GameObject holePrefab;
  public GameObject firePrefab;
  public GameObject bonusPrefab;

  private void Start() {
    BuildLevel();
  }

  public void BuildLevel() {
    GameManager.Instance.ResetLevel();
    int boardSize = GameManager.Instance.boardSize;

    // Cria matriz de blocos
    List<GameObject> fireBlocks = new List<GameObject>();
    for (int i = 0;i < boardSize + 2;i++) {
      for (int j = 0; j < boardSize + 2; j++)
      {
        if (i == 0 || j == 0 || i == boardSize + 1 || j == boardSize + 1)
        {
          GameManager.Instance.SetTile(i, j, TileType.HOLE);
        }
        else if ((i <= 2 || i >= boardSize - 1) && (j <= 2 || j >= boardSize - 1))
        {
          GameManager.Instance.SetTile(i, j, TileType.FREE);
        }
        else if (Random.Range(0, 5) <= 3) {
          GameManager.Instance.SetTile(i, j, TileType.FIRE);
        }
        else GameManager.Instance.SetTile(i, j, TileType.FREE);
      }
    }

    for (int i = 0; i < boardSize + 2; i++)
    {
      for (int j = 0; j < boardSize + 2; j++)
      {
        if (GameManager.Instance.CheckPosition(i, j, TileType.HOLE))
        {
          GameObject hole = Instantiate<GameObject>(holePrefab, transform);
          hole.transform.position = GameManager.Instance.GridToVectorPosition(i, j);
        }
        else if (GameManager.Instance.CheckPosition(i, j, TileType.FIRE))
        {
          GameObject fire = Instantiate<GameObject>(firePrefab);
          fire.transform.position = GameManager.Instance.GridToVectorPosition(i, j);
          fireBlocks.Add(fire);
        }
      }
    }

    // Distribui bônus
    for (int i = 0; i < 20; i++)
    {
      int x = Random.Range(0, fireBlocks.Count - 1);
      fireBlocks[x].GetComponent<FireController>().AddBonus(i < 10 ? BonusType.INCREASE_HYDRANT : BonusType.INCREASE_WATER);
      fireBlocks.RemoveAt(x);
    }

    // Instancia jogadores
    for (int i = 0; i < 2; i++)
    {
      GameObject playerObject = Instantiate<GameObject>(playerPrefab);
      playerObject.GetComponent<PlayerController>().SetButtons(i);
      playerObject.transform.position = GameManager.Instance.GridToVectorPosition(i == 0 ? 1 : 12, i == 0 ? 1 : 12);
      if (i == 0) playerObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      else playerObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }
  }
}
